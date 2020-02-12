using Engine;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.UI.Menus;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public int TotalWood { get; set; }
        public int TotalStone { get; set; }
        public int TotalFibers { get; set; }

        readonly float timeBetweenUpdate = 500f;
        float timeUntilUpdate;

        public CollectiveInventory(MainUI mainUI, Dictionary<String, Texture2D> icons)
        {
            Colonists = new List<GameObject>();
            ColonistInventories = new List<Inventory>();
            CollectiveResources = new Dictionary<string, ResourceItem>();

            BuildCollectiveInventory(mainUI, icons);
        }

        public void UpdateCollectiveInventory(GameTime gameTime, MainUI mainUI, Dictionary<String, Texture2D> icons)
        {
            timeUntilUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeUntilUpdate <= 0f)
            {
                Colonists.Clear();
                ColonistInventories.Clear();
                CollectiveResources.Clear();

                BuildCollectiveInventory(mainUI, icons);

                timeUntilUpdate = timeBetweenUpdate;
            }
        }

        void BuildCollectiveInventory(MainUI mainUI, Dictionary<String, Texture2D> icons)
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
                        CollectiveResources.Add(item.Type.ID, item.Clone()); 
                }
            }

            foreach (ResourceItem item in CollectiveResources.Values)
            {
                switch (item.Type.ID)
                {
                    case "food":
                        mainUI.ItemSlots[0].Texture = icons[item.Type.ID];
                        mainUI.ItemLabels[0].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;

                    case "wood":
                        mainUI.ItemSlots[1].Texture = icons[item.Type.ID];
                        mainUI.ItemLabels[1].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;

                    case "stone":
                        mainUI.ItemSlots[2].Texture = icons[item.Type.ID];
                        mainUI.ItemLabels[2].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;

                    case "fibers":
                        mainUI.ItemSlots[3].Texture = icons[item.Type.ID];
                        mainUI.ItemLabels[3].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;
                    case "axe":
                        mainUI.ItemSlots[4].Texture = icons[item.Type.ID];
                        mainUI.ItemLabels[4].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;
                    case "pickaxe":
                        mainUI.ItemSlots[5].Texture = icons[item.Type.ID];
                        mainUI.ItemLabels[5].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;
                    case "hoe":
                        mainUI.ItemSlots[6].Texture = icons[item.Type.ID];
                        mainUI.ItemLabels[6].Text = CollectiveResources[item.Type.ID].Amount.ToString();
                        break;
                }
            }

            if (CollectiveResources.ContainsKey("food"))
                TotalFood = CollectiveResources["food"].Amount;

            if (CollectiveResources.ContainsKey("wood"))
                TotalWood = CollectiveResources["wood"].Amount;

            if (CollectiveResources.ContainsKey("stone"))
                TotalStone = CollectiveResources["stone"].Amount;

            if (CollectiveResources.ContainsKey("fibers"))
                TotalFibers = CollectiveResources["fibers"].Amount;
        }
    }
}
