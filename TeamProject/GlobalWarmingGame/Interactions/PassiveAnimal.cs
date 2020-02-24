
using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    public abstract class PassiveAnimal: AnimatedSprite, IInteractable, IPathFindable
    {
        public Queue<Vector2> Goals { get; set; }
        public Queue<Vector2> Path { get; set; }
        public float Speed { get; set; }

        public List<InstructionType> InstructionTypes { get; }

        protected RandomAI ai;

        public PassiveAnimal(Vector2 position, string tag, Texture2D[][] textureSet, float speed, RandomAI ai, float frameTime = 100f) : base
        (
            position: position,
            size: new Vector2(textureSet[0][0].Width, textureSet[0][0].Height),
            rotation: 0f,
            origin: new Vector2(textureSet[0][0].Width / 2f, textureSet[0][0].Height / 2f),
            tag: tag,
            depth: 0.9f,
            textureSet: textureSet,
            frameTime: frameTime
        )
        {
            this.Speed = speed;
            this.ai = ai;
            this.InstructionTypes = new List<InstructionType>();
            this.Goals = new Queue<Vector2>();
            this.Path = new Queue<Vector2>();
            OnGoalComplete(position);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 position1 = this.Position;
            this.Position += PathFindingHelper.CalculateNextMove(gameTime, this);
            depth = (Position.Y + (Position.X / 2)) / 48000f;
            base.Update(gameTime);

            Vector2 delta = position1 - this.Position;

            //Math.Atan2(delta.X, delta.Y);

            if (delta.Equals(Vector2.Zero))
            {
                isAnimated = false;
            }
            else if (Math.Abs(delta.X) >= Math.Abs(delta.Y))
            {
                isAnimated = true;
                TextureGroupIndex = 1;
                SpriteEffect = (delta.X > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }
            else
            {
                isAnimated = true;
                TextureGroupIndex = (delta.Y > 0) ? 2 : 0;
            }


        }


        public void OnGoalComplete(Vector2 completedGoal)
        {
            Goals.Enqueue(this.Position + ai.RandomTranslation());
        }
    }
}
