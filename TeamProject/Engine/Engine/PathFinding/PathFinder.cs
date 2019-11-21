using System;
using System.Collections.Generic;
using System.Linq;
using Engine.TileGrid;
using Microsoft.Xna.Framework;

namespace Engine.PathFinding
{
    public static class PathFinder
    {
        private static readonly TileMap tileMap = ZoneManager.CurrentZone.TileMap;
        private static Dictionary<Tile, TileCosts> openDictionary;
        private static Dictionary<Tile, TileCosts> closedDictionary;

        public static Queue<Vector2> Find(Vector2 start, Vector2 goal, bool canPassObstacles)
        {
            Tile startT = tileMap.GetTileAtPosition(start);
            Tile goalT = tileMap.GetTileAtPosition(goal);
            return Find(startT, goalT, canPassObstacles);
        }

        /// <summary>
        /// Takes a start tile, end tile and a bool if the pathfinding should consider obsticles
        /// </summary>
        /// <returns> Queue<Tile> from the start tile to the end tile </returns>
        public static Queue<Vector2> Find(Tile start, Tile goal, bool canPassOverObstacles)
        {
            if (start == null) throw new PathFindingPathException(start, $"Start Tile: \"{start}\" is null");
            if (goal == null) throw new PathFindingPathException(goal, $"Goal Tile: \"{goal}\" is null");
            if (!start.Walkable) throw new PathFindingPathException(start, $"Start Tile: \"{start}\" is not walkable");
            if (!goal.Walkable) throw new PathFindingPathException(goal, $"Goal Tile: \"{goal}\" is not walkable");

            openDictionary = new Dictionary<Tile, TileCosts>() { { start, new TileCosts(0, DistanceCalculator(start, goal)) } };
            closedDictionary = new Dictionary<Tile, TileCosts>();

            ///<summary>While you still have places to look</summary>
            while (openDictionary.Count != 0)
            {

                Tile current = null;

                //Lowest is a number that shouldn't be a possible total cost in map
                double lowest = 1000000;

                TileCosts lowestTileCost = null;

                //Loop through all possible tiles and select the one with the lowest total cost
                foreach (TileCosts i in openDictionary.Values)
                {
                    if (i.GetTotalCost() < lowest)
                    {

                        lowest = i.GetTotalCost();
                        lowestTileCost = i;

                    }
                }

                //Get the current tile
                current = openDictionary.FirstOrDefault(x => x.Value.Equals(lowestTileCost)).Key;

                //Get the current tile's TileCosts
                TileCosts currentTotalCost = openDictionary[current];

                openDictionary.Remove(current);

                closedDictionary.Add(current, currentTotalCost);

                if (current.Equals(goal))
                {
                    return BuildPath(current, closedDictionary);
                }

                Dictionary<Tile, TileCosts> neighbourTiles = GetAdjacentTiles(current, goal, currentTotalCost, tileMap, canPassOverObstacles);

                foreach (Tile neighbour in neighbourTiles.Keys)
                {
                    //If the neighbour is in the closed dictionary
                    if (closedDictionary.ContainsKey(neighbour))
                    {
                        continue;
                    }

                    if (!openDictionary.ContainsKey(neighbour))
                    {
                        //Add the neighbour to the open dictionary</summary>
                        openDictionary.Add(neighbour, neighbourTiles[neighbour]);
                    }

                    //Else we have potentially found a shorter path from the start to this neighbour tile in the open list
                    else
                    {
                        //Version of neighbour already in the open dictionary
                        Tile openNeighbour = openDictionary.FirstOrDefault(x => x.Key.Equals(neighbour)).Key;

                        //If the new fromStartTileCost for neighbour is less than the old cost
                        if (neighbourTiles[neighbour].FromStart < openDictionary[openNeighbour].FromStart)
                        {
                            //Set the old cost to the new fromStartTileCost
                            openDictionary[openNeighbour].FromStart = neighbourTiles[neighbour].FromStart;

                            //>Update the parent in the old version
                            openDictionary[openNeighbour].Parent = neighbourTiles[neighbour].Parent;
                        }
                    }
                }
            }

            //Search failed return null
            return null;
        }

        /// <summary>
        /// Look at all the map tiles and return the adjacent tiles
        /// </summary>
        /// <returns> list of adjacent tiles </returns>
        private static Dictionary<Tile, TileCosts> GetAdjacentTiles(Tile current, Tile goal, TileCosts currentTotalCost, TileMap tMap, bool canPassOverObstacles)
        {
            //Temp adjacent tile list
            Dictionary<Tile, TileCosts> tileList = new Dictionary<Tile, TileCosts>();
            
            int[] xDirections = new int[] { 1, -1, 0, 0, 1, -1, 1, -1 };
            int[] yDirections = new int[] { 0, 0, 1, -1, 1, 1, -1, -1 };

            //Loop through all 8 possible adjacent tiles
            for (int i = 0; i < xDirections.Length; i++)
            {
                Tile tile = AdjacentTile(current, tMap, canPassOverObstacles, xDirections[i], yDirections[i]);

                //TileFinder will only return null if the tile is not passable or outside of the tilemap
                if (tile != null)
                {
                    //Distance from start to neighbour through current tile
                    double neighbourToStartCost = currentTotalCost.FromStart + DistanceCalculator(current, tile);

                    //Make a new total tile cost for neighbour and pass (Distance from start to neighbour through current tile) and (Direct distance from neighbour to goal tile)</summary>
                    TileCosts neighbourTileCosts = new TileCosts(neighbourToStartCost, DistanceCalculator(tile, goal))
                    {
                        Parent = current
                    };

                    tileList.Add(tile, neighbourTileCosts);
                }
            }

            return tileList;
        }

        /// <summary>
        /// Find a tile within the map that is one tile away from the current tile 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="tMap"></param>
        /// <param name="changeX"></param>
        /// <param name="changeY"></param>
        /// <returns> adjacent tile </returns>
        private static Tile AdjacentTile(Tile current, TileMap tMap, bool canPassOverObstacles, int changeX, int changeY)
        {
            int x = (int)current.Position.X / (int)Math.Round(current.size.X);
            int y = (int)current.Position.Y / (int)Math.Round(current.size.Y);

            Tile t = null;
            
            //If tile is in the tile map
            if ((x + changeX) >= 0 && (y + changeY) >= 0 && (x + changeX) < tMap.Tiles.GetLength(0) && (y + changeX) < tMap.Tiles.GetLength(1))
            {
                t = tMap.Tiles[(x + changeX), (y + changeY)];

                //If game object can't pass over game objects and tile is an obstacle
                if (!canPassOverObstacles && !t.Walkable)
                {
                    return null;
                }
            }

            return t;
        }

        ///<summary>
        ///Calculate the distance of a tile to another tile
        ///</summary>
        ///<returns> double toEndCost </returns>
        private static double DistanceCalculator(Tile current, Tile end)
        {
            //Cost from this node to another node
            double toEndCost;

            //Positions of the tiles in vector form
            Vector2 currentVector = current.Position;
            Vector2 endVector2 = end.Position;

            //Standard distance formula: distance = sqrt((X2-X1)^2 + (Y2-Y1)^2)
            toEndCost = Math.Sqrt((currentVector.X - endVector2.X) * (currentVector.X - endVector2.X) + (currentVector.Y - endVector2.Y) * (currentVector.Y - endVector2.Y));

            return toEndCost;
        }

        /// <summary>
        /// Using the parent tiles we can work our way back to the start from the end tile and store the path.
        /// Since the queue must have the starting tile at the front we have to push all of the tiles into a stack.
        /// Then pop the stack and enqueue all the tiles
        /// </summary>
        /// <returns> Queue<Vector2> </returns>
        private static Queue<Vector2> BuildPath(Tile tile, Dictionary<Tile, TileCosts> closedTiles)
        {

            Stack<Vector2> tileStack = new Stack<Vector2>();
            Queue<Vector2> tileQueueFromStart = new Queue<Vector2>();

            if (closedTiles != null)
            {

                //Current equals the end tile
                Tile current = tile;
                tileStack.Push(current.Position);

                //This will stop once current is the start
                while (closedTiles[current].HasParent())
                {
                    tileStack.Push(closedTiles[current].Parent.Position);
                    current = closedTiles[current].Parent;

                };

                int count = tileStack.Count();

                //Since the start is on top of the stack we can just pop and enqueue till the stack is empty
                for (int i = 0; i < count; i++)
                {
                    tileQueueFromStart.Enqueue(tileStack.Pop());
                }

            }

            return tileQueueFromStart;
        }
    }
}
