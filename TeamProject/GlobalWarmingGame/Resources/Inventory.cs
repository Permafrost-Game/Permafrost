using Engine;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.ResourceItems
{
    public class Inventory
    {
        public Dictionary<string, ResourceItem> Resources { get; set; }
        public float Capacity { get; set; }
        public float CurrentLoad { get; set; }
        public bool IsFull { get; set; }

        public Inventory(float capacity)
        {
            this.Resources = new Dictionary<string, ResourceItem>();
            Capacity = capacity;
            CurrentLoad = 0f;
            IsFull = false;
        }

        /// <summary>
        /// Adds ResourceItems to the Inventory while space is available
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(ResourceItem item)
        {
            item = item.Clone();

            CheckWeightLimit();

            if (!IsFull)
            {
                if (Resources.ContainsKey(item.Type.ID))
                    Resources[item.Type.ID].Amount += item.Amount;

                else
                    Resources.Add(item.Type.ID, item);

                CurrentLoad += item.Type.Weight * item.Amount;
            }

            return false;
        }

        /// <summary>
        /// Adds ResourceItems to the Inventory
        /// </summary>
        /// <param name="item">Item to be removed</param
        /// <returns>If the remove was sucsessful</returns>
        public bool RemoveItem(ResourceItem item)
        {
            if (CheckContains(item))
            {
                if (Resources[item.Type.ID].Amount > item.Amount)
                {
                    Resources[item.Type.ID].Amount -= item.Amount;
                    CurrentLoad -= item.Type.Weight * item.Amount;
                }
                else if (Resources[item.Type.ID].Amount == item.Amount)
                {
                    CurrentLoad -= item.Type.Weight * Resources[item.Type.ID].Amount;
                    Resources.Remove(item.Type.ID);
                }
                else
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the inventory contains atleast the specified ResourceItem
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckContains(ResourceItem item)
        {
            return (Resources.ContainsKey(item.Type.ID));
        }

        /// <summary>
        /// Returns true if the inventory contains all the specified ResourceItems in a list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckContainsList(List<ResourceItem> items)
        {
            foreach (ResourceItem item in items)
            {
                if (!CheckContains(item))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if the inventory has reacher maximum capicity and sets isFull accordingly
        /// </summary>
        /// <returns></returns>
        public bool CheckWeightLimit()
        {
            if (CurrentLoad < Capacity)
                IsFull = false;
            else
                IsFull = true;

            return IsFull;
        }
    }
}
