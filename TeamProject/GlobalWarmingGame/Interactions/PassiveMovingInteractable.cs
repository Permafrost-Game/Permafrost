using Engine;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions
{
    class PassiveMovingGameObject : PathFindable, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; }

        public float Health { get; private set; }

        private Random Rand;
        private int NearMoves;

        public PassiveMovingGameObject(Vector2 position, Texture2D texture, List<InstructionType> instructionTypes) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0),
            tag: "Passive", //TODO rename tag
            depth: 0.5f,
            texture: texture,
            speed: 0.5f
        )
        {
            base.AddGoal(Position);
            NearMoves = 0;
            Rand = new Random();
            Health = 1f;
            InstructionTypes = instructionTypes;
        }

        //TODO Adjust queuing the goals
        protected override void PathComplete()
        {
            while (NearMoves < 10)
            {
                //Console.WriteLine(NearMoves);
                MoveAround(1f);
            }
            NearMoves = 0;
            MoveAround(8f);
        }

        private void MoveAround(float multiplier)
        {
            float i = Rand.Next(8);

            float offset = (texture.Width) * multiplier;

            Vector2 v = new Vector2();
            if (i == 0)
            {
                v.X = Position.X + offset;
                v.Y = Position.Y;
            }
            else if (i == 1)
            {
                v.X = Position.X - offset;
                v.Y = Position.Y;
            }
            else if (i == 2)
            {
                v.X = Position.X;
                v.Y = Position.Y + offset;
            }
            else if (i == 3)
            {
                v.X = Position.X;
                v.Y = Position.Y - offset;
            }
            else if (i == 4)
            {
                v.X = Position.X + offset;
                v.Y = Position.Y + offset;
            }
            else if (i == 5)
            {
                v.X = Position.X - offset;
                v.Y = Position.Y + offset;
            }
            else if (i == 6)
            {
                v.X = Position.X + offset;
                v.Y = Position.Y - offset;
            }
            else if (i == 7)
            {
                v.X = Position.X - offset;
                v.Y = Position.Y - offset;
            }
            NearMoves++;
            base.AddGoal(v);
        }
    }
}
