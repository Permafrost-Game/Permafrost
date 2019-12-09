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
        public static List<Building> Buildings { get; private set; }

        static BuildingManager() 
        {
            Buildings = new List<Building>(20);
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

        private static void Build(Colonist colonist) 
        {                    
        }
    }
}
