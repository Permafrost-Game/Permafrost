using Engine;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlobalWarmingGame.Interactions.Enemies
{
    class Combat
    {
        Colonist colonist;
        Enemy enemy;
        private GameTime gameTime;
        private double ColonistimeToAttack;
        private double EnemytimeToAttack;

        public bool Enabled => throw new NotImplementedException();

        public int UpdateOrder => throw new NotImplementedException();

        public float LastAttackTime { get; private set; }

        public Combat() {

        }

        public void intializeCombat(Colonist col, Enemy enemy, GameTime g) {
            colonist = col;
            this.enemy = enemy;
            gameTime = g;
            
        }

        private void EnemyAttack()
        {
            Console.WriteLine("[" + enemy.enemyTag + "] | Colonist HP: " + colonist.Health + " Enemy HP: " + enemy.Health + " Enemy A.P: " + enemy.AttackPower);
            var rnd = new Random(DateTime.Now.Millisecond);
            if (enemy.enemyTag == "Robot")
            {
                enemy.AttackPower = rnd.Next(250, 450); // Set random Attack Power for the `Robot` enemy, between 250 and 450, before actually running the attacking code
            }

           

            if (EnemyAttackSpeedControl())
            {
                enemy.setAttacking(true);
                colonist.Health = colonist.Health - enemy.AttackPower;

            }
            
            
            


            if (colonist.Health <= 0 || enemy.Health<=0) {
                colonist.setDead();
                enemy.ResetEnemyTarget();
                enemy.setAttacking(false);
               
                colonist.inCombat = false;
                colonist.isAttacking = false;
                enemy.setInCombat(false);
                enemy = null;
                colonist = null;
            }
           
        }
        private Boolean ColonistAttackSpeedControl() {
            ColonistimeToAttack = ColonistimeToAttack + gameTime.ElapsedGameTime.TotalMilliseconds;

            Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            if (ColonistimeToAttack > 500 & ColonistimeToAttack < 600)
            {
                colonist.isAttacking = false;
            }
            if (ColonistimeToAttack >= colonist.attackSpeed)
            {
                ColonistimeToAttack = 0;
                return true;



            }
            return false;

        }
       
        private Boolean EnemyAttackSpeedControl()
        {
           
            EnemytimeToAttack =EnemytimeToAttack + gameTime.ElapsedGameTime.TotalMilliseconds;

            Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            if (EnemytimeToAttack > 500 & EnemytimeToAttack < 600) {
                enemy.setAttacking(false);
            }
           
            if (EnemytimeToAttack >= enemy.getAttackSpeed())
            {
                EnemytimeToAttack = 0 ;
                return true;



            }
            return false;

        }

        private void ColonistAttack() {
            if (ColonistAttackSpeedControl())
            {
                colonist.isAttacking = true;
                enemy.Health = enemy.Health - colonist.AttackPower;
            }
            Console.WriteLine("Colonist hp: " + colonist.Health + " Enemy hp: " + enemy.Health);
            


            if (enemy.Health <= 0)
            {
                enemy.SetEnemyDead();
                colonist.inCombat = false;
                colonist.isAttacking = false;
                enemy = null;
                colonist = null;
            }
        }

        private double DistanceBetweenCombatants() {
            return Math.Sqrt((enemy.Position.X - colonist.Position.X) * (enemy.Position.X - colonist.Position.X) + (enemy.Position.Y - colonist.Position.Y) * (enemy.Position.Y - colonist.Position.Y));
        }
        
        public void PerformCombat() {
            enemy.setInCombat(false);
           
            if (colonist != null && enemy != null)
            {


                if (colonist.attackRange >= DistanceBetweenCombatants() & enemy.Health > 0 & colonist.Health > 0)
                {
                    colonist.inCombat = true;
                    enemy.setInCombat(true);
                    ColonistAttack();
                }
                else {
                    colonist.inCombat = false;
                }
                if (colonist != null && enemy != null)
                {
                   
                    if (enemy.attackRange >= DistanceBetweenCombatants() & colonist.Health > 0 & enemy.Health > 0)
                    {
                        colonist.inCombat = true;
                        enemy.setInCombat(true);
                        EnemyAttack();
                    }
                    else
                    {
                        colonist.inCombat = false;
                    }
                }
                

            }
           
        }

        
    }  
}
