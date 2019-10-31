using Engine;
using Engine.PathFinding;
using Engine.TileGrid;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is for development only
    /// This class is for testing the path finder
    /// </summary>
    public class TestPathFindable : Sprite, IUpdatable, IClickable
    {
        private float speed;
        private Queue<Vector2> goals;
        public TestPathFindable(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag, float depth, Texture2D texture, float speed) :
            base(position, size, rotation, rotationOrigin, tag, depth, texture)
        {
            this.speed = speed;
            goals = new Queue<Vector2>();
        }

        public void AddGoal(Vector2 goal)
        {
            goals.Enqueue(goal);
        }

        public void OnClick(MouseState mouseState)
        {
            Queue<Tile> Goals = PathFinder.Find(this.Position, mouseState.Position.ToVector2(), false);
            foreach (Tile t in Goals)
            {
                AddGoal(t.Position);
            }
        }

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            if (goals.Count != 0)
            {
                Vector2 direction = goals.Peek() - Position;
                if (direction.Equals(Vector2.Zero))
                {
                    this.Position += direction;
                    goals.Dequeue();
                }

                this.Position += new Vector2(
                     direction.X < 0f ? Math.Max(-speed, direction.X) : Math.Min(+speed, direction.X),
                     direction.Y < 0f ? Math.Max(-speed, direction.Y) : Math.Min(+speed, direction.Y));


            }

        }

    }
}
