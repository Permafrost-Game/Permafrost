using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Enemies
{

    public abstract class Enemy : AnimatedSprite, IHealthbased, IUpdatable, IInteractable,IPathFindable
    {
        public Colonist Target { get; set; } = null; //target is anything within aggro range
        private Colonist targetInRange=null; //targetInRange is anything in attacking range

        //declaring stats variables
        public float AttackPower { get; set; }
        public float MaxHealth { get; }
        public float Health { get; set; }
        public float AttackRange { get; set; }
        private double AttackSpeed { get; set; }

        public float Speed { get; set; }
        public double AggroRange { get; set; } = 200; // could be moved down(for now all enemies aggro at the same distance)

        //initializing variables for instructions(right clicking an enemy allows you to attack it)
        public List<InstructionType> InstructionTypes { get;} = new List<InstructionType>();
        public Queue<Vector2> Goals { get; set; } = new Queue<Vector2>();
        public Queue<Vector2> Path { get; set; }  = new Queue<Vector2>();


        //timing variables
        public bool attacking=false;//determines if the enemy is attacking at the moment
        public bool isInCombat=false;//shows if the enemy is fighting a colonist
        private double timeToAttack; //a flag based on attack speed that tells the enemy to attack
        internal bool notDefeated=true;

        // variable for random movement of enemies
        public RandomAI AI { get; set; } = new RandomAI(70, 0); //variables passed here could be pushed down to make different patterns for different enemies


        public Enemy(string name, int aSpeed, int aRange, int aPower, int maxHp, Vector2 position, TextureSetTypes textureSetType) : base
        (
            //constructior setting game object details
            position: position,
            textureSet: Textures.MapSet[textureSetType],
            frameTime: 100f
        )
        {

            InstructionTypes.Add(new InstructionType("Attack", $"Attack {name}"));

            //generic stats:
            this.AttackRange = aRange;
            this.MaxHealth = maxHp;
            this.Health = maxHp;
            this.AttackPower = aPower;
            this.AttackSpeed = aSpeed;
            Speed = 0.1f;
        }

        protected abstract void SetDead();



        private void Aggro() //this method makes the enemy attack colonists and roam if there isnt any
        {
            //using globalcombatdetector to determine nearby colonists
            Colonist potentialTarget = GlobalCombatDetector.GetClosestColonist(this.Position);

            if (potentialTarget != null
                && AggroRange > Vector2.Distance(this.Position, potentialTarget.Position))
            {
                Target = potentialTarget;

                if (this.AttackRange > Vector2.Distance(this.Position, Target.Position))
                {
                    targetInRange = Target;
                }
                else
                {
                    targetInRange = null;
                }

                Speed = 0.1f; //return to normal speed (seems like speeding up when moving from roaming to chasing)
                ChaseColonist(Target); //chase the found colonist

            }
            else
            {
                Target = null;
                isAnimated = true;
                TextureGroupIndex = 1;
                Speed = 0.05f; //decreasing the default speed when roaming (more natural)
                if (AI != null) 
                {
                    Goals.Enqueue(this.Position + AI.RandomTranslation()); //make it go randomly around
                }
            }
        }

        public abstract void AnimateAttack(); //absract method for animating attacks allows more customisation
        public void SetAttacking(bool b)
        {
            attacking = b;
            if (b == false)
            {
                TextureGroupIndex = 1; //sets textures to normal when attacking is turned off
            }
        }
        public void SetInCombat(bool b)
        {
            isInCombat = b;
        }

        protected abstract void ChaseColonist(Colonist colonist);//abstract to give more customisation

        public virtual void EnemyAttack(GameTime gameTime) //virtual so it can be edited in subclasses
        {
            if (EnemyAttackSpeedControl(gameTime)) //checks if its allowed to attack yet
            {
                this.SetAttacking(true); //flags for animation
                this.AttackingSound(); //uses subclass implementation for attacking sound based on what enemy it is
                targetInRange.Health = Target.Health - this.AttackPower;
            }
            //quick check if target died after the last hit
            if (targetInRange.Health <= 0 || this.Health <= 0)
            {
                this.SetAttacking(false);
                this.SetInCombat(false);

            }
        }
        private bool EnemyAttackSpeedControl(GameTime gameTime)
        {
            timeToAttack += gameTime.ElapsedGameTime.TotalMilliseconds; //counting how much time has passed since last attack

            if (timeToAttack > 500) //if its been half a second already cancel the attacking
            {
                this.SetAttacking(false);
            }
            if (timeToAttack >= AttackSpeed)
            {
                timeToAttack = 0; //reset the counter
                return true; //attack speed is less or equal to the time passed since last attack, allow to hit again

            }
            return false;

        }

        public  void OnGoalComplete(Vector2 completedGoal){
            //its ok
        }

        public override void Update(GameTime gameTime){
            Vector2 lastPosition = this.Position; //getting the position before updating
            this.Position += PathFindingHelper.CalculateNextMove(gameTime, this); //calculating next move
            UpdateDepth(0.25f);
            base.Update(gameTime); //update the game

            if (this.Health <= 0) {
                this.SetDead();
                return;
            }

            Aggro(); // enemy is agressive all the time

            Vector2 delta = lastPosition - this.Position; //getting in which direction the enemy is moving

            if (isInCombat)
            {
                if (targetInRange != null)
                {
                    SpriteEffect = targetInRange.Position.X < this.Position.X ? SpriteEffects.FlipHorizontally : SpriteEffects.None; //face colonist
                }
                if (attacking)//flags if its time to attack
                {
                    AnimateAttack();// animates attack
                }
                else
                {
                    isAnimated = true;
                    TextureGroupIndex = 1; //setting bear to normal texture if its not attacking

                }
            }
            else //if not in combat
            {
                    if (delta.Equals(Vector2.Zero))
                    {
                        isAnimated = false; //if the bear isnt going anywhere
                    }
                    else if (Math.Abs(delta.X) >= Math.Abs(delta.Y)) //some magic with jedd's code (it animates movement)
                    {
                        isAnimated = true;
                        TextureGroupIndex = 1;
                        SpriteEffect = (delta.X > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    }
                    else
                    {
                        isAnimated = true;
                        TextureGroupIndex = (delta.Y > 0) ? 2 : 0;
                    }
            }

            if (targetInRange != null)
            {
                PerformCombat(gameTime,targetInRange); // fight if theres anyone to fight
            }
            else if(Target!=null)
            {
                SetInCombat(false);
                TextureGroupIndex = 1;
                Goals.Clear();
                ChaseColonist(Target); //if there isnt anyone in attacking range then check if anyone is around and chase him
            }
        }

        private void PerformCombat(GameTime gameTime,Colonist targetInRange)
        {
            this.SetInCombat(false);
            if ( targetInRange.Health > 0 && this.Health > 0)
            {   //set flags for animation
                targetInRange.InCombat = true; 
                this.SetInCombat(true);
                EnemyAttack(gameTime); //actually try attacking
            }

        }

        internal abstract void AttackingSound();
        internal abstract void DeathSound();
    }
}
