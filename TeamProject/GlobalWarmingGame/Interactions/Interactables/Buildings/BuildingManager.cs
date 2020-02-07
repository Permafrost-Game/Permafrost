using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    static class BuildingManager
    {
        private static float timeUntillBuildingTempCheck;
        private static float timeToBuildingTempCheck = 2000f;

        public static List<Building> Buildings { get; private set; }

        static BuildingManager() 
        {
            Buildings = new List<Building>(20);
            timeUntillBuildingTempCheck = timeToBuildingTempCheck;
        }

        public static void AddBuilding(int id, string stringID, Texture2D texture = null) 
        {
            InstructionType instructionType = new InstructionType(stringID, "Build", "Build " + stringID, Build);
            Building b = new Building(id, stringID, instructionType, texture);
            Buildings.Add(b);
        }

        public static Building GetBuilding(int id) 
        {
            Building building = null;
            foreach (Building b in Buildings) 
                if (b.ID == id)
                    building = b;
 
            return building;
        }

        public static string[] GetBuildingStrings()
        {
            string[] buildings = new string[Buildings.Count];

            for (int i = 0; i < buildings.Length; i++)
                buildings[i] = GetBuilding(i).StringID;

            return buildings; 
        }

        private static void Build(IInstructionFollower follower) 
        {                    
        }

        #region Update Building Temperatures
        public static void UpdateBuildingTemperatures(GameTime gameTime, TileMap tileMap)
        {
            timeUntillBuildingTempCheck -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeUntillBuildingTempCheck < 0) 
            {
                foreach (IHeatable heatable in GameObjectManager.Filter<IHeatable>())
                {
                    float temperature = heatable.Temperature.Value;
                    Vector2 position = ((GameObject)heatable).Position;
                    Vector2 size = ((GameObject)heatable).Size;
                    bool heating = heatable.Heating;
                    GetBuildingArea(position, size, temperature, heating, tileMap);
                }
                timeUntillBuildingTempCheck = timeToBuildingTempCheck;
            }
        }

        public static void GetBuildingArea(Vector2 position, Vector2 size, float temperature, bool heating, TileMap tileMap)
        {
            float tileWidth = tileMap.Tiles[0, 0].size.X;
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
