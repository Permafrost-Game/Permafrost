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

        public static Queue<Tile> Find(Vector2 start, Vector2 goal, bool canPassObstacles)
        {
            return Find(tileMap.GetTileAtPosition(start),
                        tileMap.GetTileAtPosition(goal),
                        canPassObstacles);
        }
        
        /// <summary>
        /// Takes a start tile, end tile and a bool if the pathfinding should consider obsticles
        /// </summary>
        /// <returns> Queue<Tile> from the start tile to the end tile </returns>
        public static Queue<Tile> Find(Tile start, Tile goal, bool canPassOverObstacles)
        {
            ///<summary>Create open dictionary with the start tile and leave closed queue empty</summary>
            openDictionary = new Dictionary<Tile, TileCosts>() { { start, new TileCosts(0, DistanceCalculator(start, goal)) } };
            closedDictionary = new Dictionary<Tile, TileCosts>();

            ///<summary>While you still have places to look</summary>
            while (openDictionary.Count != 0)
            {
                ///<summary>Current = null</summary>
                Tile current = null;

                ///<summary>Lowest is a number that shouldn't be a possible total cost in map</summary>
                double lowest = 1000000; 

                TileCosts lowestTileCost = null;

                ///<summary>Loop through all possible tiles and select the one with the lowest total cost</summary>
                foreach (TileCosts i in openDictionary.Values)
                {
                    if (i.GetTotalCost() < lowest)
                    {
                  
                        lowest = i.GetTotalCost();
                        lowestTileCost = i;

                    }
                }

                ///<summary>Get the current tile</summary>
                current = openDictionary.FirstOrDefault(x => x.Value.Equals(lowestTileCost)).Key;

                ///<summary>Get the current tile's TileCosts</summary>
                TileCosts currentTotalCost = openDictionary[current];

                ///<summary>Remove current from the open dictionary</summary>
                openDictionary.Remove(current);

                ///<summary>Add current to the closed dictionary</summary>
                closedDictionary.Add(current, currentTotalCost);

                ///<summary>If the current tile is the destination tile</summary>
                if (current.Equals(goal))
                {
                    ///<summary>RETURN THE QUEUE AND FINISH</summary>
                    return BuildPath(current, closedDictionary);
                }

                Dictionary<Tile, TileCosts> neighbourTiles = GetAdjacentTiles(current, goal, currentTotalCost, tileMap, canPassOverObstacles);

                foreach (Tile neighbour in neighbourTiles.Keys)
                {
                    ///<summary>If the neighbour is in the closed dictionary</summary>
                    if (closedDictionary.ContainsKey(neighbour))
                    {
                        continue;
                    }

                    ///<summary>If neighbour is not in the openDictionary</summary>
                    if (!openDictionary.ContainsKey(neighbour))
                    {
                        ///<summary>Add the neighbour to the open dictionary</summary>
                        openDictionary.Add(neighbour, neighbourTiles[neighbour]);
                    }

                    ///<summary>Else we have potentially found a shorter path from the start to this neighbour tile in the open list</summary>
                    else
                    { 
                        ///<summary>Version of neighbour already in the open dictionary</summary>
                        Tile openNeighbour = openDictionary.FirstOrDefault(x => x.Key.Equals(neighbour)).Key;

                        ///<summary>If the new fromStartTileCost for neighbour is less than the old cost</summary>
                        if (neighbourTiles[neighbour].FromStart < openDictionary[openNeighbour].FromStart)
                        {
                            ///<summary>Set the old cost to the new fromStartTileCost</summary>
                            openDictionary[openNeighbour].FromStart = neighbourTiles[neighbour].FromStart;

                            ///<summary>Update the parent in the old version</summary>
                            openDictionary[openNeighbour].Parent = neighbourTiles[neighbour].Parent;
                        }
                    }
                }
            }

            ///<summary>Search failed return null</summary>
            return null;
        }

        /// <summary>
        /// Look at all the map tiles and return the adjacent tiles
        /// </summary>
        /// <returns> Return a list of adjacent tiles </returns>
        private static Dictionary<Tile,TileCosts> GetAdjacentTiles(Tile current, Tile goal, TileCosts currentTotalCost, TileMap tMap, bool canPassOverObstacles)
        {
            ///<summary>Temp adjacent tile list</summary>
            Dictionary<Tile, TileCosts> tileList = new Dictionary<Tile, TileCosts>();
            double currentTileX = current.Position.X;
            double currentTileY = current.Position.Y;

            foreach (Tile tile in tMap.Tiles)
            {
                double tileX = tile.Position.X;
                double tileY = tile.Position.Y;

                ///<summary>Find the tiles that are diagonal, horizontal and vertical.</summary>
                if ((DistanceCalculator(current,tile) == tile.size.X) || (DistanceCalculator(current, tile) == (tile.size.X * Math.Sqrt(2)))) 
                {
                    ///<summary>If game object can't pass over game objects and tile is an obstacle</summary>
                    if (!canPassOverObstacles && !tile.Walkable) 
                    {
                        continue;
                    }

                    ///<summary>Distance from start to neighbour through current tile</summary>
                    double neighbourToStartCost = currentTotalCost.FromStart + DistanceCalculator(current, tile);

                    ///<summary>Make a new total tile cost for neighbour and pass (Distance from start to neighbour through current tile) and (Direct distance from neighbour to goal tile)</summary>
                    TileCosts neighbourTileCosts = new TileCosts(neighbourToStartCost, DistanceCalculator(tile, goal));

                    ///<summary>Set parent to current tile</summary>
                    neighbourTileCosts.Parent = current;

                    ///<summary>Add tile to adjacent list</summary>
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
            ///<summary>Cost from this node to another node</summary>
            double toEndCost;

            ///<summary>Positions of the tiles in vector form</summary>
            Vector2 currentVector = current.Position;
            Vector2 endVector2 = end.Position;

            ///<summary>Standard distance formula: distance = sqrt((X2-X1)^2 + (Y2-Y1)^2)</summary>
            toEndCost = Math.Sqrt((currentVector.X - endVector2.X) * (currentVector.X - endVector2.X) + (currentVector.Y - endVector2.Y) * (currentVector.Y - endVector2.Y));
            
            return toEndCost;
        }

        /// <summary>
        /// Using the parent tiles we can work our way back to the start from the end tile and store the path.
        /// Since the queue must have the starting tile at the front we have to push all of the tiles into a stack.
        /// Then pop the stack and enqueue all the tiles
        /// </summary>
        /// <returns> Queue<Tile> </returns>
        private static Queue<Tile> BuildPath(Tile tile, Dictionary<Tile, TileCosts> closedTiles) {

            Stack<Tile> tileStack = new Stack<Tile>();
            Queue<Tile> tileQueueFromStart = new Queue<Tile>();

            if (closedTiles != null) {

                ///<summary>Current equals the end tile</summary>
                Tile current = tile;
                tileStack.Push(current);

                ///<summary>This will stop once current is the start</summary>
                while (closedTiles[current].HasParent())
                {
                    tileStack.Push(closedTiles[current].Parent);
                    current = closedTiles[current].Parent;

                };

                int count = tileStack.Count();

                ///<summary>Since the start is on top of the stack we can just pop and enqueue till the stack is empty</summary>
                for (int i = 0; i < count; i++)
                {
                    tileQueueFromStart.Enqueue(tileStack.Pop());
                }

            }

            return tileQueueFromStart;
        }
    }
}
