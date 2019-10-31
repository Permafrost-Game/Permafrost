using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is for development only
    /// This class is for testing the path finder
    /// </summary>
    public class TestPathFindable : Sprite, IUpdatable
    {
        private float speed;
        private Queue<Vector2> goals;
        public TestPathFindable(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, float depth, Texture2D texture, float speed) :
            base(position, size, rotation, rotationOrigin, depth, texture)
        {
            this.speed = speed;
            goals = new Queue<Vector2>();
        }

        public void AddGoal(Vector2 goal)
        {
            goals.Enqueue(goal);
        }

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            if (goals.Count != 0)
            {
                Vector2 direction = goals.Peek() - position;
                if (direction.Equals(Vector2.Zero))
                {
                    goals.Dequeue();
                }

                this.position += new Vector2(direction.X < 0f ? -speed : +speed,
                                            direction.Y < 0f ? -speed : +speed);
            }

        }

    }
}
