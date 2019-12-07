using Engine;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions
{
    class PassiveMovingGameObject : Sprite, Engine.IUpdatable, IPathFindable, IInteractable
    {
        private readonly Random Rand;
        private int NearMoves;

        private float offset;

        private readonly float[] xDirections;
        private readonly float[] yDirections;

        public float Health { get; private set; }
        public List<InstructionType> InstructionTypes { get; }
        public Queue<Vector2> Goals { get; set; }
        public Queue<Vector2> Path { get; set; }
        public float Speed { get; set; }

        public PassiveMovingGameObject(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag, float depth, Texture2D texture, List<InstructionType> instructionTypes, float speed) : base
        (
            position: position,
            size: size,
            rotation: rotation,
            rotationOrigin: rotationOrigin,
            tag: tag,
            depth: depth,
            texture: texture
        )
        {
            Rand = new Random();
            NearMoves = 0;
            offset = texture.Width;
            xDirections = new float[] { offset, -offset, 0, 0, offset, -offset, offset, -offset };
            yDirections = new float[] { 0, 0, offset, -offset, offset, offset, -offset, -offset };

            this.Health = 1f;
            this.InstructionTypes = instructionTypes;
            this.Goals = new Queue<Vector2>();
            this.Goals.Enqueue(Position);
            this.Path = new Queue<Vector2>();
            this.Speed = speed;
            
        }

        //TODO Adjust queuing the goals
        public void OnGoalComplete(Vector2 completedGoal)
        {
            if (NearMoves < 15)
            {
                MoveAround(1f);
            }
            else
            {
                NearMoves = 0;
                MoveAround(8f);
            }
        }

        private void MoveAround(float multiplier)
        {
            int i = Rand.Next(8);

            Vector2 v = new Vector2
            {
                X = Position.X + xDirections[i] * multiplier,
                Y = Position.Y + yDirections[i] * multiplier
            };
            
            NearMoves++;
            Goals.Enqueue(v);
        }

        public void Update(GameTime gameTime)
        {
            this.Position += PathFindingHelper.CalculateNextMove(gameTime, this);
        }
    }
}
