using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class AnimatedSprite : GameObject, IDrawable, IUpdatable
    {
        

        protected float timeToNextFrame;
        private float timeUntilNextFrame;

        private int currentTextureIndex;
        private int currentTextureGroupIndex;

        protected Texture2D[][] textureSet;
        protected float depth;

        protected bool isAnimated;

        protected int TextureGroupIndex
        {
            get { return currentTextureGroupIndex; }
            set
            {
                currentTextureGroupIndex = value;
                currentTextureIndex = 0;
                //timeUntilNextFrame = timeToNextFrame;
            }
        }
        protected SpriteEffects SpriteEffect { get; set; }

        public AnimatedSprite(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag, float depth, Texture2D[][] textureSet, SpriteEffects spriteEffect = SpriteEffects.None) :
            base(position, size, rotation, rotationOrigin, tag)
        {
            isAnimated = true;
            this.depth = depth;
            this.SpriteEffect = spriteEffect;
            this.textureSet = textureSet;
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture: textureSet[currentTextureGroupIndex][currentTextureIndex],
                position: Position,
                sourceRectangle: null,
                color: Color.White,
                rotation: Rotation,
                origin: RotationOrigin,
                scale: 1f,
                effects: SpriteEffects.None,
                layerDepth: depth);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (isAnimated)
            {
                timeUntilNextFrame -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeUntilNextFrame <= 0)
                {
                    currentTextureIndex = (currentTextureIndex + 1) % textureSet[currentTextureGroupIndex].Length;
                    timeUntilNextFrame = timeToNextFrame;
                }
            }
        }
    }
}
