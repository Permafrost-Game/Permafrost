using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.Interactions.Enemies
{
    
   public abstract class Enemy : AnimatedSprite, IUpdatable,IInteractable,IPathFindable
    {
        
        public Colonist target=null;
        public Colonist targetInRange=null;
        Vector2 fakeLeftXcoordinate;
        Vector2 fakeRightXcoordinate;
        public float AttackPower { get; set; }
        public float Health { get; set; }
        public float attackRange { get; set; }
        public string enemyTag { get; set; }
        private double attackSpeed { get; set; }
      

        public List<InstructionType> InstructionTypes { get;} = new List<InstructionType>();
        public Queue<Vector2> Goals { get; set; } = new Queue<Vector2>();
        public Queue<Vector2> Path { get; set; }  = new Queue<Vector2>();
        public float Speed { get; set; }

        
        public Boolean attacking=false;
    
        public bool isInCombat=false;
 
        private double EnemytimeToAttack;
        public double aggroRange=200;
        public bool targetToTheLeftBefore;
        public bool targetToTheLeftAfter;
        public bool flip;
        RandomAI ai = new RandomAI(70, 0);


        public Enemy(String tag, int aSpeed, int aRange, int aPower, int maxHp, Vector2 position, Texture2D[][] textureSet) : base
        (
            position: position,
            size: new Vector2(textureSet[0][0].Width, textureSet[0][0].Height),
            rotation: 0f,
            origin: new Vector2(textureSet[0][0].Width / 2f, textureSet[0][0].Height / 2f),
            tag: tag,
            depth: 0f,
            textureSet: textureSet,
            frameTime: 100f
            
           
        )
        {
            //generic stats:
            Speed = 0.2f;



            InstructionTypes.Add(new InstructionType("attack", "Attack " + tag, "Attack the " + tag, onComplete: EnemyAttacked));

            

         
          
            this.attackRange = aRange;
            this.Health = maxHp;
            this.AttackPower = aPower;
            this.enemyTag = tag;
            this.attackSpeed = aSpeed;
       
        }

        private void EnemyAttacked(Instruction instruction)
        {
            Aggro();
        }

        public void SetEnemyDead(){
            SoundFactory.PlaySong(Songs.Main);
            GlobalCombatDetector.mainIsPlaying = true;
            GlobalCombatDetector.combatSoundPlaying = false;
            GameObjectManager.Remove(this);
            
        }



        private void Aggro()
        {


            target = GlobalCombatDetector.ColonistInAggroRange(this);
            targetInRange = GlobalCombatDetector.FindEnemyThreat(this);
            
            if (target == null)
            {
                
                Speed = 0.05f;
                
                Goals.Enqueue(this.Position + ai.RandomTranslation());

            }
            else
            {
                
                Speed = 0.2f;
                ChaseColonist(target);
            }
        }

        public abstract void animateAttack();

        public void setAttacking(Boolean b) {
            attacking = b;
        }
        public void setInCombat(Boolean b)
        {
            isInCombat = b;
        }


        protected abstract void ChaseColonist(Colonist colonist);
        

        //change random movement
        public  void OnGoalComplete(Vector2 completedGoal){
           //its ok
        }

        
        public double getAttackSpeed() {
            return attackSpeed;
        }

        public override void Update(GameTime gameTime){
            Vector2 position1 = this.Position;
            this.Position += PathFindingHelper.CalculateNextMove(gameTime, this);
            depth = (Position.X + (Position.Y / 2)) / 48000f;
            Aggro();
            
            base.Update(gameTime);
            
            Vector2 delta = position1 - this.Position;

            Math.Atan2(delta.X, delta.Y);
            if (isInCombat)
            {

                if (targetInRange != null)
                {

                    SpriteEffect = targetInRange.Position.X < this.Position.X ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                    targetToTheLeftBefore = targetToTheLeftAfter;
                }

                if (attacking)
                {
                    animateAttack();
                }
                else
                {
                
                    isAnimated = true;
                    TextureGroupIndex = 1;
                   
                }
            }
            else
            {
                
                 if (delta.Equals(Vector2.Zero))
                 {
                     isAnimated = false;
                 }
                 else if (Math.Abs(delta.X) >= Math.Abs(delta.Y))
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
               
               
                PerformCombat(gameTime,targetInRange);
            
            }
            else if(target!=null)
            {
                fakeLeftXcoordinate = new Vector2(target.Position.X -40,target.Position.Y);
                fakeRightXcoordinate = new Vector2(target.Position.X + 40, target.Position.Y);
                Goals.Clear();
                if (this.Position.X < target.Position.X) { 
                Goals.Enqueue(fakeLeftXcoordinate);
                }
                else
                {
                    Goals.Enqueue(fakeRightXcoordinate);
                }
            }
        }

        private void PerformCombat(GameTime gameTime,Colonist targetInRange)
        {
            this.setInCombat(false);

                
                if (targetInRange != null)
                {

                    if ( targetInRange.Health > 0 & this.Health > 0)
                    {
                        targetInRange.inCombat = true;
                        this.setInCombat(true);
                        EnemyAttack(gameTime);
                    }
                    else
                    {
                       
                        this.setInCombat(false);
                     }
                }

        }

        public virtual void EnemyAttack(GameTime gameTime)
        {
            if (EnemyAttackSpeedControl(gameTime))
            {
                this.setAttacking(true);
                this.attackingSound();
                targetInRange.Health = target.Health - this.AttackPower;

            }
            

            if (targetInRange.Health <= 0 || this.Health <= 0)
            {
              
                
                this.setAttacking(false);

               // SoundFactory.PlaySong(Songs.Main);
                GlobalCombatDetector.mainIsPlaying = true;
                GlobalCombatDetector.combatSoundPlaying = false;

                this.setInCombat(false);
                targetInRange = null;
                
                    
                
            }
        }

        internal abstract void attackingSound();

        private bool EnemyAttackSpeedControl(GameTime gameTime)
        {
            EnemytimeToAttack = EnemytimeToAttack + gameTime.ElapsedGameTime.TotalMilliseconds;

            if (EnemytimeToAttack > 500)
            {
                this.setAttacking(false);
            }

            if (EnemytimeToAttack >= this.getAttackSpeed())
            {
                EnemytimeToAttack = 0;
                return true;

            }
            return false;

        }
    }
}

