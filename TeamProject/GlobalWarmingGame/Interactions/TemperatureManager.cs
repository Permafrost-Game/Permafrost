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
    public static class TemperatureManager
    {
        private static float timeToTemperatureUpdate = 0f;
        private static readonly float timeUntilTemperatureUpdate = 2000f;

        private static float timeUntillBuildingTempCheck = 0f;
        private static readonly float timeToBuildingTempCheck = 2000f;

        public static void UpdateTemperature(GameTime gameTime)
        {
            //TileMap temperature in the current zone
            GameObjectManager.ZoneMap.UpdateTilesTemperatures(gameTime);

            //Colonist Temperature
            timeToTemperatureUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeToTemperatureUpdate < 0f)
            {
                UpdateColonistTemperatures(gameTime);
                timeToTemperatureUpdate = timeUntilTemperatureUpdate;
            }

            //Building Temperature
            timeUntillBuildingTempCheck -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeUntillBuildingTempCheck < 0)
            {
                UpdateBuildingTemperatures(gameTime);
                timeUntillBuildingTempCheck = timeToBuildingTempCheck;
            }
        }

        #region Update Colonist Temperatures
        private static void UpdateColonistTemperatures(GameTime gameTime)
        {
            //Adjust the temperatures of the colonists
            foreach (Colonist colonist in GameObjectManager.GetObjectsByTag("Colonist"))
            {
                float tileTemp = GameObjectManager.ZoneMap.GetTileAtPosition(colonist.Position).temperature.Value;

                colonist.UpdateTemp(tileTemp);
                //Console.Out.WriteLine(colonist.Temperature.Value + " " + colonist.Health);
            }
        }
        #endregion

        #region Update Building Temperatures
        private static void UpdateBuildingTemperatures(GameTime gameTime)
        {
            foreach (IHeatable heatable in GameObjectManager.Filter<IHeatable>())
            {
                float temperature = heatable.Temperature.Value;
                Vector2 position = ((GameObject)heatable).Position;
                Vector2 size = ((GameObject)heatable).Size;
                bool heating = heatable.Heating;
                GetBuildingArea(position, size, temperature, heating, GameObjectManager.ZoneMap);
            }
        }

        public static void GetBuildingArea(Vector2 position, Vector2 size, float temperature, bool heating, TileMap tileMap)
        {
            float tileWidth = tileMap.Tiles[0, 0].Size.X;
            int numberOfTilesX = (int)(size.X / tileWidth);
            int numberOfTilesY = (int)(size.Y / tileWidth);
            for (int y = 0; y < numberOfTilesX; y++)
            {
                for (int x = 0; x < numberOfTilesY; x++)
                {
                    Tile t = tileMap.Tiles[(int)(position.X / tileWidth + x), (int)(position.Y / tileWidth + y)];
                    t.Heated = heating;
                    if (heating)
                    {
                        t.temperature.SetTemp(temperature);
                    }
                }
            }
        }
        #endregion
    }
}
