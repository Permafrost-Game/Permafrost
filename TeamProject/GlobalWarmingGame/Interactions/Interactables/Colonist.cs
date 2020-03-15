﻿using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables
{
    public class Colonist : AnimatedSprite, IPathFindable, IInstructionFollower, IInteractable, Engine.IUpdatable, IReconstructable
    {
        private const float COLONIST_FRAME_TIME = 100f;
        private const int COLONIST_DEFAULT_INVENTORY_SIZE = 100;

        #region Instruction

        public List<InstructionType> InstructionTypes { get; }
        private readonly Queue<Instruction> instructions;

        [PFSerializable]
        public readonly Inventory inventory;
        public event EventHandler<ResourceItem> InventoryChange = delegate { };

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
        public int AttackSpeed { get; set; }
        public float AttackPower { get; set; }
        public float AttackRange { get; set; }
        public float MaxHealth { get; private set; }

        private bool toBeRemoved = false;
        private bool isDead = false;
        private bool combatModeOn = false;
        public Vector2 lastPosition;
        Enemy enemy = null;

        private bool _inCombat = false;
        public bool InCombat
        {
            get { return _inCombat; }
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
        private bool deathSoundPlayed;

        #region PathFinding
        public Queue<Vector2> Goals { get; set; } = new Queue<Vector2>();
        public Queue<Vector2> Path { get; set; } = new Queue<Vector2>();
        public float Speed { get; set; }
        public double ColonistimeToAttack { get; private set; }
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

            this.inventory.InventoryChange += InvokeInventoryChange;
            

            AttackRange = 70;
            AttackPower = 30;
            AttackSpeed = 1000;
            lastPosition = position;


            Speed = 0.25f;
            MaxHealth = 100f;
            Health = MaxHealth;

            timeUntillNextHungerCheck = BASE_FOOD_CONSUMPTION;
            timeToFreezeCheck = timeUntillNextFreezeCheck;

            instructions = new Queue<Instruction>();
            InstructionTypes = new List<InstructionType>();

        }

        private void InvokeInventoryChange(Object sender, ResourceItem resourceItem)
        {
            InventoryChange.Invoke(this, resourceItem);
        }

        internal void SetDead()
        {
            this.Rotation = 1.5f;
            this.isDead = true;
            isAnimated = false;
            if (!deathSoundPlayed)
            { 
                SoundFactory.PlaySoundEffect(Sound.colonistDying);
                deathSoundPlayed = true;
            }

            Task.Delay(new TimeSpan(0, 0, 2)).ContinueWith(o =>
            {
                toBeRemoved = true;
            });
        }


        public void OnGoalComplete(Vector2 completedGoal)
        {
            if (Goals.Count == 0
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
            if(toBeRemoved)
            {
                GameObjectManager.Remove(this);
                return;
            }

            Vector2 lastPosition = this.Position;
            Move(gameTime);
            base.Update(gameTime);
            enemy = GlobalCombatDetector.FindColonistThreat(this);

            Vector2 delta = lastPosition - this.Position;


            if (delta.Equals(Vector2.Zero))
            {
                if (!isAttacking)
                {
                    //TextureGroupIndex = 0;
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
                instructions.Peek().Update(gameTime);
                if (instructions.Count > 0)
                {
                    if (Goals.Count == 0)
                    {
                        Goals.Enqueue(instructions.Peek().PassiveMember.Position);
                    }
                }
            }

            if (enemy != null)
            {
                combatModeOn = true;
                SpriteEffect = enemy.Position.X < this.Position.X ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }
            else
            {
                combatModeOn = false;

            }
            if (combatModeOn)
            {
                PerformCombat(gameTime, enemy);
            }

            if (this.Health <= 0 && !isDead)
            {
                this.SetDead();
            }

            FreezeCheck(gameTime);
            HungerCheck(gameTime);

        }

        private void PerformCombat(GameTime gameTime, Enemy enemy)
        {

            

                if (enemy.Health > 0 && this.Health > 0)
                {
                    InCombat = true;
                  
                    ColonistAttack(gameTime);
                }
            

        }

        #region Colonist Freeze Check
        /// <summary>
        /// Is the colonist currently freezing to death?
        /// </summary>
        /// <param name="gameTime">game time</param>
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
        /// <param name="gameTime">game time</param>
        private void HungerCheck(GameTime gameTime)
        {
            timeUntillNextHungerCheck -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeUntillNextHungerCheck < 0)
            {
                //If colonist doesn't have food on them, they are starving -1 health
                if (!Inventory.RemoveItem(new ResourceItem(Resource.Food, 1)))
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
        /// Adjust the colonist's temperature based on the tile they are over and their lower comfort range
        /// </summary>
        /// <param name="tileTemp">temperature of the tile below the colonist</param>
        public void UpdateTemp(float tileTemp)
        {
            //If tile temperature is greater than the colonists temperature and greater than LowerComfortRange
            //Increase the colonist's temperature by 2
            //clamp the colonist's temperature with the tile's temperature as the max 
            if (tileTemp > Temperature.Value && tileTemp > LowerComfortRange)
            {
                Temperature.Value = MathHelper.Clamp(Temperature.Value + 2, Temperature.Min, tileTemp);
            }
            //If tile temperature is less than the colonists temperature and less than LowerComfortRange
            //Decrease the colonist's temperature by 1
            //clamp the colonist's temperature with the tile's temperature as the min
            else if (tileTemp < Temperature.Value && tileTemp < LowerComfortRange)
            {
                Temperature.Value = MathHelper.Clamp(Temperature.Value - 1, tileTemp, Temperature.Max);
            }
        }
        #endregion

        public void AddInstruction(Instruction instruction, int priority = 0)
        {
            //TODO implement priority
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

        private void CheckInventoryDump()
        {
            foreach(StorageUnit storageUnit in GameObjectManager.Filter<StorageUnit>())
            {
                if(storageUnit.ResourceItem != null && inventory.ContainsType(storageUnit.ResourceItem.ResourceType.ResourceID))
                {
                    AddInstruction(new Instruction(
                        type: storageUnit.StoreInstruction,
                        activeMember: this,
                        passiveMember: storageUnit
                        )
                    );
                    break;
                }
            }
        }

        private void OnInstructionComplete(Instruction instruction)
        {
            if (instructions.Peek() == instruction)
            {
                instructions.Dequeue();
                CheckInventoryDump();
                if (!InCombat)
                {
                    TextureGroupIndex = 0;
                    
                }
            }
            else
            {
                throw new Exception("Async instruction completed");
            }
        }

        private void ColonistAttack(GameTime gameTime)
        {

            if (ColonistAttackSpeedControl(gameTime))
            {
                this.isAttacking = true;
                SoundFactory.PlaySoundEffect(Sound.slashSound);
                enemy.Health = enemy.Health - this.AttackPower;
                if (enemy.Health<=0)
                {
                    this.InCombat = false;
                    this.isAttacking = false;

                }
            }
            
        }

        private bool ColonistAttackSpeedControl(GameTime gameTime)
        {
            ColonistimeToAttack = ColonistimeToAttack + gameTime.ElapsedGameTime.TotalMilliseconds;

            // Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            if (ColonistimeToAttack > 500 & ColonistimeToAttack < 600)
            {
                isAttacking = false;
                TextureGroupIndex = 2;
            }
            if (ColonistimeToAttack >= this.AttackSpeed)
            {
                ColonistimeToAttack = 0;
                return true;



            }
            return false;


        }
        public object Reconstruct()
        {
            return new Colonist(PFSPosition, (TextureSetTypes)textureSetID, inventory);
        }

    }
}



