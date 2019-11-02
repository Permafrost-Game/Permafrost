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
    public class PathFindable : Sprite, IUpdatable, IClickable
    {
        private float speed;
        private Queue<Vector2> path;
        private Queue<Vector2> goals;
        public PathFindable(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag, float depth, Texture2D texture, float speed) :
            base(position, size, rotation, rotationOrigin, tag, depth, texture)
        {
            this.speed = speed;
            path = new Queue<Vector2>();
            goals = new Queue<Vector2>();
        }

        public void AddGoal(Vector2 goal)
        {
            goals.Enqueue(goal);
        }

        public void OnClick(MouseState mouseState)
        {
            AddGoal(mouseState.Position.ToVector2());
        }

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            if (path.Count == 0)
            {
                if (goals.Count != 0)
                {
                    Queue<Tile> paths;
                    try
                    {
                        paths = PathFinder.Find(this.Position, this.goals.Dequeue(), false);
                    }
                    catch (PathFindingPathException)
                    {
                        //Path is not a valid path
                        path.Clear();
                        return;
                    }

                    foreach (Tile t in paths)
                    {
                        path.Enqueue(t.Position);
                    }



                }
            }
            if(path.Count != 0)
            {
                Vector2 direction = path.Peek() - Position;
                if (direction.Equals(Vector2.Zero))
                {
                    this.Position += direction;
                    path.Dequeue();
                }

                this.Position += new Vector2(
                     direction.X < 0f ? Math.Max(-speed, direction.X) : Math.Min(+speed, direction.X),
                     direction.Y < 0f ? Math.Max(-speed, direction.Y) : Math.Min(+speed, direction.Y));
            }

        }

    }
}
