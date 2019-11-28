﻿using Engine;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Resources
{
    static class CollectiveInventory
    {
        static List<GameObject> Colonists { get; set; }
        static List<Inventory> ColonistInventories { get; set; }

        public static float CollectiveCapacity { get; set; }
        public static float CollectiveCurrentLoad { get; set; }

        static CollectiveInventory()
        {
            Colonists = new List<GameObject>();
            ColonistInventories = new List<Inventory>();

            BuildCollectiveInventory();
        }

        public static void BuildCollectiveInventory()
        {
            CollectiveCapacity = 0f;
            CollectiveCurrentLoad = 0f;

            Colonists = GameObjectManager.GetObjectsByTag("Colonist");

            foreach (Colonist colonist in Colonists)
                ColonistInventories.Add(colonist.Inventory);

            foreach (Inventory inventory in ColonistInventories)
            {
                CollectiveCapacity += inventory.Capacity;
                CollectiveCurrentLoad += inventory.CurrentLoad;
            }
        }

        public static void UpdateCollectiveInventory()
        {
            Colonists.Clear();
            ColonistInventories.Clear();

            BuildCollectiveInventory();
        }
    }
}