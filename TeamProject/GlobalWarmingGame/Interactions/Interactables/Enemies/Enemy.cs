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
    //add animated sprite instead of aggressive movement
    //add random movement using randomAI class
   public abstract class Enemy : AnimatedSprite, IUpdatable,IInteractable,IPathFindable
    {
        
        Colonist target=null;
        Colonist targetInRange=null;
       
        public float AttackPower { get; set; }
        public float Health { get; set; }
        public float attackRange { get; set; }
        public string enemyTag { get; set; }
        private double attackSpeed { get; set; }
        private float xdifference;
        private float ydifference;

        public List<InstructionType> InstructionTypes { get;} = new List<InstructionType>();
        public Queue<Vector2> Goals { get; set; } = new Queue<Vector2>();
        public Queue<Vector2> Path { get; set; }  = new Queue<Vector2>();
        public float Speed { get; set; }

        Combat c;
        public Boolean attacking=false;
    
        private bool isInCombat=false;
        private bool flipped;
        private double EnemytimeToAttack;
        public double aggroRange=200;

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



            InstructionTypes.Add(new InstructionType("attack", "Attack " + tag, "Attack the " + tag, onStart: EnemyAttacked));

            if (this.Tag == "Robot") { xdifference = 19; ydifference = 16; } else if(this.Tag=="Bear"){ xdifference=16; ydifference = -15; }


            c = new Combat();
            this.attackRange = aRange;
            this.Health = maxHp;
            this.AttackPower = aPower;
            this.enemyTag = tag;
            this.attackSpeed = aSpeed;
       
        }

        private void EnemyAttacked(IInstructionFollower follower)
        {
            Aggro();
        }

        public void SetEnemyDead(){
            GameObjectManager.Remove(this);
            
        }



        private void Aggro()
        {






            target = GlobalCombatDetector.ColonistInAggroRange(this);
            targetInRange = GlobalCombatDetector.FindEnemyThreat(this);
            
            if (target == null)
            {
             //needs random movement
               
            }
            else
            {
                
                ChaseColonist(target);
            }
        }

        public void animateAttack() {
            if (targetInRange != null)
            {
                bool flippedOnce = false;

                if (this.Tag == "Robot")
                {
                    isAnimated = true;
                    this.TextureGroupIndex = 3;
                    if (this.Position.X < targetInRange.Position.X)
                    {
                        SpriteEffect = SpriteEffects.FlipHorizontally;
                        flippedOnce = true;
                    }
                    else
                    {
                        if (flippedOnce == true)
                        {
                            SpriteEffect = SpriteEffects.FlipHorizontally;
                        }
                        flippedOnce = false;
                    }
                }
                else if (this.Tag == "Bear")
                {

                    isAnimated = true;
                    this.TextureGroupIndex = 3;
                    if (this.Position.X < target.Position.X)
                    {
                        SpriteEffect = SpriteEffects.FlipHorizontally;
                        flippedOnce = true;
                    }
                    else
                    {
                        if (flippedOnce == true)
                        {
                            SpriteEffect = SpriteEffects.FlipHorizontally;
                        }
                        flippedOnce = false;
                    }

                }


            }  
        }
        public void setAttacking(Boolean b) {
            attacking = b;
        }
        public void setInCombat(Boolean b)
        {
            isInCombat = b;
        }
        

        private void ChaseColonist(Colonist colonist)
        {
            


            
                    Goals.Enqueue(colonist.Position);

            Console.WriteLine("bear " + this.Position);




        }

        //change random movement
        public  void OnGoalComplete(Vector2 completedGoal){
           
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

            //Math.Atan2(delta.X, delta.Y);
            if (isInCombat)
            {
                if (flipped)
                {
                    //SpriteEffect = SpriteEffects.FlipHorizontally;
                }
                if (attacking)
                {
                   
                    animateAttack();
                    

                }
                else if (!attacking)
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
                    flipped = true;
                }
                else
                {
                    isAnimated = true;
                    TextureGroupIndex = (delta.Y > 0) ? 2 : 0;

                }
            }
            

           




           
            if (targetInRange != null)
            {
               //  c.intializeCombat((Colonist)target, this,gameTime);
               
                PerformCombat(gameTime,targetInRange);
            
            }
            else if(target!=null)
            {
                Goals.Clear();
                Goals.Enqueue(target.Position);
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

        private void EnemyAttack(GameTime gameTime)
        {
            if (EnemyAttackSpeedControl(gameTime))
            {
                this.setAttacking(true);
                targetInRange.Health = target.Health - this.AttackPower;

            }





            if (targetInRange.Health <= 0 || this.Health <= 0)
            {
              
                
                this.setAttacking(false);

                
                this.setInCombat(false);
                targetInRange = null;
                
            }
        }

        private bool EnemyAttackSpeedControl(GameTime gameTime)
        {
            EnemytimeToAttack = EnemytimeToAttack + gameTime.ElapsedGameTime.TotalMilliseconds;

            Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            if (EnemytimeToAttack > 500 & EnemytimeToAttack < 600)
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

