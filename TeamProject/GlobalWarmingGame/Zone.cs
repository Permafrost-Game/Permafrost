using Engine;
using Engine.TileGrid;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using System.Collections.Generic;
using System;

namespace GlobalWarmingGame
{
    class Zone
    {
        public List<GameObject> GameObjects { get; private set; }  

        private Zone(List<GameObject> objects)
        {
            GameObjects = objects;
        }
        public static Zone GenerateZone(int seed, TileMap tileMap)
        {
            List<GameObject> GameObjects = new List<GameObject>();
            Random rand = new Random(seed);
            foreach (Tile t in tileMap.Tiles)
            {
                int item = rand.Next(0, 5);
                switch (item)
                {
                    case 0:
                        GameObjects.Add(InteractablesFactory.MakeTree(t.Position));
                        break;
                    case 1:
                        GameObjects.Add(InteractablesFactory.MakeBush(t.Position));
                        break;
                    case 2:
                        GameObjects.Add(InteractablesFactory.MakeStoneNode(t.Position));
                        break;
                    default:
                        GameObjects.Add(InteractablesFactory.MakeTallGrass(t.Position));
                        break; 
                }
            }
            return new Zone(GameObjects);
        }
    }
    
}