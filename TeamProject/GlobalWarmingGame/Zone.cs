using Engine;
using Engine.TileGrid;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using System.Collections.Generic;
using System;
using SimplexNoise;
using GlobalWarmingGame.Interactions.Interactables;

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
                //int item = rand.Next(0, 100);
                float value = ((Noise.CalcPixel2D((int)t.Position.X, (int)t.Position.Y, 4f) / 255) + (Noise.CalcPixel2D((int)t.Position.X, (int)t.Position.Y, 0.1f) / 255)) / 2;
                if (!t.type.Equals("textures/tiles/main_tileset/water"))
                {

                    if (t.type.Equals("textures/tiles/main_tileset/Grass"))
                    {
                        if (value > 0.80)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                        }
                        if (value < 0.9 && value > 0.85)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bush, t.Position));
                        }
                        if (value < 0.1)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.TallGrass, t.Position));
                        }
                    }

                    if (t.type.Equals("textures/tiles/main_tileset/Stone"))
                    {
                        if (value > 0.85)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.StoneNode, t.Position));
                        }
                    }

                    if (t.type.Equals("textures/tiles/main_tileset/Tundra1"))
                    {
                        if (value > 0.9)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                        }
                    }
                    if (t.type.Equals("textures/tiles/main_tileset/Snow"))
                    {
                        if (value > 0.9)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                        }

                        if (rand.Next(0, 10000) == 99)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bear, t.Position));
                        }
                    }
                }
            }
            return new Zone(GameObjects);
        }
    }
    
}