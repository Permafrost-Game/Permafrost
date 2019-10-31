using System;
using System.Collections.Generic;
using System.Linq;
using Engine.TileGrid;
using Microsoft.Xna.Framework;

namespace Engine
{
    public static class PathFinder
    {
        private static readonly TileMap tileMap = ZoneManager.CurrentZone.TileMap;
        private static Dictionary<Tile, TileCosts> openDictionary;
        private static Dictionary<Tile, TileCosts> closedSet;


       


        public static Queue<Tile> Find(Tile start, Tile goal, bool canPassOverObstacles)
        {
            //Create open dictionary with the start tile and leave closed queue empty
            openDictionary = new Dictionary<Tile, TileCosts>() { { start, new TileCosts(0, DistanceCalculator(start, goal)) } };
            closedSet = new Dictionary<Tile, TileCosts>();

            //While you still have places to look
            while (openDictionary.Count != 0)
            {

                Tile current = null; //Current = null
                double lowest = 1000000; //Lowest is a number that shouldn't be a possible total cost in map 

                //Loop through all possible tiles and select the one with the lowest total cost
                foreach (TileCosts i in openDictionary.Values)
                {
                    if (i.GetTotalCost() < lowest)
                    {
                        lowest = i.GetTotalCost();
                    }
                }

                //Get the current tile
                current = openDictionary.FirstOrDefault(x => x.Key.Equals(current)).Key;

                //Get the current tile's TileCosts
                TileCosts currentTotalCost = openDictionary[current];

                //Remove current from the open dictionary
                openDictionary.Remove(current);
                
                //Add current to the closed dictionary
                closedSet.Add(current, currentTotalCost);

                if (current.Equals(goal)) //If the current tile is the destination tile
                {
                    //RETURN THE QUEUE AND FINISH
                    return BuildPath(current, closedSet);
                }

                Dictionary<Tile, TileCosts> neighbourTiles = GetAdjacentTiles(current, goal, currentTotalCost, tileMap, canPassOverObstacles);

                foreach (Tile neighbour in neighbourTiles.Keys)
                {
                    if (closedSet.ContainsKey(neighbour))
                    { //If the neighbour is in the closed queue
                        continue;
                    }

                    if (!openDictionary.ContainsKey(neighbour)) //If neighbour is not in the openDictionary 
                    {          
                        //Add the neighbour to the open dictionary
                        openDictionary.Add(neighbour, neighbourTiles[neighbour]);
                    }
                    else { //Else we have potentially found a shorter path from the start to this neighbour tile in the open list
                        
                        //Version of neighbour already in the open dictionary
                        Tile openNeighbour = openDictionary.FirstOrDefault(x => x.Key.Equals(neighbour)).Key;
                        
                        //If the new fromStartTileCost for neighbour is less than the old cost
                        if (neighbourTiles[neighbour].FromStart < openDictionary[openNeighbour].FromStart) {
                            //Set the old cost to the new fromStartTileCost
                            openDictionary[openNeighbour].FromStart = neighbourTiles[neighbour].FromStart;
                            //Update the parent in the old version
                            openDictionary[openNeighbour].Parent = neighbourTiles[neighbour].Parent;
                        }
                    }
                }
            }

            //Search failed return null
            return null;
        }

        //Return a list of adjacent tiles
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

                if ((tileX - currentTileX) == tile.size.X || (tileX - currentTileX) == -tile.size.X) //If one tile to the left or right of current tile
                {
                    if (!canPassOverObstacles && !tile.Walkable) //If game object can't pass over game objects and tile is an obstacle
                    {
                        continue;
                    }

                    //Distance from start to neighbour through current tile
                    double neighbourToStartCost = currentTotalCost.FromStart + DistanceCalculator(current, tile); 
                    //Make a new total tile cost for neighbour and pass (Distance from start to neighbour through current tile) and (Direct distance from neighbour to goal tile)
                    TileCosts neighbourTileCosts = new TileCosts(neighbourToStartCost, DistanceCalculator(tile, goal));

                    neighbourTileCosts.Parent = current; //Set parent to current tile

                    tileList.Add(tile, neighbourTileCosts); //Add tile to adjacent list
                    continue;
                }
                else if ((tileY - currentTileY) == tile.size.Y || (tileY - currentTileY) == -tile.size.Y) //If one tile above or below of current tile
                {
                    if (!canPassOverObstacles && !tile.Walkable) //If game object can't pass over game objects and tile is an obstacle
                    {
                        continue;
                    }

                    //Distance from start to neighbour through current tile
                    double neighbourToStartCost = currentTotalCost.FromStart + DistanceCalculator(current, tile);
                    //Make a new total tile cost for neighbour and pass (Distance from start to neighbour through current tile) and (Direct distance from neighbour to goal tile)
                    TileCosts neighbourTileCosts = new TileCosts(neighbourToStartCost, DistanceCalculator(tile, goal));

                    neighbourTileCosts.Parent = current; //Set parent to current tile

                    tileList.Add(tile, neighbourTileCosts); //Add tile to adjacent list
                    continue;
                }
            }
            return tileList;
        }

        //Calculate the distance of a tile to another tile
        private static double DistanceCalculator(Tile current, Tile end)
        {
            double toEndCost; //Cost from this node to another node

            //Positions of the tiles in vector form
            Vector2 currentVector = current.Position;
            Vector2 endVector2 = end.Position;

            //Standard distance formula: distance = sqrt((X2-X1)^2 + (Y2-Y1)^2)
            toEndCost = Math.Sqrt((currentVector.X - endVector2.X) * (currentVector.X - endVector2.X) + (currentVector.Y - endVector2.Y) * (currentVector.Y - endVector2.Y));
            
            return toEndCost;
        }

        /** 
         * Using the parent tiles we can work our way back to the start from the end tile and store the path.
         *  Since the queue must have the starting tile at the front we have to push all of the tiles into a stack.
         *  Then pop the stack and enqueue all the tiles
         */
        private static Queue<Tile> BuildPath(Tile tile, Dictionary<Tile, TileCosts> closedTiles) {

            Stack<Tile> tileStack = new Stack<Tile>();
            Queue<Tile> tileQueueFromStart = new Queue<Tile>();

            //Current equals the end tile
            Tile current = tile;
            tileStack.Push(current);

            while (closedTiles[current].HasParent()) //This will stop once current is the start
            {
                current = closedTiles[current].Parent;
                tileStack.Push(current);
            }

            //Since the start is on top of the stack we can just pop and enqueue till the stack is empty
            for (int i = 0; i < tileStack.Count();i++)
            {
                tileQueueFromStart.Enqueue(tileStack.Pop());                                                
            }

            return tileQueueFromStart;
        }
    }
}
