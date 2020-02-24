using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables
{
    public class Colonist : AnimatedSprite, IPathFindable, IInstructionFollower, IInteractable, IUpdatable, IStorage
    {
        #region Instruction

        public List<InstructionType> InstructionTypes { get; }
        private Queue<Instruction> instructions;
        public Inventory Inventory { get; }

        #endregion

        #region Combat
        public float Health { get; set; }
        public int attackSpeed { get; set; }
        public float AttackPower { get; set; }
        public float attackRange { get; set; }
        public float MaxHealth { get; private set; }
        public bool ColonistDead { get; set; } = false;
        private bool combatModeOn = false;
        Enemy enemy = null;

        private bool _inCombat = false;
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
        private bool _isAttacking = false;
        public bool isAttacking
        {
            get { return _isAttacking; }
            set
            {
                _isAttacking = value;
                isAnimated = true;
                SpriteEffect = SpriteEffects.None;
                TextureGroupIndex = _isAttacking ? 1 : 0;

            }
        }
        #endregion

        #region Temperature

        public Temperature Temperature { get; set; } = new Temperature(38);
        private readonly float CoreBodyTemperature = 38;
        public int UpperComfortRange { get; private set; } = 40;
        public int LowerComfortRange { get; private set; } = 15;
        #endregion

        #region Food
        private static readonly float BASE_FOOD_CONSUMPTION = 60000f;
        private float timeUntillFoodTick;
        private float timeUntillTemperature = 2000f;
        private float timeToTemperature;
        private float timeUntilTemperatureUpdate = 2000f;
        private float timeToTemperatureUpdate;
        #endregion

        #region PathFinding
        public Queue<Vector2> Goals { get; set; } = new Queue<Vector2>();
        public Queue<Vector2> Path { get; set; } = new Queue<Vector2>();
        public float Speed { get; set; }
        public double ColonistimeToAttack { get; private set; }
        #endregion


        public Colonist(Vector2 position, Texture2D[][] textureSet, float inventoryCapacity = 100) : base
        (
            position: position,
            size: new Vector2(textureSet[0][0].Width, textureSet[0][0].Height),
            rotation: 0f,
            origin: new Vector2(textureSet[0][0].Width / 2, textureSet[0][0].Height / 2),
            tag: "Colonist",
            depth: 0f,
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
            timeUntillFoodTick = BASE_FOOD_CONSUMPTION;
            timeToTemperature = timeUntillTemperature;
            timeToTemperatureUpdate = timeUntilTemperatureUpdate;

            instructions = new Queue<Instruction>();
            InstructionTypes = new List<InstructionType>();
            
        }

        internal void setDead()
        {
            this.Rotation = 1.5f;
            ColonistDead = true;

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
                currentInstruction.Type.Start(this);

                //if (currentInstruction.Type.ResourceItem != null)
                //    Inventory.AddItem(currentInstruction.Type.ResourceItem);

                instructions.Dequeue();
            }
        }


        public override void Update(GameTime gameTime)
        {
            Vector2 position1 = this.Position;
            Position += PathFindingHelper.CalculateNextMove(gameTime, this);
            depth = (Position.Y + 0.5f + (Position.X + 0.5f / 2)) / 48000f; // "+ 1f" stops Z Fighting
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
            enemy = GlobalCombatDetector.FindColonistThreat(this);

            if (enemy!=null)
            {
                combatModeOn = true;
            }
            else {
                combatModeOn = false;
            }

            if (combatModeOn)
            {
                performCombat(gameTime, enemy);
            }


        }

        private void performCombat(GameTime gameTime, Enemy enemy)
        {
            if (enemy != null)
            {


                if (enemy.Health > 0 & this.Health > 0)
                {
                    inCombat = true;
                    enemy.setInCombat(true);
                    ColonistAttack(gameTime);
                }
                else
                {
                    this.inCombat = false;
                }
                
            }





            #region Colonist Temperature Check
           /* private void TemperatureCheck(GameTime gameTime)
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
            }*/
            #endregion

          /*  #region Colonist Hunger Check
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
                    timeUntillFoodTick = BASE_FOOD_CONSUMPTION;
                }
            }
            private void FoodTick()
            {
                //If colonist doesn't have food on them, they are starving -1 health
                ResourceItem food = new ResourceItem(ResourceTypeFactory.MakeResource(Resource.Food), 1);
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

            public void AddInstruction(Instruction instruction, int priority)
            {
                instructions.Enqueue(instruction);
            }
            #endregion

    */
        }

        private void ColonistAttack(GameTime gameTime)
        {
            
                if (ColonistAttackSpeedControl(gameTime))
                {
                    this.isAttacking = true;
                    enemy.Health = enemy.Health - this.AttackPower;
                }
                Console.WriteLine("Colonist hp: " + this.Health + " Enemy hp: " + enemy.Health);



                if (enemy.Health <= 0)
                {
                    enemy.SetEnemyDead();
                    this.inCombat = false;
                    this.isAttacking = false;
                    enemy = null;
                    
                }
            }

        private bool ColonistAttackSpeedControl(GameTime gameTime)
        {
            ColonistimeToAttack = ColonistimeToAttack + gameTime.ElapsedGameTime.TotalMilliseconds;

            Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            if (ColonistimeToAttack > 500 & ColonistimeToAttack < 600)
            {
                this.isAttacking = false;
            }
            if (ColonistimeToAttack >= this.attackSpeed)
            {
                ColonistimeToAttack = 0;
                return true;



            }
            return false;

        }

        public void AddInstruction(Instruction instruction, int priority)
        {
            throw new NotImplementedException();
        }
    }
}
