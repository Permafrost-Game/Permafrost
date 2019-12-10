using Engine;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Menus;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Resources
{
    class CollectiveInventory
    {
        List<GameObject> Colonists { get; set; }
        public List<Inventory> ColonistInventories { get; set; }
        public Dictionary<string, ResourceItem> CollectiveResources { get; set; }
        public float CollectiveCapacity { get; set; }
        public float CollectiveCurrentLoad { get; set; }
        public int TotalFood { get; set; }

        readonly float timeBetweenUpdate = 500f;
        float timeUntilUpdate;
        
        public CollectiveInventory(MainUI mainUI)
        {
            Colonists = new List<GameObject>();
            ColonistInventories = new List<Inventory>();
            CollectiveResources = new Dictionary<string, ResourceItem>();

            BuildCollectiveInventory(mainUI);
        }

        public void UpdateCollectiveInventory(GameTime gameTime, MainUI mainUI)
        {
            timeUntilUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeUntilUpdate <= 0f)
            {
                Colonists.Clear();
                ColonistInventories.Clear();
                CollectiveResources.Clear();

                BuildCollectiveInventory(mainUI);

                timeUntilUpdate = timeBetweenUpdate;
            }
        }

        void BuildCollectiveInventory(MainUI mainUI)
        {
            TotalFood = 0;
            CollectiveCapacity = 0f;
            CollectiveCurrentLoad = 0f;

            Colonists = GameObjectManager.GetObjectsByTag("Colonist");

            foreach (Colonist colonist in Colonists)
                ColonistInventories.Add(colonist.Inventory);

            foreach (Inventory inventory in ColonistInventories)
            {
                CollectiveCapacity += inventory.Capacity;
                CollectiveCurrentLoad += inventory.CurrentLoad;
                
                foreach (ResourceItem item in inventory.Resources.Values)
                {
                    if (CollectiveResources.ContainsKey(item.Type.ID))
                        CollectiveResources[item.Type.ID].Amount += item.Amount;

                    else
                        CollectiveResources.Add(item.Type.ID, item);   
                }
            }

            foreach (ResourceItem item in CollectiveResources.Values)
            {
                switch (item.Type.ID)
                {
                    case "food":
                        mainUI.ItemSlots[0].IconType = IconType.Apple;
                        mainUI.ItemLabels[0].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;

                    case "wood":
                        mainUI.ItemSlots[1].IconType = IconType.Bone;
                        mainUI.ItemLabels[1].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;

                    case "stone":
                        mainUI.ItemSlots[2].IconType = IconType.Diamond;
                        mainUI.ItemLabels[2].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;
                }
            }
        }
    }
}
