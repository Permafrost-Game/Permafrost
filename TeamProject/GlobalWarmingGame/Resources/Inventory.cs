using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.ResourceItems
{
    public class Inventory : IUpdatable
    {
        Dictionary<string, ResourceItem> Resources { get; set; }
        List<Colonist> colonists;

        public float Capacity { get; set; } //TODO - Set as total of colonist inventory capacity [Harcode it ?]
        public float CurrentLoad { get; private set; }

        bool isFull;

        public Inventory()
        {
            this.Resources = new Dictionary<string, ResourceItem>();
            colonists = GameObjectManager.GetObjectsByTag("Colonist");
            CurrentLoad = 0f;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Colonist colonist in colonists)
            {
                ;
            }
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

            if (!isFull)
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
        bool CheckWeightLimit()
        {
            if (CurrentLoad < Capacity)
                isFull = false;
            else
                isFull = true;
            
            return isFull;
        }

        /// <summary>
        /// Returns the total weight of all items in the inventory
        /// </summary>
        /// <returns></returns>
        float CheckInventoryWeight()
        {
            foreach (var resource in Resources)
                CurrentLoad += resource.Value.Type.Weight;

            return CurrentLoad;
        }
    }
}
