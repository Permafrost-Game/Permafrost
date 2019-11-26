using Engine;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.ResourceItems
{
    public class Inventory
    {
        Dictionary<string, ResourceItem> Resources { get; set; }

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
        /// Adds ResourceItems to the Iventory while space is available
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddItem(ResourceItem item)
        {
            CheckInventoryWeight();
            CheckWeightLimit();

            if (!IsFull)
            {
                if (Resources.ContainsKey(item.Type.ID))
                    Resources[item.Type.ID].Amount += item.Amount;

                else
                    Resources.Add(item.Type.ID, item);
            }

            return false;
        }

        /// <summary>
        /// Adds ResourceItems to the Inventory
        /// </summary>
        /// <param name="item">Item to be removed</param
        /// <returns>If the remove was sucsessful</returns>
        public bool RemoveItems(ResourceItem item)
        {
            if(CheckContains(item))
            {
                Resources[item.Type.ID].Amount -= item.Amount;
                if (Resources[item.Type.ID].Amount == 0) 
                    Resources.Remove(item.Type.ID);

                return true;
            } 
            
            else
                return false;
        }

        /// <summary>
        /// Returns true if the inventory contains atleast the specified ResourceItem
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckContains(ResourceItem item)
        {
            return (Resources.ContainsKey(item.Type.ID) &&
                    Resources[item.Type.ID].Amount >= item.Amount);
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

        /// <summary>
        /// Returns the total weight of all items in the inventory
        /// </summary>
        /// <returns></returns>
        public float CheckInventoryWeight()
        {
            foreach (var resource in Resources)
                CurrentLoad += resource.Value.Type.Weight;

            return CurrentLoad;
        }
    }
}
