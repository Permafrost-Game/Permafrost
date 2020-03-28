using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.UI.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables
{
    public class Colonist : AnimatedSprite, IPathFindable, IInstructionFollower, IInteractable, Engine.IUpdatable, IReconstructable
    {
        private const float COLONIST_FRAME_TIME = 100f;
        private const int COLONIST_DEFAULT_INVENTORY_SIZE = 100;
        private static readonly Random random = new Random();

        #region Instruction

        public List<InstructionType> InstructionTypes { get; }
        private readonly SimplePriorityQueue<Instruction> instructions;

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
                    IsAttacking = false;
                }

            }
        }
        private bool _ranged = false;
        public bool Ranged
        {
            get { return _ranged; }
            set
            {
                _ranged = value;
                if (value == false)
                {
                    AttackRange = 70;
                    AttackPower = 30;
                    AttackSpeed = 1000;
                }
                else {
                    AttackRange = 350;
                    AttackPower = 45;
                    AttackSpeed = 1500;
                }

            }
        }
        private bool _isAttacking = false;
        public bool IsAttacking
        {
            get { return _isAttacking; }
            set
            {
                _isAttacking = value;
                isAnimated = true;
                SpriteEffect = SpriteEffects.None;
                if (Ranged)
                {
                    TextureGroupIndex = _isAttacking ? 3 : 0;
                }
                else {
                    TextureGroupIndex = _isAttacking ? 1 : 0;
                }
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
        private bool AttackTrigger=false;
        private Enemy target;

        public bool HasRangedItem { get; set; } = false;


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

            instructions = new SimplePriorityQueue<Instruction>();
            InstructionTypes = new List<InstructionType>();

        }

        private void InvokeInventoryChange(Object sender, ResourceItem resourceItem)
        {
            InventoryChange.Invoke(this, resourceItem);
            Ranged = inventory.ContainsType(Resource.Shotgun);
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
                && this.Position == (instructions.First.PassiveMember.Position)
                )
            {
                Instruction currentInstruction = instructions.First;
                try
                {
                    currentInstruction.Start();    
                }
                catch (InvalidInstruction e)
                {
                    //instruction Failed
                    OnInstructionComplete(e.instruction);
                }
                
            }

        }

        private bool CheckRequireResources(Instruction instruction)
        {
            List<Instruction> instructionsToEnqueue = new List<Instruction>();

            IEnumerable<StorageUnit> storageUnits = GameObjectManager.Filter<StorageUnit>();
            Queue<ResourceItem> requiredItems = new Queue<ResourceItem>(instruction.Type.RequiredResources.Select(i => i.Clone()));

            
            while(requiredItems.Count > 0)
            {
                ResourceItem requiredItem = requiredItems.Dequeue();
                if (inventory.ContainsType(requiredItem.ResourceType.ResourceID))
                {
                    requiredItem.Weight -= inventory.Resources[requiredItem.ResourceType.ResourceID].Weight;
                }
                foreach (StorageUnit s in storageUnits)
                {
                    if (s.ResourceItem != null
                     && s.ResourceItem.ResourceType.Equals(requiredItem.ResourceType))
                    {
                        int amountToTake = Math.Min(requiredItem.Weight, s.ResourceItem.Weight);
                        if(amountToTake > 0)
                        {
                            instructionsToEnqueue.Add(new Instruction(
                                                            type: s.TakeItemInstruction(amountToTake),
                                                            priorityOverride: instruction.Priority - 1,
                                                            activeMember: this,
                                                            passiveMember: s
                                                            ));
                            requiredItem.Weight -= amountToTake;
                        }

                        if (requiredItem.Weight <= 0)
                        {
                            break;
                        }

                    }
                }
                if(requiredItem.Weight > 0)
                {
                    return false;
                }
                
            }
            
            foreach(Instruction i in instructionsToEnqueue)
            {
                AddInstruction(i);
            }
            return true;
        }

        private void Move(GameTime gameTime)
        {
            if (!isDead)
            {
                Position += PathFindingHelper.CalculateNextMove(gameTime, this);
                UpdateDepth(0.5f);
            }
        }


        public override void Update(GameTime gameTime)
        {
            if(toBeRemoved)
            {
                List<ResourceItem> droppedItems= new List<ResourceItem>();
                foreach (ResourceItem item in inventory.Resources.Values) {
                    droppedItems.Add(item);
                }

                GameObjectManager.Add(new Loot(droppedItems, this.Position));
                GameObjectManager.Remove(this);
                return;
            }

            Vector2 lastPosition = this.Position;
            Move(gameTime);
            base.Update(gameTime);
            enemy = GlobalCombatDetector.FindColonistThreat(this);
            if (AttackTrigger)
            {
                Hunt(enemy);
            }
            Vector2 delta = lastPosition - this.Position;


            if (delta.Equals(Vector2.Zero))
            {
                if (!IsAttacking && instructions.Count==0)
                {
                    TextureGroupIndex = 0;
                    
                   
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
                try
                {
                    instructions.First.Update(gameTime);
                }
                catch (InvalidInstruction e)
                {
                    OnInstructionComplete(e.instruction);
                }
                
                if (instructions.Count > 0)
                {
                    if (Goals.Count == 0 )
                    {
                        Instruction i1 = instructions.First;
                        if ((!inventory.ContainsAll(i1.Type.RequiredResources)
                            && CheckRequireResources(i1))
                            | inventory.ContainsAll(instructions.First.Type.RequiredResources)) //instructions.First may have been changed by the previous line.
                        {
                            Goals.Enqueue(instructions.First.PassiveMember.Position);
                        }
                        else
                        {
                            //No valid resources
                            GameUIController.ResourceNotification(instructions.Dequeue());
                            
                            
                        }
                        
                    }
                }
            }

            if (enemy != null && enemy.notDefeated)
            {
                combatModeOn = true;
                SpriteEffect = enemy.Position.X < this.Position.X ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }
            else
            {
                if (combatModeOn)
                {
                    IsAttacking = false;
                    combatModeOn = false;
                }
                
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

        public void AddInstruction(Instruction instruction)
        {
            //TODO implement priority
            instruction.OnStart.Add(OnInstructionStart);
            instruction.OnComplete.Add(OnInstructionComplete);
            if (instruction.Type.ID == "Attack")
            {
                AttackTrigger = true;
                target = (Enemy)instruction.PassiveMember;
            }
            else
            {
                instructions.Enqueue(instruction, instruction.Priority);
            }

        }

        private void OnInstructionStart(Instruction instruction)
        {
            if (instructions.First == instruction)
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
            if (instructions.Count > 0)
            {
                if (instructions.First == instruction)
                {
                    instructions.Dequeue();
                    if (instruction.Type.ID != "takeItem")
                    {
                        CheckInventoryDump();
                    }

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

            CheckMove();
        }

        public void CheckMove()
        {
            if (instructions.Count == 0)
            {
                //If a colonis is standing on another colonist, he should move
                foreach (Colonist c in GlobalCombatDetector.colonists)
                {
                    if (c != this
                        && this.Position == c.Position)
                    {
                        if (c.Goals.Count == 0)
                        {
                            Vector2 tileSize = GameObjectManager.ZoneMap.TileSize;
                            Goals.Enqueue(this.Position + new Vector2(random.Next(-1, 2) * tileSize.X, random.Next(-1, 2) * tileSize.Y));
                        }
                    }
                }
            }
        }

        private void ColonistAttack(GameTime gameTime)
        {

            if (ColonistAttackSpeedControl(gameTime))
            {
                instructions.Clear();
                Goals.Clear();
                this.IsAttacking = true;
                PlayAttackingSound();
                enemy.Health -= this.AttackPower;
                if (enemy.Health<=0)
                {
                    this.InCombat = false;
                    this.IsAttacking = false;


                }
            }
            
        }

        private void PlayAttackingSound()
        {
            if (Ranged)
            {
                SoundFactory.PlaySoundEffect(Sound.Shotgun);
            }
            else
            {
                SoundFactory.PlaySoundEffect(Sound.slashSound);
            }
        }

        private bool ColonistAttackSpeedControl(GameTime gameTime)
        {
            ColonistimeToAttack += gameTime.ElapsedGameTime.TotalMilliseconds;

            // Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            if (ColonistimeToAttack > 500 & ColonistimeToAttack < 600)
            {
                IsAttacking = false;
                TextureGroupIndex = 2;
            }
            if (ColonistimeToAttack >= this.AttackSpeed)
            {
                ColonistimeToAttack = 0;
                return true;



            }
            return false;


        }

        private void Hunt(Enemy enemy) {
            Goals.Clear();
            if (enemy == null)
            {
                Goals.Enqueue(target.Position);
            }
            else {
                AttackTrigger = false;
                target = null;
            }
            

        }

        /// <summary>
        /// Clears instructions, Goals, and Path
        /// </summary>
        public void ClearInstructions()
        {
            instructions.Clear();
            Goals.Clear();
            Path.Clear();
        }

        public object Reconstruct()
        {
            return new Colonist(PFSPosition, (TextureSetTypes)textureSetID, inventory);
        }

    }
}



