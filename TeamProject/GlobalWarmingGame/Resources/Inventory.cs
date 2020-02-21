using Engine;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.ResourceItems
{
    public class Inventory
    {
        public event EventHandler InventoryChange = delegate { };

        public Dictionary<ResourceType, ResourceItem> Resources { get; private set; }
        public float Capacity { get; private set; }
        public float CurrentLoad { get; private set; }
        public bool IsFull { get => Capacity < CurrentLoad;  }

        public Inventory(float capacity)
        {
            Resources = new Dictionary<ResourceType, ResourceItem>();
            Capacity = capacity;
            CurrentLoad = 0f;
        }


        /// <summary>
        /// Adds a copy of <paramref name="item"/> to the Inventory if space is available
        /// </summary>
        /// <param name="item"></param>
        /// <returns>whether the item can be added</returns>
        public bool AddItem(ResourceItem item)
        {
            if (CurrentLoad + item.Weight < Capacity)
            {
                AddItemUnchecked(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds <paramref name="item"/> to <see cref="Inventory.Resources"/> without checking load
        /// </summary>
        /// <param name="item">item to be added</param>
        private void AddItemUnchecked(ResourceItem item)
        {
            
            if (Resources.ContainsKey(item.ResourceType))
            {
                Resources[item.ResourceType].Weight += item.Weight;
                
            }
            else
            {
                Resources.Add(item.ResourceType, item.Clone());
            }
                
            CurrentLoad += item.Weight;
            InventoryChange.Invoke(this, new EventArgs());
        }


        private void RemoveItemUnchecked(ResourceItem item)
        { 
            Resources[item.ResourceType].Weight -= item.Weight;

            if(Resources[item.ResourceType].Weight <= 0)
                Resources.Remove(item.ResourceType);

            CurrentLoad -= item.Weight;
            InventoryChange.Invoke(this, new EventArgs());
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
                RemoveItemUnchecked(item);
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
            return Resources.ContainsKey(resourceItem.ResourceType) ?
                Resources[resourceItem.ResourceType].Weight >= resourceItem.Weight : false;
        }

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
    }
}
