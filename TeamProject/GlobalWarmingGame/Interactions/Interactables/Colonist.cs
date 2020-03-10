using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables
{
    public class Colonist : AnimatedSprite, IPathFindable, IInstructionFollower, IInteractable, IUpdatable, IStorage, IReconstructable
    {
        private const float COLONIST_FRAME_TIME = 100f;
        private const int COLONIST_DEFAULT_INVENTORY_SIZE = 100;

        #region Instruction

        public List<InstructionType> InstructionTypes { get; }
        private readonly Queue<Instruction> instructions;

        [PFSerializable]
        public readonly Inventory inventory;

        public Inventory Inventory { get => inventory; }

        [PFSerializable]
        public readonly int textureSetID;

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        #endregion

        #region Combat
        public float Health { get; set; }
        public int attackSpeed { get; set; }
        public float AttackPower { get; set; }
        public float attackRange { get; set; }
        public float MaxHealth { get; private set; }
        public bool ColonistDead { get; set; } = false;

        private bool _inCombat = false;
        public bool inCombat
        {
            get { return _isAttacking; }
            set
            {
                _inCombat = value;
                if (value == false)
                {
                    //TextureGroupIndex = 0;
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
                //isAnimated = true;
                SpriteEffect = SpriteEffects.None;
                //TextureGroupIndex = _isAttacking ? 1 : 0;

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
        #endregion

        public Colonist() : this(position: Vector2.Zero) { }

        public Colonist(Vector2 position, TextureSetTypes textureSetType = TextureSetTypes.colonist, Inventory inventory = default, int capacity = COLONIST_DEFAULT_INVENTORY_SIZE) : base
        (
            position: position,
            textureSet: Textures.MapSet[textureSetType],
            frameTime: COLONIST_FRAME_TIME
        )
        {
            textureSetID = (int)textureSetType;

            if (inventory == null)
                this.inventory = new Inventory(capacity);
            else
                this.inventory = inventory;

            attackRange = 60;
            AttackPower = 30;
            attackSpeed = 1000;


            Speed = 0.25f;
            MaxHealth = 100f;
            Health = MaxHealth;
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


        public void OnGoalComplete(Vector2 completedGoal)
        {
            if( Goals.Count == 0
                && instructions.Count > 0 
                && completedGoal == (instructions.Peek().PassiveMember.Position)
                    )
            {
                Instruction currentInstruction = instructions.Peek();
                try
                {
                    currentInstruction.Start();
                }
                catch (InvalidInstruction e)
                {
                    OnInstructionComplete(e.instruction);
                }
                
            }
        }


        private void Move(GameTime gameTime)
        {
            Position += PathFindingHelper.CalculateNextMove(gameTime, this);
            UpdateDepth(0.5f);
        }


        public override void Update(GameTime gameTime)
        {
            Vector2 position1 = Position;
            Move(gameTime);
            base.Update(gameTime);

            Vector2 delta = position1 - Position;


            if (delta.Equals(Vector2.Zero))
            {
                if (!isAttacking)
                {
                    //isAnimated = false;
                }
            }
            else if (Math.Abs(delta.X) >= Math.Abs(delta.Y))
            {
                isAnimated = true;
                TextureGroupIndex = 2;
                SpriteEffect = (delta.X > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }

            if (instructions.Count > 0)
            {
                if (Goals.Count == 0)
                {
                    Goals.Enqueue(instructions.Peek().PassiveMember.Position);
                }
                instructions.Peek().Update(gameTime);
            }



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
                timeUntillFoodTick = BASE_FOOD_CONSUMPTION;
            }
        }
        private void FoodTick()
        {
            //If colonist doesn't have food on them, they are starving -1 health
            ResourceItem food = new ResourceItem(ResourceTypeFactory.GetResource(Resource.Food), 1);
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

        public void AddInstruction(Instruction instruction, int priority)
        {
            instruction.OnStart.Add(OnInstructionStart);
            instruction.OnComplete.Add(OnInstructionComplete);
            instructions.Enqueue(instruction);

        }

        private void OnInstructionStart(Instruction instruction)
        {
            if (instructions.Peek() == instruction)
            {
                if (instruction.Type.TimeCost > 0)
                {
                    TextureGroupIndex = 1;
                    isAnimated = true;
                }
            }
            else
            {
                throw new Exception("Async instruction started");
            }
        }

        private void OnInstructionComplete(Instruction instruction)
        {
            if (instructions.Peek() == instruction)
            {
                instructions.Dequeue();

                TextureGroupIndex = 0;
            }
            else
            {
                throw new Exception("Async instruction completed");
            }
        }

        public object Reconstruct()
        {
            return new Colonist(PFSPosition, (TextureSetTypes)textureSetID, inventory);
        }

    }
}
