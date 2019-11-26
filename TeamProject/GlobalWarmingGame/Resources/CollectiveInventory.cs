using Engine;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.ResourceItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Resources
{
    class CollectiveInventory
    {
        List<GameObject> colonists;
        Colonist colonist;

        public List<Inventory> CollectiveInvetory { get; set; }
        
        public float CollectiveCapacity { get; set; }
        public float CollectiveCurrentLoad { get; set; }
        public bool IsFull { get; set; }

        public CollectiveInventory()
        {
            colonists = GameObjectManager.GetObjectsByTag("Colonist");

            foreach (GameObject go in colonists)
            {
                colonist = (Colonist)go;
                CollectiveInvetory.Add(colonist.Inventory);
            }

            foreach (Inventory inv in CollectiveInvetory)
            {
                CollectiveCapacity += inv.Capacity;
                CollectiveCurrentLoad += inv.CurrentLoad;
            }

            IsFull = false;
        }
    }
}
