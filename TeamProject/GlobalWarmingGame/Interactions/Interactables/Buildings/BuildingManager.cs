using GlobalWarmingGame.Action;
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
        static List<Building> buildings;

        static BuildingManager() 
        {
            buildings = new List<Building>(20);
        }

        public static void AddBuilding(int id, string stringID, Texture2D texture) 
        {
            InstructionType instructionType = new InstructionType(stringID, "Build", "Build " + stringID, Build);
            Building b = new Building(id+1, texture, instructionType);
            buildings.Add(b);
        }

        public static Building GetTextureByID(int id) 
        {
            Building building = null;
            foreach (Building b in buildings) 
            {
                if (b.ID == id) 
                {
                    building = b;
                }
            }
            return building;
        }

        private static void Build(Colonist colonist) 
        {                    
        }
    }
}
