using Engine.TileGrid;
using System;

namespace Engine.PathFinding
{
    public class PathFindingPathException : Exception
    {
        public Tile t;
        public PathFindingPathException(Tile t, string message) : base(message)
        {
            this.t = t;
        }

    }
}
