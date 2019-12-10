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

namespace GlobalWarmingGame.Interactions.Enemies
{
    //add animated sprite instead of aggressive movement
    //add random movement using randomAI class
    class Enemy : AggressiveMovement, IUpdatable
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
        Combat c;

        public Enemy(String tag, int aSpeed, int aRange, int aPower, int maxHp, Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: tag,
            depth: 0.9f,
            texture: texture,
            instructionTypes: new List<InstructionType>(),
            speed: 5f
        )
        {
            InstructionTypes.Add(new InstructionType("attack", "Attack " + tag, "Attack the " + tag, EnemyAttacked));

            colonists = GameObjectManager.GetObjectsByTag("Colonist");
            
            c = new Combat();
            this.attackRange = aRange;
            this.Health = maxHp;
            this.AttackPower = aPower;
            this.enemyTag = tag;
            this.attackSpeed = aSpeed;
       
        }

        public void SetEnemyDead(){
            GameObjectManager.Remove(this);
            
        }

        public void ResetEnemyTarget() {
            colonists.Remove(target);
            targetFound = false;
            target = null;
            base.goals.Clear();
        }

        public void EnemyAttacked(){
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
                base.goals.Clear();
            }
            else
            {
                ChaseColonist();
            }
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

            if (toEndCost < 100)
            {
                return true;
            }

            return false;
        }

        private void ChaseColonist(){
            if (InAggroRange(target.Position))
            {
                base.goals.Clear();
                base.AddGoal(target.Position);
            }
            else
            {
                base.goals.Clear();
            }
        }

        //change random movement
        protected override void OnGoalComplete(Vector2 completedGoal){
            Random rnd = new Random();
            Vector2 v = new Vector2
            {
                X = rnd.Next(500, 600),
                Y = rnd.Next(500, 600)
            };
            base.AddGoal(v);
        }

        
        public double getAttackSpeed() {
            return attackSpeed;
        }

        public override void Update(GameTime gameTime){
            base.Update(gameTime);
            EnemyAttacked();
            if (target != null && targetFound == true)
            {
                 c.intializeCombat((Colonist)target, this,gameTime);
               // this.attackSpeed = this.attackSpeed - gameTime.ElapsedGameTime.TotalMilliseconds;

              //  if (this.attackSpeed <= 0)
              //  {
                    c.PerformCombat();
                   
                    
                    
               // }
            }
            else
            {
                base.goals.Clear();
                base.AddGoal(this.Position);
            }
        }
    }
}

