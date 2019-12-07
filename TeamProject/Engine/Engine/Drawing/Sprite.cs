using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    /// <summary>
    /// A <see cref="Sprite"/> is <see cref="GameObject"/> that is drawn with a texture and a depth.
    /// </summary>
    public class Sprite : GameObject, IDrawable
    {
        protected float depth;
        protected Texture2D texture;

        public Sprite(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag, float depth, Texture2D texture) :
            base(position, size, rotation, rotationOrigin, tag)
        {
            this.depth = depth;
            this.texture = texture;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture:    texture, 
                position:   Position,
                sourceRectangle: null,
                color:      Color.White,
                rotation:   Rotation,
                origin:     RotationOrigin,
                scale:      1f,
                effects:    SpriteEffects.None,
                layerDepth: depth);
        }
    }
}
