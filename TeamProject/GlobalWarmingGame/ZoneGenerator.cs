using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using Microsoft.Xna.Framework;
using SimplexNoise;
using System;

namespace GlobalWarmingGame
{
    static class ZoneGenerator
    {

        private static readonly int towerDistance = 7;
        private static readonly int towerSpan = 100;

        public static void SpawnGameObjects(int seed, Vector2 zonePos)
        {
            Random rand = new Random(seed);
            int tileSize = (int)GameObjectManager.ZoneMap.TileSize.Y;

            Vector2 zoneCenter = tileSize * (GameObjectManager.ZoneMap.Size / 2);

            if (zonePos.X % towerDistance == 0
                && zonePos.X < towerSpan
                && zonePos.X > -towerSpan
                && zonePos.Y % towerDistance == 0
                && zonePos.Y < towerSpan
                && zonePos.Y > -towerSpan
                && !zonePos.Equals(Vector2.Zero)
                && !GameObjectManager.ZoneMap.GetTileAtPosition(zoneCenter).Type.Equals("textures/tiles/main_tileset/water")
                )
            {
            
                GameObjectManager.Add((Tower)InteractablesFactory.MakeInteractable(Interactable.Tower, zoneCenter));
                
            }


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

                        else if (value < 0.1)
                        {
                            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.TallGrass, t.Position));
                        }
                        else if (value < 0.12)
                        {
                            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Rabbit, t.Position));
                        }
                    }
                }

                if (t.Type.Equals("textures/tiles/main_tileset/Stone"))
                {
                    if (value > 0.85)
                    {
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.StoneNodeSmall, t.Position));
                    }
                }

                if (t.Type.Equals("textures/tiles/main_tileset/Tundra1"))
                {
                    if (value > 0.9)
                    {
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                    }
                }
                if (t.Type.Equals("textures/tiles/main_tileset/Snow"))
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