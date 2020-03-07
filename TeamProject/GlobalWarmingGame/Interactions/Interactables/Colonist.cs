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
        public float UpperComfortRange { get; private set; } = 50;
        public float LowerComfortRange { get; private set; } = 15;
        private float timeToFreezeCheck;
        private readonly float timeUntillNextFreezeCheck = 2000f;
        #endregion

        #region Food
        public int Hunger { get; private set; } = 0;
        private float timeUntillNextHungerCheck;
        private readonly float BASE_FOOD_CONSUMPTION = 12000f;
        #endregion

        #region PathFinding
        public Queue<Vector2> Goals { get; set; } = new Queue<Vector2>();
        public Queue<Vector2> Path { get; set; } = new Queue<Vector2>();
        public float Speed { get; set; }
        #endregion

        public Colonist() : this(Vector2.Zero, TextureSetTypes.colonist)
        {

        }

        public Colonist(Vector2 position, TextureSetTypes textureSetType, Inventory inventory = null, int capacity = 100) : base
        (
            position: position,
            size: new Vector2(Textures.MapSet[textureSetType][0][0].Width, Textures.MapSet[textureSetType][0][0].Height),
            rotation: 0f,
            origin: new Vector2(Textures.MapSet[textureSetType][0][0].Width / 2, Textures.MapSet[textureSetType][0][0].Height / 2),
            tag: "Colonist",
            depth: 0f,
            textureSet: Textures.MapSet[textureSetType],
            frameTime: 100f
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

            Temperature.Value = 37;
            timeUntillNextHungerCheck = BASE_FOOD_CONSUMPTION;
            timeToFreezeCheck = timeUntillNextFreezeCheck;

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
            if (Goals.Count == 0
                && instructions.Count > 0
                && completedGoal == (instructions.Peek().PassiveMember.Position)
                    )
            {
                Instruction currentInstruction = instructions.Peek();
                currentInstruction.Start();
                if (currentInstruction.Type.TimeCost > 0)
                {
                    TextureGroupIndex = 1;
                    isAnimated = true;
                }
            }
        }


        private void Move(GameTime gameTime)
        {
            Position += PathFindingHelper.CalculateNextMove(gameTime, this);
            depth = (Position.Y + 0.5f + (Position.X + 0.5f / 2)) / 48000f; // "+ 0.5f" stops Z Fighting
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

            FreezeCheck(gameTime);
            HungerCheck(gameTime);
        }

        #region Colonist Freeze Check
        /// <summary>
        /// Is the colonist currently freezing to death?
        /// </summary>
        /// <param name="gameTime"></param>
        private void FreezeCheck(GameTime gameTime)
        {
            //Temperature affecting colonist's health
            timeToFreezeCheck -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToFreezeCheck < 0f)
            {
                if (Temperature.Value < LowerComfortRange || Temperature.Value > UpperComfortRange)
                {
                    Health -= 1;
                }
                timeToFreezeCheck = timeUntillNextFreezeCheck;
            }
        }
        #endregion

        #region Colonist Hunger Check
        /// <summary>
        /// Is the colonist currently starving?
        /// </summary>
        /// <param name="gameTime"></param>
        private void HungerCheck(GameTime gameTime)
        {
            timeUntillNextHungerCheck -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeUntillNextHungerCheck < 0)
            {
                //If colonist doesn't have food on them, they are starving -1 health
                if (!Inventory.RemoveItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Food), 1)))
                {
                    //If the colonist is hungry they take health damage and reset else increase hunger
                    if (Hunger == 5)
                    {
                        Health -= 1;
                    }
                    else
                    {
                        Hunger++;
                    }
                }
                else 
                {
                    //Food was eaten, reset hunger
                    Hunger = 0;
                }

                //If colonist's current temperature is lower than their acceptable lowerComfortRange
                //they will have their next hunger check 40% faster
                if (Temperature.Value < LowerComfortRange)
                {
                    timeUntillNextHungerCheck = BASE_FOOD_CONSUMPTION * 0.6f;
                }
                else 
                {
                    timeUntillNextHungerCheck = BASE_FOOD_CONSUMPTION;
                }
            }
        }
        #endregion

        #region Update Temperature
        /// <summary>
        /// Adjust the colonist's temperature based on the tile they are over
        /// </summary>
        /// <param name="tileTemp"></param>
        public void UpdateTemp(float tileTemp)
        {
            //If tile temperature is greater than the colonists temperature
            //clamp the colonist's temperature with the tile's temperature as the max 
            //else clamp the colonist's temperature with the tile's temperature as the min
            if (tileTemp > Temperature.Value && tileTemp > 0)
            {
                //Gain 1/10th of the tiles temperature.
                Temperature.Value = Temperature.Value + (tileTemp / 10);
                //Colonist's temperature should be able to be greater than the tile they are over
                Temperature.Value = MathHelper.Clamp(Temperature.Value, Temperature.Min, tileTemp);
            }
            else if (tileTemp < Temperature.Value && tileTemp <= 0)
            {
                Temperature.Value = Temperature.Value - 1;
                //Colonist's temperature should be able to be lower than the tile they are over
                Temperature.Value = MathHelper.Clamp(Temperature.Value, tileTemp, Temperature.Max);
            }
        }
        #endregion

        public void AddInstruction(Instruction instruction, int priority)
        {
            instruction.OnComplete.Add(OnInstructionComplete);
            instructions.Enqueue(instruction);

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
