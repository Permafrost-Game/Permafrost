using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using GlobalWarmingGame.Interactions.Interactables;
using Engine.Drawing;
using Engine.PathFinding;

namespace GlobalWarmingGame.Interactions.Enemies
{
    //add animated sprite instead of aggressive movement
    //add random movement using randomAI class
    class Enemy : AnimatedSprite, IUpdatable,IInteractable,IPathFindable
    {
        List<GameObject> colonists;
        GameTime duration;
        GameObject target;
        Boolean targetFound = false;
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

        public Enemy(String tag, int aSpeed, int aRange, int aPower, int maxHp, Vector2 position, Texture2D[][] textureSet) : base
        (
            position: position,
            size: new Vector2(textureSet[0][0].Width, textureSet[0][0].Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: tag,
            depth: 0.9f,
            textureSet: textureSet,
            frameTime: 100f
            
           
        )
        {
            //generic stats:
            Speed = 0.2f;



            InstructionTypes.Add(new InstructionType("attack", "Attack " + tag, "Attack the " + tag, EnemyAttacked));

            if (this.Tag == "Robot") { xdifference = 19; ydifference = 16; } else if(this.Tag=="Bear"){ xdifference=16; ydifference = -15; }


            c = new Combat();
            this.attackRange = aRange;
            this.Health = maxHp;
            this.AttackPower = aPower;
            this.enemyTag = tag;
            this.attackSpeed = aSpeed;
       
        }

        private void EnemyAttacked(Colonist colonist)
        {
            EnemyAttacked();
        }

        public void SetEnemyDead(){
            GameObjectManager.Remove(this);
            
        }

        public void ResetEnemyTarget() {
            colonists.Remove(target);
            targetFound = false;
            target = null;
            Goals.Clear();
        }

        private void EnemyAttacked()
        {
            colonists = GameObjectManager.GetObjectsByTag("Colonist");
            targetFound = false;
            for (int i = 0; i < colonists.Count; i++)
            {
                if (InAggroRange(colonists.ElementAt(i).Position) & targetFound == false)
                {
                    targetFound = true;
                    target = colonists.ElementAt(i);
                }
            }
            if (targetFound == false)
            {
                Goals.Clear();
            }
            else
            {
                ChaseColonist();
            }
        }

        public void animateAttack() {


            if (this.Tag == "Robot")
            {
                isAnimated = true;
                this.TextureGroupIndex = 3;
            }
            else if (this.Tag == "Bear") {

                isAnimated = true;
                this.TextureGroupIndex = 3;
                
            }
                
                
            
        }
        public void setAttacking(Boolean b) {
            attacking = b;
        }
        public void setInCombat(Boolean b)
        {
            isInCombat = b;
        }
        private bool InAggroRange(Vector2 aggressor)
        {
            //Cost from this node to another node
            double toEndCost;

            //Positions of the tiles in vector form
            Vector2 enemy = this.Position;
            Vector2 attacker = aggressor;

            //Standard distance formula: distance = sqrt((X2-X1)^2 + (Y2-Y1)^2)
            toEndCost = Math.Sqrt((enemy.X - attacker.X) * (enemy.X - attacker.X) + (enemy.Y - attacker.Y) * (enemy.Y - attacker.Y));

            if (toEndCost < 200)
            {
                return true;
            }

            return false;
        }

        private void ChaseColonist(){
            if (InAggroRange(target.Position))
            {
               Goals.Clear();

               
                Vector2 fakePosition = new Vector2(target.Position.X+xdifference, target.Position.Y+ydifference);
                Goals.Enqueue(fakePosition);
            }
            else
            {
               Goals.Clear();
            }
        }

        //change random movement
        public  void OnGoalComplete(Vector2 completedGoal){
            Random rnd = new Random();
            Vector2 v = new Vector2
            {
                X = rnd.Next(500, 600),
                Y = rnd.Next(500, 600)
            };
            Goals.Enqueue(v);
        }

        
        public double getAttackSpeed() {
            return attackSpeed;
        }

        public override void Update(GameTime gameTime){
            Vector2 position1 = this.Position;
            this.Position += PathFindingHelper.CalculateNextMove(gameTime, this);
            base.Update(gameTime);

            Vector2 delta = position1 - this.Position;

            //Math.Atan2(delta.X, delta.Y);
            if (isInCombat)
            {
                if (flipped)
                {
                    SpriteEffect = SpriteEffects.FlipHorizontally;
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
            

           




            EnemyAttacked();
            if (target != null && targetFound == true)
            {
                 c.intializeCombat((Colonist)target, this,gameTime);
               
                 c.PerformCombat();
            
            }
            else
            {
                Goals.Clear();
                Goals.Enqueue(this.Position);
            }
        }

      
    }
}

