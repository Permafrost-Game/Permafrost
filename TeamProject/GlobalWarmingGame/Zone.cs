using Engine;
using Engine.TileGrid;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using System.Collections.Generic;
using System;
using SimplexNoise;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;

namespace GlobalWarmingGame
{
    class Zone
    {
        public List<GameObject> GameObjects { get; private set; }  

        private Zone(List<GameObject> objects)
        {
            GameObjects = objects;
        }
        public static Zone GenerateZone(int seed, TileMap tileMap, Vector2 zonePos)
        {
            List<GameObject> GameObjects = new List<GameObject>();
            Random rand = new Random(zonePos.GetHashCode());
            if (zonePos.Equals(new Vector2(5,0)) || zonePos.Equals(new Vector2(0, 2)) || zonePos.Equals(new Vector2(0, -5)))
            {
                GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tower, new Vector2((32*50),(32*50))));
                for (int i = rand.Next(0, 5); i < 5; i++) {
                    GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Robot, new Vector2(((rand.Next(24, 40)) * 50), ((rand.Next(24, 40)) * 50))));
                }
            }
            foreach (Tile t in tileMap.Tiles)
            {
                //int item = rand.Next(0, 100);
                float value = ((Noise.CalcPixel2D((int)t.Position.X, (int)t.Position.Y, 4f) / 255) + (Noise.CalcPixel2D((int)t.Position.X, (int)t.Position.Y, 0.1f) / 255)) / 2;
                if (!t.Type.Equals("textures/tiles/main_tileset/water"))
                {

                    if (t.Type.Equals("textures/tiles/main_tileset/Grass"))
                    {
                        if (value > 0.80)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                        }
                        else if (value < 0.9 && value > 0.85)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bush, t.Position));
                        }
                        else if(value < 0.1)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.TallGrass, t.Position));
                        }
                        else if(value < 0.12)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Rabbit, t.Position));
                        }
                    }

                    if (t.Type.Equals("textures/tiles/main_tileset/Stone"))
                    {
                        if (value > 0.85)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.StoneNode, t.Position));
                        }
                    }

                    if (t.Type.Equals("textures/tiles/main_tileset/Tundra1"))
                    {
                        if (value > 0.9)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                        }
                    }
                    if (t.Type.Equals("textures/tiles/main_tileset/Snow"))
                    {
                        if (value > 0.9)
                        {
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                        }

                        else if(rand.Next(0, 10000) == 99)
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