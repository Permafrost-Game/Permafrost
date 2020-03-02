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

        private static readonly int towerDistance = 7;
        private static readonly int towerSpan = 100;

        public List<GameObject> GameObjects { get; private set; }  

        private Zone(List<GameObject> objects)
        {
            GameObjects = objects;
        }

        public static Zone GenerateZone(int seed, TileMap tileMap, Vector2 zonePos)
        {
            List<GameObject> GameObjects = new List<GameObject>();
            Random rand = new Random(zonePos.GetHashCode());
            Vector2 zoneCenter = 32 *(GameObjectManager.ZoneMap.Size / 2);

            if (   zonePos.X % towerDistance == 0
                && zonePos.X < towerSpan
                && zonePos.X > -towerSpan
                && zonePos.Y % towerDistance == 0
                && zonePos.Y < towerSpan
                && zonePos.Y > -towerSpan
                && !zonePos.Equals(Vector2.Zero)
                && !tileMap.GetTileAtPosition(zoneCenter).Type.Equals("textures/tiles/main_tileset/water")
                )
            {
                GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tower, zoneCenter));

                for (int i = 0; i < rand.Next(1, 5); i++) {
                    GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Robot,
                        tileMap.GetTileAtPosition(zoneCenter).Type.Equals("textures/tiles/main_tileset/water")?
                        zoneCenter :
                        zoneCenter + new Vector2(rand.Next(-128, 128), rand.Next(-128, 128))
                        ));
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
                            GameObjects.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.StoneNodeSmall, t.Position));
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