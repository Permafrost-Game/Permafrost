using Engine;
using Engine.TileGrid;
using Engine.Drawing;
using GlobalWarmingGame.Interactions;
using System.Collections.Generic;
using System;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
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
            
                GameObjectManager.Add((Tower)InteractablesFactory.MakeInteractable(Interactable.Tower, zoneCenter));
                
            }

            seed++;
            FastNoise noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
            noise.SetFrequency(0.005f);
            noise.SetFractalOctaves(1);

            Random random = new Random(seed);
            int chance;
            int tileCount = GameObjectManager.ZoneMap.Tiles.Length;

            foreach (Tile t in GameObjectManager.ZoneMap.Tiles)
            {
                float value = noise.GetNoise(t.Position.X, t.Position.Y);           

                if (t.Type.Equals("textures/tiles/main_tileset/Grass"))
                {
                    if (value > 0.4f || value < -0.4f)
                    {
                        chance = random.Next(tileCount);
                        if (chance < 6666)
                            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                    }

                    else if (value < 0.01f && value > 0)
                    {
                        chance = random.Next(tileCount);
                        if (chance < 6666)
                            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bush, t.Position));
                    }

                    else if (value > -0.01f && value < 0)
                    {
                        chance = random.Next(tileCount);
                        if (chance < 6666)
                            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.TallGrass, t.Position));
                    }
                }

                else if (t.Type.Equals("textures/tiles/main_tileset/Stone"))
                {
                    chance = random.Next(tileCount);

                    if (chance < 750)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.StoneNodeSmall, t.Position));

                    else if (chance < 1000)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.StoneNodeBig, t.Position));
                }

                else if (t.Type.Equals("textures/tiles/main_tileset/Tundra1"))
                {
                    chance = random.Next(tileCount);
                    if (value > 0.4f || value < -0.4f)
                        if (chance < 750)
                            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                }

                else if (t.Type.Equals("textures/tiles/main_tileset/Snow"))
                {
                    chance = random.Next(tileCount);
                    if (value > 0.4f || value < -0.4f)
                        if (chance < 1000)
                            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, t.Position));
                }

                chance = random.Next(tileCount);
                if (!t.Type.Equals("textures/tiles/main_tileset/Stone") && !t.Type.Equals("textures/tiles/main_tileset/water"))
                    if (chance < 15)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Rabbit, t.Position));

                chance = random.Next(tileCount);
                if (t.Type.Equals("textures/tiles/main_tileset/Snow"))
                    if(chance < 5)
                        GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bear, t.Position));
            }
        }
    }
}