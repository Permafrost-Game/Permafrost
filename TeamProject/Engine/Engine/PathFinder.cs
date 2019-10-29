using System;
using System.Collections.Generic;
using System.Linq;
using Engine.TileGrid;
using Microsoft.Xna.Framework;

namespace Engine
{
    public static class PathFinder
    {
        private static TileMap tileMap = ZoneManager.currentZone.getTileMap();
        private static Dictionary<Tile, TileCosts> openDictionary;
        private static HashSet<Tile> closedSet;

        public static Queue<Tile> find(Tile start, Tile goal, bool canPassOverObstacles)
        {
            //Create open dictionary with the start tile and leave closed queue empty
            openDictionary = new Dictionary<Tile, TileCosts>() { { start, new TileCosts(0, distanceCalculator(start, goal)) } };
            closedSet = new HashSet<Tile>();

            //While you still have places to look
            while (openDictionary.Count != 0)
            {

                Tile current = null; //Current = null
                double lowest = 1000000; //Lowest is a number that shouldn't be possible 

                //Loop through all possible tiles and select the one with the lowest total cost
                foreach (TileCosts i in openDictionary.Values)
                {
                    if (i.getTotalCost() < lowest)
                    {
                        lowest = i.getTotalCost();
                    }
                }

                //Get the current tile
                current = openDictionary.FirstOrDefault(x => x.Key.equals(current)).Key;

                //Get the current tile's TileCosts
                TileCosts currentTotalCost = openDictionary[current];

                //Remove current from the open dictionary
                openDictionary.Remove(current);
                
                //Add current to the closed dictionary
                closedSet.Add(current);

                if (current.Equals(goal)) //If the current tile is the destination tile
                {
                    //RETURN THE QUEUE AND FINISH
                    return buildPath(current);
                }

                foreach (Tile neighbour in getAdjacentTiles(current, tileMap, canPassOverObstacles))
                {
                    if (closedSet.Contains(neighbour))
                    { //If the neighbour is in the closed queue
                        continue;
                    }

                    double neighbourToStartCost = currentTotalCost.getFromStartCost() + distanceCalculator(current, neighbour); //Distance from start to neighbour through current tile

                    //Make a new total tile cost for neighbour and pass (Distance from start to neighbour through current tile) and (Direct distance from neighbour to goal tile)
                    TileCosts neighbourTileCosts = new TileCosts(neighbourToStartCost, distanceCalculator(neighbour, goal));

                    if (!openDictionary.ContainsKey(neighbour)) //If neighbour is not in the openDictionary 
                    {          
                        //Add the neighbour to the open dictionary
                        openDictionary.Add(neighbour, neighbourTileCosts);
                    }
                    else { //Else we have potentially found a shorter path from the start to this neighbour tile in the open list
                        
                        //Version of neighbour already in the open dictionary
                        Tile openNeighbour = openDictionary.FirstOrDefault(x => x.Key.equals(neighbour)).Key;
                        
                        //If the new fromStartTileCost for neighbour is less than the old cost
                        if (neighbourTileCosts.getFromStartCost() < openDictionary[openNeighbour].getFromStartCost()) {
                            //Set the old cost to the new fromStartTileCost
                            openDictionary[openNeighbour].setFromStart(neighbourTileCosts.getFromStartCost());
                            //Update the parent in the old version
                            openNeighbour.setParent(neighbour.getParent());
                        }
                    }
                }
            }

            //Search failed return null
            return null;
        }

        //Return a list of adjacent tiles
        private static List<Tile> getAdjacentTiles(Tile current, TileMap tMap, bool canPassOverObstacles)
        {
            //Temp adjacent tile list
            List<Tile> tileList = new List<Tile>();
            double currentTileX = current.getPosition().X;
            double currentTileY = current.getPosition().Y;

            foreach (Tile tile in tMap.Tiles)
            {
                double tileX = tile.getPosition().X;
                double tileY = tile.getPosition().Y;

                if ((tileX - currentTileX) == tile.getSize().X || (tileX - currentTileX) == -tile.getSize().X) //If one tile to the left or right of current tile
                {
                    if (!canPassOverObstacles && !tile.getWalkable()) //If game object can't pass over game objects and tile is an obstacle
                    {
                        continue;
                    }
                    tile.setParent(current); //Set parent to current tile
                    tileList.Add(tile); //Add tile to adjacent list
                    continue;
                }
                else if ((tileY - currentTileY) == tile.getSize().Y || (tileY - currentTileY) == -tile.getSize().Y) //If one tile above or below of current tile
                {
                    if (!canPassOverObstacles && !tile.getWalkable()) //If game object can't pass over game objects and tile is an obstacle
                    {
                        continue;
                    }
                    tile.setParent(current); //Set parent to current tile
                    tileList.Add(tile); //Add tile to adjacent list
                    continue;
                }
            }
            return tileList;
        }

        //Calculate the distance of a tile to another tile
        private static double distanceCalculator(Tile current, Tile end)
        {
            double toEndCost; //Cost from this node to another node

            //Positions of the tiles in vector form
            Vector2 currentVector = current.getPosition();
            Vector2 endVector2 = end.getPosition();

            //Standard distance formula: distance = sqrt((X2-X1)^2 + (Y2-Y1)^2)
            toEndCost = Math.Sqrt((currentVector.X - endVector2.X) * (currentVector.X - endVector2.X) + (currentVector.Y - endVector2.Y) * (currentVector.Y - endVector2.Y));
            
            return toEndCost;
        }

        /** Using the parent tiles we can work our way back to the start from the end tile and store the path.
         *  Since the queue must have the starting tile at the front we have to push all of the tiles into a stack.
         *  Then pop the stack and enqueue all the tiles
         */
        private static Queue<Tile> buildPath(Tile tile) {

            Stack<Tile> tileStack = new Stack<Tile>();
            Queue<Tile> tileQueueFromStart = new Queue<Tile>();

            //Current equals the end tile
            Tile current = tile;
            tileStack.Push(current);

            while (current.hasParent()) //This will stop once current is the start
            {
                current = current.getParent();
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
