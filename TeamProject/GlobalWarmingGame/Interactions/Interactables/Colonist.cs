using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using GlobalWarmingGame.Action;
using Engine;
using GlobalWarmingGame;
using Engine.TileGrid;
using GlobalWarmingGame.ResourceItems;
using System.Threading.Tasks;
using GlobalWarmingGame.Resources.ResourceTypes;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Colonist : AnimatedSprite, IPathFindable, IInteractable, IUpdatable
    {

        public List<InstructionType> InstructionTypes { get; }
        public Inventory Inventory { get; }
        private Queue<Instruction> instructions;

        public float Health { get; set; }
        public int attackSpeed { get; set; }
        public string Name { get; private set; }
        public float AttackPower { get; set; }
        public float attackRange { get; set; }
        public float MaxHealth { get; private set; }
        public Temperature Temperature { get; set; } = new Temperature(38);
        private readonly float CoreBodyTemperature = 38;
        public int UpperComfortRange { get; private set; } = 40;
        public int LowerComfortRange { get; private set; } = 15;

        public Boolean colonistDead = false;


        private static readonly float Base_Consumption_Rate = 60000f;
        private float timeUntillFoodTick;
        private float timeUntillTemperature = 2000f;
        private float timeToTemperature;
        private float timeUntilTemperatureUpdate = 2000f;
        private float timeToTemperatureUpdate;
        private bool _inCombat  = false;
        public bool inCombat
        {
            get { return _isAttacking; }
            set
            {
                _inCombat = value;
                if (value == false)
                {
                    TextureGroupIndex = 0;
                }
                
            }
        }
        private bool _isAttacking  = false;
        public bool isAttacking {
            get { return _isAttacking; }
            set { _isAttacking = value;
                isAnimated = true;
                SpriteEffect = SpriteEffects.None;
                TextureGroupIndex = _isAttacking ? 1 : 0;

            }
        }

        #region IPathFindable
        public Queue<Vector2> Goals { get; set; } = new Queue<Vector2>();
        public Queue<Vector2> Path { get; set; } = new Queue<Vector2>();
        public float Speed { get; set; }
        #endregion


        public Colonist(Vector2 position, Texture2D[][] textureSet, float inventoryCapacity) : base
        (
            position: position,
            size: new Vector2(textureSet[0][0].Width, textureSet[0][0].Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Colonist",
            depth: 1f,
            textureSet: textureSet,
            frameTime: 100f
        )
        {
            attackRange = 60;
            AttackPower = 30;
            attackSpeed = 1000;

            Speed = 0.5f;
            MaxHealth = 100f;
            Health = MaxHealth;
            Inventory = new Inventory(inventoryCapacity);
            Temperature.Value = CoreBodyTemperature;
            timeUntillFoodTick = Base_Consumption_Rate;
            timeToTemperature = timeUntillTemperature;
            timeToTemperatureUpdate = timeUntilTemperatureUpdate;

            instructions = new Queue<Instruction>();
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("select", "Select Colonist", "Selects this colonist")
            };
        }

        internal void setDead()
        {
            this.Rotation = 1.5f;
            colonistDead = true;

            #region Start 2 Seconds Delay for 'Animation'
            Task.Delay(new TimeSpan(0, 0, 2)).ContinueWith(o =>
            {
                GameObjectManager.Remove(this);
            });
            #endregion
        }

        public void AddInstruction(Instruction instruction)
        {
            instructions.Enqueue(instruction);
        }

        public void OnGoalComplete(Vector2 completedGoal)
        {
            if (instructions.Count > 0 &&
                //Since the instruction is identified by the goal, this may cause problems if two instructions have the same goal position.
                completedGoal == (((GameObject)instructions.Peek().PassiveMember).Position) &&
                instructions.Count != 0)
            {
                Instruction currentInstruction = instructions.Peek();
                currentInstruction.Type.Act(this);

                if (currentInstruction.Type.ResourceItem != null)
                    Inventory.AddItem(currentInstruction.Type.ResourceItem);

                instructions.Dequeue();
            }
        }

        public Boolean isColonistDead()
        {
            return colonistDead;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 position1 = this.Position;
            this.Position += PathFindingHelper.CalculateNextMove(gameTime, this);
            base.Update(gameTime);

            Vector2 delta = position1 - this.Position;

           
            if (delta.Equals(Vector2.Zero))
            {
                if (!isAttacking)
                {
                    isAnimated = false;
                }
            }
            else if (Math.Abs(delta.X) >= Math.Abs(delta.Y))
            {

                isAnimated = true;
                TextureGroupIndex = 2;
                SpriteEffect = (delta.X > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                
            }

            if (Goals.Count == 0 && instructions.Count > 0)
                Goals.Enqueue(((GameObject)instructions.Peek().PassiveMember).Position);

            TemperatureCheck(gameTime);
            HungerCheck(gameTime);
        }



        #region Colonist Temperature Check
        private void TemperatureCheck(GameTime gameTime)
        {
            //Temperature affecting colonist's health
            timeToTemperature -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToTemperature < 0f)
            {
                if (Temperature.Value < (LowerComfortRange - 5) || Temperature.Value > (UpperComfortRange + 10))
                {
                    //Health -= 1;
                }
                timeToTemperature = timeUntillTemperature;
            }
        }
        #endregion

        #region Colonist Hunger Check
        private void HungerCheck(GameTime gameTime)
        {
            //Temperature affecting food
            timeUntillFoodTick -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Double foodFormula = (1 + Temperature.Value / CoreBodyTemperature);

            if (foodFormula <= 0.25)
            {
                foodFormula = 0.25;
            }
            //TODO uncomment
            // foodFormula is a multiplier on the timeUntillFoodTick
            if ((timeUntillFoodTick * foodFormula) < 0)
            {
                FoodTick();
                timeUntillFoodTick = Base_Consumption_Rate;
            }
        }
        private void FoodTick()
        {
            //If colonist doesn't have food on them, they are starving -1 health
            ResourceItem food = new ResourceItem(new Food(), 1);
            if (!Inventory.RemoveItem(food))
            {
                //TODO uncomment
                //Health -= 1;
            }
            else
            {
                //((DisplayLabel)GameObjectManager.GetObjectsByTag("lblFood")[0]).Value -= 1;
            }
        }
        #endregion

        #region Update Temperature
        public void UpdateTemp(float tileTemp, GameTime gameTime)
        {
            //Adjust the colonist's temperature based on the tile they are over
            timeToTemperatureUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToTemperatureUpdate < 0f)
            {
                if (tileTemp > CoreBodyTemperature)
                {
                    Temperature.Value = Temperature.Value + (tileTemp / 10);
                    //Colonist's temperature should be able to be greater than the tile they are over
                    Temperature.Value = MathHelper.Clamp(Temperature.Value, -100, tileTemp);
                    //Console.Out.WriteLine("Greater" + Temperature.Value + " t:" + tileTemp + " core: " + CoreBodyTemperature + " h: " + Health);
                }
                else
                {
                    Temperature.Value = Temperature.Value - 1;
                    //Colonist's temperature should be able to be lower than the tile they are over
                    Temperature.Value = MathHelper.Clamp(Temperature.Value, tileTemp, 100);
                    //Console.Out.WriteLine("Lower" + Temperature.Value + " t:" + tileTemp + " core: " + CoreBodyTemperature + " h: " + Health);
                }

                timeToTemperatureUpdate = timeUntilTemperatureUpdate;
            }
        }
        #endregion

    }
}

