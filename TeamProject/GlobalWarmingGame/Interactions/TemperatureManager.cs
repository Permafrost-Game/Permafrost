using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions
{
    /// <summary>
    /// TemperatureManager handles updating the temperatures of colonists and buildings
    /// based on the tile they are above and tiles around them respectively.
    /// </summary>
    public static class TemperatureManager
    {
        public static Temperature GlobalTemperature { get; set; } = new Temperature(-2);

        private static readonly float timeUntilTemperatureUpdate = 2000f;
        private static float timeToTemperatureUpdate = timeUntilTemperatureUpdate;

        private static readonly float timeUntilColonistTemperatureUpdate = 2000f;
        private static float timeToColonistTemperatureUpdate = timeUntilColonistTemperatureUpdate;

        private static readonly float timeToBuildingTemperatureUpdate = 2000f;
        private static float timeUntillBuildingTemperatureUpdate = timeToBuildingTemperatureUpdate;

        public static void UpdateTemperature(GameTime gameTime)
        {
            //TileMap temperature in the current zone
            timeToTemperatureUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeToTemperatureUpdate < 0)
            {
                UpdateTilesTemperatures(GlobalTemperature.Value);
                timeToTemperatureUpdate = timeUntilTemperatureUpdate;
            }

            //Colonist Temperature
            timeToColonistTemperatureUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToColonistTemperatureUpdate < 0f)
            {
                UpdateColonistTemperatures();
                timeToColonistTemperatureUpdate = timeUntilColonistTemperatureUpdate;
            }

            //Building Temperature
            timeUntillBuildingTemperatureUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeUntillBuildingTemperatureUpdate < 0)
            {
                UpdateBuildingTemperatures();
                timeUntillBuildingTemperatureUpdate = timeToBuildingTemperatureUpdate;
            }
        }

        #region Update TileMap Tiles' Temperatures
        /// <summary>
        /// Update each tile's temperature
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="globalTemperature"></param>
        private static void UpdateTilesTemperatures(float globalTemperature)
        {
            foreach (Tile tile in GameObjectManager.ZoneMap.Tiles)
            {
                //if tile is being heated by a structure it's temperature isn't affected by the cold
                if (tile.Heated)
                {
                    continue;
                }

                //Calculate new temperature as an average of surrounding tiles
                List<Tile> adjacentTiles = AdjacentTiles(tile);
                float sumTemperature = 0;

                foreach (Tile t in adjacentTiles)
                {
                    sumTemperature += t.Temperature.Value;
                }

                float averageTemperature = sumTemperature / adjacentTiles.Count;

                //Adjust the temperature based on the global temperature
                //Formula decreases temperature when sumTemperature > globalTemperature
                //and increases the temperature when sumTemperature < globalTemperature
                tile.Temperature.Value = averageTemperature + ((globalTemperature - averageTemperature) / 128);
            }
        }

        /// <summary>
        /// Calculate the adjacent tiles to the center tile
        /// </summary>
        /// <param name="tile">Center tile</param>
        /// <returns>List of adjacent tiles</returns>
        private static List<Tile> AdjacentTiles(Tile tile)
        {
            List<Tile> adjTiles = new List<Tile>();

            Vector2 v;
            float tileSize = tile.Size.X;

            if ((tile.Position.X - tileSize) >= 0)
            {
                v = new Vector2((tile.Position.X - tileSize), tile.Position.Y);
                adjTiles.Add(GameObjectManager.ZoneMap.GetTileAtPosition(v));
            }

            if ((tile.Position.X + tileSize) < GameObjectManager.ZoneMap.Size.X * tileSize)
            {

                v = new Vector2((tile.Position.X + tileSize), tile.Position.Y);
                adjTiles.Add(GameObjectManager.ZoneMap.GetTileAtPosition(v));

            }

            if ((tile.Position.Y - tileSize) >= 0)
            {

                v = new Vector2(tile.Position.X, (tile.Position.Y - tileSize));
                adjTiles.Add(GameObjectManager.ZoneMap.GetTileAtPosition(v));

            }

            if ((tile.Position.Y + tileSize) < GameObjectManager.ZoneMap.Size.Y * tileSize)
            {

                v = new Vector2(tile.Position.X, (tile.Position.Y + tileSize));
                adjTiles.Add(GameObjectManager.ZoneMap.GetTileAtPosition(v));

            }

            return adjTiles;
        }
        #endregion

        #region Update Colonist Temperatures
        /// <summary>
        /// Adjust the each colonist's temperature based on the tile they are over and their lower comfort range
        /// </summary>
        private static void UpdateColonistTemperatures()
        {
            //Adjust the temperatures of the colonists
            foreach (Colonist colonist in GameObjectManager.Filter<Colonist>())
            {
                float tileTemp = GameObjectManager.ZoneMap.GetTileAtPosition(colonist.Position).Temperature.Value;
                float colonistTemp = colonist.Temperature.Value;

                float absoluteTempDifference = MathHelper.Distance(colonistTemp, tileTemp);

                if (colonist.Temperature.Value < tileTemp)
                {
                    colonistTemp += absoluteTempDifference / 4;
                }
                else if (colonist.Temperature.Value > tileTemp)
                {
                    colonistTemp -= absoluteTempDifference / 8;
                }

                //Cap colonists temperature at -50 and 50
                colonist.Temperature.Value = MathHelper.Clamp(colonistTemp, colonist.TemperatureMin, colonist.TemperatureMax);

                //Console.Out.WriteLine("Colonist temp: " + colonist.Temperature.Value + " tile temperature: " + tileTemp + " health: " + colonist.Health);
            }
        }
        #endregion

        #region Update Building Temperatures
        /// <summary>
        /// Update the temperature of all buildings that produce heat
        /// </summary>
        private static void UpdateBuildingTemperatures()
        {
            foreach (IHeatSource heatSource in GameObjectManager.Filter<IHeatSource>())
            {
                float temperature = heatSource.Temperature.Value;
                Vector2 position = ((GameObject)heatSource).Position;
                Vector2 size = ((GameObject)heatSource).Size;

                //True if the building is currently producing heat
                bool heating = heatSource.Heating;
                HeatBuildingArea(position, size, temperature, heating, GameObjectManager.ZoneMap);
            }
        }

        /// <summary>
        /// Calculate the tile area the building takes up and heat the tiles.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="temperature"></param>
        /// <param name="heating">True if the building is currently producing heat</param>
        /// <param name="tileMap"></param>
        public static void HeatBuildingArea(Vector2 position, Vector2 size, float temperature, bool heating, TileMap tileMap)
        {
            float tileWidth = tileMap.Tiles[0, 0].Size.X;

            int numberOfTilesX = (int)(size.X / tileWidth);
            int numberOfTilesY = (int)(size.Y / tileWidth);

            //Heat up all the tiles occupied by the texture's space
            //From bottom left of the texture
            for (int y = 0; y < numberOfTilesY; y++)
            {
                for (int x = 0; x < numberOfTilesX; x++)
                {
                    //Position is offset in regards to temperature tests with objects
                    Tile t = tileMap.GetTileAtPosition(new Vector2(position.X - (size.X / 2.5f) + x * tileWidth,
                                                                   position.Y + (size.Y / 2.5f) - y * tileWidth));

                    if (heating)
                    {
                        //Prevent this tile's temperature from being reduced and set it to the buildings output temperature
                        t.Temperature.Value = temperature;
                        t.Heated = true;
                    }
                    else
                    {
                        //Allow this tile's temperature to be changed
                        t.Heated = false;
                    }
                }
            }
        }
        #endregion       
    }
}
