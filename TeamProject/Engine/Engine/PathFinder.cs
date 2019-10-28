using System;
using System.Collections.Generic;
using System.Linq;
using Engine.TileGrid;
using Microsoft.Xna.Framework;

namespace Engine
{
    public static class PathFinder
    {
        //Declare open and closed dictionaries
        private static Dictionary<double, Tile> open;
        private static Dictionary<double, Tile> closed;
        private static TileMap tileMap = ZoneManager.currentZone.getTileMap();

        public static void find(Tile start, Tile goal)
        {
            //Create open dictionary with the start tile and leave closed null
            open = new Dictionary<double, Tile>() { { 0, start } };
            closed = new Dictionary<double, Tile>() { };

            //While you still have places to look
            while (open.Count != 0)
            {

                Tile current = null; //Current = null
                double lowest = 1000000; //Lowest is a really big number

                //Loop through all possible moves and select the one with the lowest cost
                foreach (int i in open.Keys)
                {
                    if (i < lowest)
                    {
                        lowest = i;
                    }
                }                
                current = open[lowest];

                double currentKey = open.FirstOrDefault(x => x.Value == current).Key;

                //Rempove current from the open dictionary
                open.Remove(currentKey);
                //Add current to the closed dictionary
                closed.Add(currentKey, current);

                if (current.Equals(goal)) //If the current tile is the destination tile
                {
                    //RETURN THE QUEUE THINGY AND FINISH
                    return;
                }





            }
        }

        //Calculate the heuristic of the tile
        private static double heuristicCalculator(Tile current, Tile start, Tile goal) {

            double totalCost; 
            double toStartCost; //Cost from current to start aka distance travelled
            double toGoalCost; //Cost to goal from current aka distance left to go

            //Positions of the tiles in vector form
            Vector2 currentVector = current.getPosition();
            Vector2 startVector2 = start.getPosition();
            Vector2 goalVector2 = goal.getPosition();
            
            //Standard distance formula: distance = sqrt((X2-X1)^2 + (Y2-Y1)^2)
            toStartCost = Math.Sqrt((currentVector.X - startVector2.X) * (currentVector.X - startVector2.X) + (currentVector.Y - startVector2.Y) * (currentVector.Y - startVector2.Y));
            toGoalCost = Math.Sqrt((currentVector.X - goalVector2.X) * (currentVector.X - goalVector2.X) + (currentVector.Y - goalVector2.Y) * (currentVector.Y - goalVector2.Y));

            //Heuristic used is totalCost = toStartCost + toGoalCost
            return (totalCost = toStartCost + toGoalCost);
        }
    }
}
