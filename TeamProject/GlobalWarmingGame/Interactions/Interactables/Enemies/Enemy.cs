using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Enemies;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.Interactions.Enemies
{
    
    public abstract class Enemy : AnimatedSprite, IUpdatable,IInteractable,IPathFindable
    {
        
        private Colonist target=null; //target is anything within aggro range
        private Colonist targetInRange=null; //targetInRange is anything in attacking range

        //declaring stats variables
        public float AttackPower { get; set; }
        public float Health { get; set; }
        public float AttackRange { get; set; }
        private double AttackSpeed { get; set; }
        public float Speed { get; set; }
        public readonly double aggroRange = 200; // could be moved down(for now all enemies aggro at the same distance)

        //initializing variables for instructions(right clicking an enemy allows you to attack it)
        public List<InstructionType> InstructionTypes { get;} = new List<InstructionType>();
        public Queue<Vector2> Goals { get; set; } = new Queue<Vector2>();
        public Queue<Vector2> Path { get; set; }  = new Queue<Vector2>();
        

        //timing variables
        public bool attacking=false;//determines if the enemy is attacking at the moment
        public bool isInCombat=false;//shows if the enemy is fighting a colonist
        private double timeToAttack; //a flag based on attack speed that tells the enemy to attack

            // variable for random movement of enemies
        private readonly RandomAI ai = new RandomAI(70, 0); //variables passed here could be pushed down to make different patterns for different enemies


        public Enemy(string name, int aSpeed, int aRange, int aPower, int maxHp, Vector2 position, Texture2D[][] textureSet) : base
        (
            //constructior setting game object details
            position: position,
            textureSet: textureSet,
            frameTime: 100f
        )
        {

            //InstructionTypes.Add(new InstructionType("Shoot", $"Shoot {name}", onStart: Shoot));

            //generic stats:
            this.AttackRange = aRange;
            this.Health = maxHp;
            this.AttackPower = aPower;
            this.AttackSpeed = aSpeed;
            Speed = 0.2f;
        }

       

        private void Shoot(Instruction instruction)
        {
            
        }


        public abstract void SetEnemyDead();



        private void Aggro() //this method makes the enemy attack colonists and roam if there isnt any
        {
            //using globalcombatdetector to determine nearby colonists
            target = GlobalCombatDetector.ColonistInAggroRange(this);
            targetInRange = GlobalCombatDetector.FindEnemyThreat(this);
            
            if (target == null)
            {
                isAnimated = true;
                TextureGroupIndex = 1;
                Speed = 0.05f;//decreasing the default speed when roaming (more natural)
                Goals.Enqueue(this.Position + ai.RandomTranslation()); //make it go randomly around
            }
            else
            {
                Speed = 0.2f;//return to normal speed (seems like speeding up when moving from roaming to chasing)
                ChaseColonist(target); //chase the found colonist
            }
        }

        public abstract void AnimateAttack(); //absract method for animating attacks allows more customisation
       
        //getters and setters
        public double GetAttackSpeed() => AttackSpeed;
        public void SetAttacking(bool b) {
            attacking = b;
            if( b == false){
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
                targetInRange.Health = target.Health - this.AttackPower;
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
            if (timeToAttack >= this.GetAttackSpeed())
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
            Vector2 position1 = this.Position; //getting the position before updating
            this.Position += PathFindingHelper.CalculateNextMove(gameTime, this); //calculating next move
            depth = (Position.X + (Position.Y / 2)) / 48000f; //depth
            
            base.Update(gameTime); //update the game
                if (this.Health <= 0) {
                    this.SetEnemyDead();
                    return;
                }
            Aggro(); // enemy is agressive all the time

            Vector2 delta = position1 - this.Position; //getting in which direction the enemy is moving
            Math.Atan2(delta.X, delta.Y); // jedd's maths magic

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
            else if(target!=null && targetInRange==null)
            {
                SetInCombat(false);
                TextureGroupIndex = 1;
                Goals.Clear();
                ChaseColonist(target); //if there isnt anyone in attacking range then check if anyone is around and chase him
            }
        }

        private void PerformCombat(GameTime gameTime,Colonist targetInRange)
        {
            this.SetInCombat(false);

            if (targetInRange != null) //double checking if the target didnt disappear 
            {

                if ( targetInRange.Health > 0 && this.Health > 0)
                {   //set flags for animation
                    targetInRange.InCombat = true; 
                    this.SetInCombat(true);
                    EnemyAttack(gameTime); //actually try attacking
                }
                else
                {
                    this.SetInCombat(false); //suddenly there is no enemy anymore then set out of combat
                }
            }

        }

        internal abstract void AttackingSound(); //woah sounds so cool
        internal abstract void DeathSound();
        internal abstract List<ResourceItem> Loot();


    }
}

