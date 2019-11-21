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
        /// <returns> Return a list of adjacent tiles </returns>
        private static Dictionary<Tile,TileCosts> GetAdjacentTiles(Tile current, Tile goal, TileCosts currentTotalCost, TileMap tMap, bool canPassOverObstacles)
        {
            //Temp adjacent tile list
            Dictionary<Tile, TileCosts> tileList = new Dictionary<Tile, TileCosts>();
            double currentTileX = current.Position.X;
            double currentTileY = current.Position.Y;

            foreach (Tile tile in tMap.Tiles)
            {
                double tileX = tile.Position.X;
                double tileY = tile.Position.Y;

                //If game object can't pass over game objects and tile is an obstacle
                if (!canPassOverObstacles && !tile.Walkable)
                {
                    continue;
                }

                //Find the tiles that are diagonal, horizontal and vertical.
                if ((DistanceCalculator(current,tile) == tile.size.X) || (DistanceCalculator(current, tile) == (tile.size.X * Math.Sqrt(2)))) 
                {

                    //Distance from start to neighbour through current tile
                    double neighbourToStartCost = currentTotalCost.FromStart + DistanceCalculator(current, tile);

                    //Make a new total tile cost for neighbour and pass (Distance from start to neighbour through current tile) and (Direct distance from neighbour to goal tile)</summary>
                    TileCosts neighbourTileCosts = new TileCosts(neighbourToStartCost, DistanceCalculator(tile, goal))
                    {
                        Parent = current
                    };

                    tileList.Add(tile, neighbourTileCosts); 
                    continue;
                }
            }
            return tileList;
        }

        ///<summary>
        ///Calculate the distance of a tile to another tile
        ///</summary>
        ///<returns>double toEndCost</returns>
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
        /// <returns> Queue<Tile> </returns>
        private static Queue<Vector2> BuildPath(Tile tile, Dictionary<Tile, TileCosts> closedTiles) {

            Stack<Vector2> tileStack = new Stack<Vector2>();
            Queue<Vector2> tileQueueFromStart = new Queue<Vector2>();

            if (closedTiles != null) {

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
