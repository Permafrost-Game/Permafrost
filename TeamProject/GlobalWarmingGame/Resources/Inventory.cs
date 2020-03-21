using Engine;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.ResourceItems
{
    public class Inventory : IReconstructable
    {
        public event EventHandler<ResourceItem> InventoryChange = delegate { };

        public Dictionary<Resource, ResourceItem> Resources { get; private set; }

        [PFSerializable]
        public float Capacity { get; private set; }

        public float CurrentLoad { get; private set; }
        public bool IsFull { get => Capacity < CurrentLoad;  }

        [PFSerializable]
        public readonly IEnumerable<ResourceItem> resourceItems;

        public Inventory()
        {

        }

        public Inventory(float capacity)
        {
            Resources = new Dictionary<Resource, ResourceItem>();
            Capacity = capacity;
            CurrentLoad = 0f;

            resourceItems = Resources.Values;
        }


        /// <summary>
        /// Adds a copy of <paramref name="item"/> to the Inventory if space is available
        /// </summary>
        /// <param name="item"></param>
        /// <returns>whether the item can be added</returns>
        public bool AddItem(ResourceItem item)
        {
            if (CanAddItem(item))
            {
                AddItemUnchecked(item);
                return true;
            }

            return false;
        }

        public bool CanAddItem(ResourceItem item) => CurrentLoad + item.Weight < Capacity;

        /// <summary>
        /// Adds <paramref name="item"/> to <see cref="Inventory.Resources"/> without checking load
        /// </summary>
        /// <param name="item">item to be added</param>
        private void AddItemUnchecked(ResourceItem item)
        {
            
            if (Resources.ContainsKey(item.ResourceType.ResourceID))
            {
                Resources[item.ResourceType.ResourceID].Weight += item.Weight;
                if (Resources[item.ResourceType.ResourceID].Weight <= 0)
                {
                    Resources.Remove(item.ResourceType.ResourceID);
                }
            }
            else if(item.Weight > 0)
            {
                Resources.Add(item.ResourceType.ResourceID, item.Clone());
            }
            
                
            CurrentLoad += item.Weight;
            InventoryChange.Invoke(this, item);
        }


        /// <summary>
        /// Removes <paramref name="item"/> from the <see cref="Inventory"/>
        /// </summary>
        /// <param name="item"><see cref="ResourceItem"/> to be removed</param>
        /// <returns>whether the <paramref name="item"/> can be removed</returns>
        public bool RemoveItem(ResourceItem item)
        {
            if (Contains(item))
            {
                ResourceItem invertedItem = item.Clone();
                invertedItem.Weight = -item.Weight;
                AddItemUnchecked(invertedItem);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the inventory contains a <paramref name="resourceItem"/>.
        /// </summary>
        /// <param name="resourceItem">the <see cref="ResourceItem"/> to be checked</param>
        /// <returns><code>true</code> if the <see cref="Inventory"/> contains the <paramref name="resourceItem"/></returns>
        public bool Contains(ResourceItem resourceItem)
        {
            return Resources.ContainsKey(resourceItem.ResourceType.ResourceID) ?
                Resources[resourceItem.ResourceType.ResourceID].Weight >= resourceItem.Weight : false;
        }

        public bool ContainsType(Resource resourceID) => Resources.ContainsKey(resourceID);

        /// <summary>
        /// Checks if all of <paramref name="resourceItem"/> are in the inventory.
        /// </summary>
        /// <param name="resourceItem">the <see cref="ResourceItem"/>s to be checked</param>
        /// <returns>true if all <see cref="ResourceItem"/>s in <paramref name="resourceItem"/> are in the <see cref="Inventory"/></returns>
        public bool ContainsAll(IEnumerable<ResourceItem> resourceItem)
        {
            foreach (ResourceItem item in resourceItem)
            {
                if (!Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public object Reconstruct()
        {
            Inventory inventory = new Inventory(Capacity);

            foreach (ResourceItem item in resourceItems)
                inventory.AddItemUnchecked(item);

            return inventory;
        }
    }
}
