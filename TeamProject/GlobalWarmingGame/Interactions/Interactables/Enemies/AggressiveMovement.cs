﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlobalWarmingGame.Interactions.Enemies
{
    class AggressiveMovement : PathFindable, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; }
        public float Health { get; private set; }

        private readonly Random Rand;
        private int NearMoves;

        private float offset;

        private readonly float[] xDirections;
        private readonly float[] yDirections;

        public AggressiveMovement(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag, float depth, Texture2D texture, List<InstructionType> instructionTypes, float speed) : base
        (
            position: position,
            size: size,
            rotation: rotation,
            rotationOrigin: rotationOrigin,
            tag: tag,
            depth: depth,
            texture: texture,
            speed: speed
        )
        {
            AddGoal(Position);
            NearMoves = 0;
            Rand = new Random();

            Health = 1f;
            InstructionTypes = instructionTypes;

            offset = texture.Width;
            xDirections = new float[] { offset, -offset, 0, 0, offset, -offset, offset, -offset };
            yDirections = new float[] { 0, 0, offset, -offset, offset, offset, -offset, -offset };
        }

        //TODO Adjust queuing the goals
        protected override void OnGoalComplete(Vector2 completedGoal)
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
            AddGoal(v);
        }
    }
}