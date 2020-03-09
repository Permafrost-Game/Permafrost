using GlobalWarmingGame.Interactions.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Enemies
{
    public class Robot : Enemy
    {
        

        public Robot(Vector2 position, Texture2D[][] textureSet) : base("Robot",5000, 70, 0, 500, position, textureSet)
        {
        
        }

        public override void animateAttack()
        {

            if (targetInRange != null)
            {


                isAnimated = true;
                this.TextureGroupIndex = 3;
                
                
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (this.Health <= 0)
            {
                GameObjectManager.Remove(this);
            }
        }

        protected override void ChaseColonist(Colonist colonist)
        {
            Vector2 fakeLeftXcoordinate = new Vector2(colonist.Position.X - 40, colonist.Position.Y);
           Vector2 fakeRightXcoordinate = new Vector2(colonist.Position.X + 40, colonist.Position.Y);
            if (this.Position.X < colonist.Position.X)
            {
                Goals.Enqueue(fakeLeftXcoordinate);
            }
            else
            {
                Goals.Enqueue(fakeRightXcoordinate);
            }
        }

        internal override void attackingSound()
        {
            SoundFactory.PlaySoundEffect(Sound.robotShock);
        }

        public override void EnemyAttack(GameTime gameTime) {
            Random dmg = new Random();
            AttackPower = dmg.Next(20, 50);
            base.EnemyAttack(gameTime);
        }
    }
}
