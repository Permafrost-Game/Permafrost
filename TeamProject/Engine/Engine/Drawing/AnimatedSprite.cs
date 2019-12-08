using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    /// <summary>
    /// An <see cref="Sprite"/> with a <see cref="AnimatedSprite.textureSet"/> of animation frames<br/>
    /// <br/>
    /// When <see cref="AnimatedSprite.isAnimated"/> is true, the <c>Texture2D[]</c> of index <see cref="AnimatedSprite.currentTextureGroupIndex"/> in <see cref="AnimatedSprite.textureSet"/><br/>
    /// which contains animation frames, will be drawn every frame. After <see cref="AnimatedSprite.frameTime"/>, the next texture in the array will be used, looping back round
    /// </summary>
    public class AnimatedSprite : Sprite, IUpdatable
    {
        
        /// <summary>The time in ms between animation frames</summary>
        protected float frameTime;
        private float timeUntilNextFrame;

        private int currentTextureIndex;
        private int currentTextureGroupIndex;

        /// <summary>A nested array, of texture groups, each sub array contains animation frames</summary>
        protected Texture2D[][] textureSet;

        /// <summary>Enables / dissables the playing of animation frames</summary>
        protected bool isAnimated;

        /// <summary>
        /// The first dimension of <see cref="AnimatedSprite.textureSet"/> that is active
        /// </summary>
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



        /// <summary>
        /// Creates a new <see cref="AnimatedSprite"/> with a <paramref name="textureSet"/> 
        /// </summary>
        /// <param name="textureSet">A 2D array of textures, where the first dimension is for texture groups, and the seccond dimension is for animation frames</param>
        /// <param name="frameTime">The time in ms between frames</param>
        public AnimatedSprite(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag, float depth, Texture2D[][] textureSet, float frameTime, SpriteEffects spriteEffect = SpriteEffects.None) :
            base(position, size, rotation, rotationOrigin, tag, depth, textureSet[0][0], spriteEffect )
        {
            this.isAnimated = true;
            this.depth = depth;
            this.SpriteEffect = spriteEffect;
            this.textureSet = textureSet;
            this.frameTime = frameTime;
            this.timeUntilNextFrame  = frameTime;

        }

        public virtual void Update(GameTime gameTime)
        {
            if (isAnimated)
            {
                timeUntilNextFrame -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeUntilNextFrame <= 0)
                {
                    currentTextureIndex = (currentTextureIndex + 1) % textureSet[currentTextureGroupIndex].Length;
                    Texture = textureSet[currentTextureGroupIndex][currentTextureIndex];

                    timeUntilNextFrame = frameTime;
                    
                }
            }
        }
    }
}
