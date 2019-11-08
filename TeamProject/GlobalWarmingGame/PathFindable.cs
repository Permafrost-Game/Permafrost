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
    /// This class allows for the navigation of sprites through across the TileMap
    /// </summary>
    public abstract class PathFindable : Sprite, IUpdatable, IClickable
    {
        protected float speed;
        protected Queue<Vector2> goals;
        private Queue<Vector2> path;
        
        public PathFindable(Vector2 position, Vector2 size, float rotation, Vector2 rotationOrigin, string tag, float depth, Texture2D texture, float speed) :
            base(position, size, rotation, rotationOrigin, tag, depth, texture)
        {
            this.speed = speed;
            path = new Queue<Vector2>();
            goals = new Queue<Vector2>();
        }

        /// <summary>
        /// Adds a Goal
        /// </summary>
        /// <param name="goal">The position of the place this object should navigate to</param>
        public void AddGoal(Vector2 goal)
        {
            goals.Enqueue(goal);
        }

        public void OnClick(Vector2 Position)
        {
            AddGoal(Position);
        }

        //public void OnClick(Point clickPos)
        //{
        //    AddGoal(new Vector2(clickPos.X, clickPos.Y));
        //}

        public virtual void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        /// <summary>
        /// Moves the sprite towards the next goal
        /// </summary>
        /// <param name="gameTime"></param>
        private void Move(GameTime gameTime)
        {
            if (path.Count == 0)
            {
                PathComplete();
                foreach (Tile t in QueueNextPath())
                {
                    path.Enqueue(t.Position);
                }
            }

            if (path.Count != 0)
            {
                Vector2 direction = path.Peek() - Position;
                if (direction.Equals(Vector2.Zero))
                {
                    this.Position += direction;
                    path.Dequeue();
                }
                //TODO game time
                this.Position += new Vector2(
                        direction.X < 0f ? Math.Max(-speed, direction.X) : Math.Min(+speed, direction.X),
                        direction.Y < 0f ? Math.Max(-speed, direction.Y) : Math.Min(+speed, direction.Y)
                        );
            }
        }

        /// <summary>
        /// Takes the next Goal and calculates it's path
        /// </summary>
        /// <remarks>This method may be overridden to adjust functionality</remarks>
        /// <returns></returns>
        protected virtual Queue<Tile> QueueNextPath()
        {
            Queue<Tile> paths = new Queue<Tile>();
            if (goals.Count != 0)
            {
                try
                {
                    paths = PathFinder.Find(this.Position, this.goals.Dequeue(), false);
                }
                catch (PathFindingPathException)
                {
                    //Path is not a valid path
                    path.Clear();
                    return paths;
                }

                return paths;
            }
            //No more goals
            return paths;
        }

        /// <summary>
        /// Called when the object has finished a Goal, called before the next goal is loaded.
        /// </summary>
        protected abstract void PathComplete();

    }
}
