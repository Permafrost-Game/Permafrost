using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Contracts;

namespace Engine.Drawing
{
    /// <summary>
    /// A <see cref="Sprite"/> is <see cref="GameObject"/> that is drawn with a texture and a depth.
    /// </summary>
    public class Sprite : GameObject, IDrawable
    {
        protected float depth;
        public Texture2D Texture { get; protected set; }
        protected SpriteEffects SpriteEffect { get; set; }

        #region Depth Methods
        /// <summary>
        /// Updates <see cref="Sprite.depth"/>
        /// </summary>
        /// <param name="bias">offset to stop Z fighting</param>
        protected void UpdateDepth(float bias = 0f) => depth = CalculateDepth(Position, bias);

        /// <summary>
        /// Calculate depth based on position
        /// </summary>
        /// <param name="position">the position of the sprite</param>
        /// <param name="bias">offset to stop Z fighting</param>
        /// <returns>the depth value</returns>
        protected static float CalculateDepth(Vector2 position, float bias = 0f) => (position.Y + bias + ((position.X + bias) / 2)) / 48000f;

        #endregion

        #region Origin Methods
        /// <summary>
        /// Sets <see cref="GameObject.Origin"/> based on the <see cref="GameObject"/>s <see cref="GameObject.Size"/>
        /// </summary>
        protected void UpdateOrigin() => Origin = CalculateOrigin(Size);

        /// <summary>
        /// Calculates the origin point
        /// </summary>
        /// <param name="size">the size of the <see cref="GameObject"/></param>
        /// <returns>the new origin point</returns>
        protected static Vector2 CalculateOrigin(Vector2 size) => size / 2f;
        #endregion

        #region Constructors

        public Sprite(Vector2 position, Texture2D texture, float rotation = 0f, string tag = "", SpriteEffects spriteEffect = SpriteEffects.None) :
        this(
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: rotation,
            origin: CalculateOrigin(new Vector2(texture.Width, texture.Height)),
            tag: tag,
            depth: CalculateDepth(position),
            texture: texture,
            spriteEffect: spriteEffect
            ) { }


        public Sprite(Vector2 position, Vector2 size, float rotation = 0f, Vector2 origin = default, string tag = "", float depth = 0f, Texture2D texture = default, SpriteEffects spriteEffect = SpriteEffects.None) :
            base(position, size, rotation, origin, tag)
        {
            this.depth = depth;
            this.Texture = texture;
            this.SpriteEffect = spriteEffect;
        }

        #endregion
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
