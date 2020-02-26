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
    static class ZoneGenerator
    {
        public static void SpawnGameObjects(int seed)
        {
            Random rand = new Random(seed);

            foreach (Tile t in GameObjectManager.ZoneMap.Tiles)
            {
                //int item = rand.Next(0, 100);
                float value = ((Noise.CalcPixel2D((int)t.Position.X, (int)t.Position.Y, 4f) / 255) + (Noise.CalcPixel2D((int)t.Position.X, (int)t.Position.Y, 0.1f) / 255)) / 2;
                
                if (!t.Type.Equals("textures/tiles/main_tileset/water"))
                {

                    if (t.Type.Equals("textures/tiles/main_tileset/Grass"))
                    {
                        if (value > 0.80 && value < 0.85)
                        {
                            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                        }
                        else if (value > 0.85 && value < 0.9)
                        {
                            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bush, t.Position));
                        }
                    }
                    else if (value < 0.1)
                    {
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.TallGrass, t.Position));
                    }
                    else if (value < 0.12)
                    {
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Rabbit, t.Position));
                    }
                }

                else if (t.Type.Equals("textures/tiles/main_tileset/Stone"))
                {
                    if (value > 0.85)
                    {
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.StoneNode, t.Position));
                    }
                }

                else if (t.Type.Equals("textures/tiles/main_tileset/Tundra1"))
                {
                    if (value > 0.9)
                    {
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                    }
                }
                else if (t.Type.Equals("textures/tiles/main_tileset/Snow"))
                {
                    if (value > 0.9)
                    {
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                    }

                    else if (rand.Next(0, 10000) == 99)
                    {
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bear, t.Position));
                    }
                }
            }
        }
    }
    
}