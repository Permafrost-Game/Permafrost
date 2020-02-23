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
        public Texture2D Texture { get; protected set; }


        /// <summary>
        /// The currently active sprite effect
        /// </summary>
        protected SpriteEffects SpriteEffect { get; set; }


        public Sprite(Vector2 position, Vector2 size, float rotation, Vector2 origin, string tag, float depth, Texture2D texture = default, SpriteEffects spriteEffect = SpriteEffects.None) :
            base(position, size, rotation, origin, tag)
        {
            this.depth = depth;
            this.Texture = texture;
            this.SpriteEffect = spriteEffect;
        }

        public Sprite(Vector2 position, Vector2 size, float rotation = 0f, Vector2 origin = default, string tag = "", Texture2D texture = default, SpriteEffects spriteEffect = SpriteEffects.None) :
            base(position, size, rotation, origin, tag)
        {
            this.depth = (position.Y + (position.X / 2)) / 48000f;
            this.Texture = texture;
            this.SpriteEffect = spriteEffect;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture:    Texture, 
                position:   Position,
                sourceRectangle: null,
                color:      Color.White,
                rotation:   Rotation,
                origin:     Origin,
                scale:      1f,
                effects: SpriteEffect,
                layerDepth: depth);
        }
    }
}
