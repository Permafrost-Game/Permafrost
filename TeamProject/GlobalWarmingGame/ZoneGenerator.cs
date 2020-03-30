using Engine;
using Engine.TileGrid;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using System.Collections.Generic;
using System;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;

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
                GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tower, zoneCenter));

                for (int i = 0; i < rand.Next(1, 5); i++)
                {
                    GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Robot,
                        GameObjectManager.ZoneMap.GetTileAtPosition(zoneCenter).Type.Equals("textures/tiles/main_tileset/water") ?
                        zoneCenter :
                        zoneCenter + new Vector2(rand.Next(-tileSize * 3, tileSize * 3), rand.Next(-tileSize * 3, tileSize * 3))
                        ));
                }
            }

            FastNoise noise = new FastNoise(seed + 5);
            noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            noise.SetFrequency(0.005f);

            foreach (Tile t in GameObjectManager.ZoneMap.Tiles)
            {      
                float value = noise.GetNoise(t.Position.X, t.Position.Y);
                Console.WriteLine(value);

                if (t.Type.Equals("textures/tiles/main_tileset/Grass"))
                {
                    if (value > 0.35f || value < -0.35f)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));

                    else if (value < 0.002f && value > 0)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bush, t.Position));

                    else if (value > -0.002f && value < 0)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.TallGrass, t.Position));
                }

                else if (t.Type.Equals("textures/tiles/main_tileset/Stone"))
                {
                    if (value > 0.5f || value < -0.5f)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.StoneNodeSmall, t.Position));
                }

                else if (t.Type.Equals("textures/tiles/main_tileset/Tundra1"))
                {
                    if (value > 0.5f || value < -0.5f)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                }

                else if (t.Type.Equals("textures/tiles/main_tileset/Snow"))
                {
                    if (value > 0.6f || value < -0.6f)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                }
            }
        }
    }
}