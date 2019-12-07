using Engine.TileGrid;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.PathFinding
{
    public static class PathFindingHelper
    {

        public static Vector2 CalculateNextMove(GameTime gameTime, IPathFindable pathFindable)
        {
            if (pathFindable.Goals.Count != 0)
            {
                if (pathFindable.Path.Count != 0)
                {
                    Vector2 position = ((GameObject)pathFindable).Position;
                    Vector2 direction = pathFindable.Path.Peek() - position;
                    if (pathFindable.Path.Peek().Equals(position))
                    {
                        pathFindable.Path.Dequeue();

                        if (pathFindable.Path.Count == 0)
                        {
                            pathFindable.OnGoalComplete(pathFindable.Goals.Dequeue());
                        }
                    }

                    float speed = pathFindable.Speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    return new Vector2(
                            (direction.X < 0f ? Math.Max(-speed, direction.X) : Math.Min(+speed, direction.X)),
                            (direction.Y < 0f ? Math.Max(-speed, direction.Y) : Math.Min(+speed, direction.Y))
                           );

                }
                else
                {
                    foreach (Tile t in EnqueueNextPath(pathFindable))
                    {
                        pathFindable.Path.Enqueue(t.Position);
                    }
                    if (pathFindable.Path.Count == 0)
                    {
                        pathFindable.OnGoalComplete(pathFindable.Goals.Dequeue());
                    }
                }
            }
            return Vector2.Zero;
        }

        private static Queue<Tile> EnqueueNextPath(IPathFindable pathFindable)
        {
            Queue<Tile> paths = new Queue<Tile>();
            try
            {
                pathFindable.Path = PathFinder.Find(((GameObject)pathFindable).Position, pathFindable.Goals.Peek(), false);
                if (pathFindable.Path.Count == 0)
                {
                    pathFindable.OnGoalComplete(pathFindable.Goals.Dequeue());
                }
            }

            catch (PathFindingPathException)
            {
                //Path is not a valid path
                pathFindable.Path.Clear();
            }

            return paths;
        }

    }
}
