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
        

        public Robot(Vector2 position, Texture2D[][] textureSet) : base("Robot", 10000, 70, 0, 500, position, textureSet)
        {
            






        }

        public override void animateAttack()
        {

            if (targetInRange != null)
            {


                isAnimated = true;
                this.TextureGroupIndex = 3;
                Console.WriteLine("bear " + this.Position.X);
                Console.WriteLine("colonist " + targetInRange.Position.X);
                Console.WriteLine("After: " + targetToTheLeftAfter + "  Before: " + targetToTheLeftBefore);
                
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
    }
}
