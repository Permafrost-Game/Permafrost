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
                int item = rand.Next(0, 100);
                Console.WriteLine(t.type);
                if (! t.type.Equals("textures/tiles/main_tileset/water")) {
                    if (t.type.Equals("textures/tiles/main_tileset/Stone"))
                    {
                        if (item > 85)
                        {
                            GameObjects.Add(InteractablesFactory.MakeStoneNode(t.Position));
                        }
                    }
                    if (t.type.Equals("textures/tiles/main_tileset/Grass"))
                    {
                        if (item > 90)
                        {
                            GameObjects.Add(InteractablesFactory.MakeTree(t.Position));
                        }
                        if (item < 90 && item > 85)
                        {
                            GameObjects.Add(InteractablesFactory.MakeBush(t.Position));
                        }
                        if (item < 10) {
                            GameObjects.Add(InteractablesFactory.MakeTallGrass(t.Position));
                        }
                    }
                    if (t.type.Equals("textures/tiles/main_tileset/Tundra1"))
                    {
                        if (item > 97)
                        {
                            GameObjects.Add(InteractablesFactory.MakeTree(t.Position));
                        }
                    }
                    if (t.type.Equals("textures/tiles/main_tileset/Snow"))
                    {
                        if (item > 97)
                        {
                            GameObjects.Add(InteractablesFactory.MakeTree(t.Position));
                        }
                    }
                }
            }
            return new Zone(GameObjects);
        }
    }
    
}