using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;

namespace GlobalWarmingGame.Interactions
{
    /// <summary>
    /// TemperatureManager handles updating the temperatures of colonists and buildings
    /// based on the tile they are above and tiles around them respectively.
    /// </summary>
    public static class TemperatureManager
    {
        public static Temperature GlobalTemperature { get; set; } = new Temperature(-2);

        private static float timeToColonistTemperatureUpdate = 0f;
        private static readonly float timeUntilColonistTemperatureUpdate = 2000f;

        private static float timeUntillBuildingTemperatureUpdate = 0f;
        private static readonly float timeToBuildingTemperatureUpdate = 2000f;

        public static void UpdateTemperature(GameTime gameTime)
        {
            //TileMap temperature in the current zone
            GameObjectManager.ZoneMap.UpdateTilesTemperatures(gameTime, GlobalTemperature.Value);

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

        #region Update Colonist Temperatures
        /// <summary>
        /// Update the temperatures for each colonist
        /// </summary>
        private static void UpdateColonistTemperatures()
        {
            //Adjust the temperatures of the colonists
            foreach (Colonist colonist in GameObjectManager.Filter<Colonist>())
            {
                float tileTemp = GameObjectManager.ZoneMap.GetTileAtPosition(colonist.Position).Temperature.Value;

                colonist.UpdateTemp(tileTemp);
                //Console.Out.WriteLine("Colonist temp: " + colonist.Temperature.Value + " tile temperature: " + tileTemp + " health: " + colonist.Health + " hunger: " + colonist.Hunger);
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
