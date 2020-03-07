using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions
{
    /// <summary>
    /// TemperatureManager handles updating the temperatures of colonists and buildings
    /// based on the tile they are above and tiles around them respectively.
    /// </summary>
    public static class TemperatureManager
    {
        public static Temperature GlobalTemperature { get; set; } = new Temperature(-5);

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
            foreach (Colonist colonist in GameObjectManager.GetObjectsByTag("Colonist"))
            {
                float tileTemp = GameObjectManager.ZoneMap.GetTileAtPosition(colonist.Position).temperature.Value;

                colonist.UpdateTemp(tileTemp);
                Console.Out.WriteLine("Colonist temp: " + colonist.Temperature.Value + " health: " + colonist.Health + " hunger: " + colonist.Hunger);
            }
        }
        #endregion

        #region Update Building Temperatures
        /// <summary>
        /// Update the temperature of all buildings that produce heat
        /// </summary>
        private static void UpdateBuildingTemperatures()
        {
            foreach (IHeatable heatable in GameObjectManager.Filter<IHeatable>())
            {
                float temperature = heatable.Temperature.Value;
                Vector2 position = ((GameObject)heatable).Position;
                Vector2 size = ((GameObject)heatable).Size;
                bool heating = heatable.Heating; //Is the building currently producing heat
                HeatBuildingArea(position, size, temperature, heating, GameObjectManager.ZoneMap);
            }
        }

        /// <summary>
        /// Calculate the tile area the building takes up and heat the tiles.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="temperature"></param>
        /// <param name="heating"></param>
        /// <param name="tileMap"></param>
        public static void HeatBuildingArea(Vector2 position, Vector2 size, float temperature, bool heating, TileMap tileMap)
        {
            float tileWidth = tileMap.Tiles[0, 0].Size.X;
            int numberOfTilesX = (int)(size.X / tileWidth);
            int numberOfTilesY = (int)(size.Y / tileWidth);
            for (int y = 0; y < numberOfTilesY; y++)
            {
                for (int x = 0; x < numberOfTilesX; x++)
                {
                    Tile t = tileMap.GetTileAtPosition(new Vector2(position.X + x * tileWidth, position.Y - y * tileWidth));
                    if (heating)
                    {
                        t.Heated = heating;
                        t.temperature.SetTemp(temperature);
                    }
                }
            }
        }
        #endregion
    }
}
