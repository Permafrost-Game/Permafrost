using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    class Sprite : GameObject, IDrawable
    {
        protected float depth;
        protected Texture2D texture;

        public Sprite(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, float depth, Texture2D texture) :
            base(position, size, rotation, rotationOrigin)
        {
            this.depth = depth;
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, color: Color.White, rotation, rotationOrigin, 1f, SpriteEffects.None, depth);
        }
    }
}
