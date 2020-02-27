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
   public class Bear : Enemy
    {


        public Bear ( Vector2 position, Texture2D[][] textureSet): base ("Bear",1000, 60, 10, 300, position,textureSet)
        {
        
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
