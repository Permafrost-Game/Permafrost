using System.Collections.Generic;

namespace GlobalWarmingGame.ResourceItems
{
    public class Inventory
    {
        Dictionary<string, ResourceItem> ResourceElements { get; set; }

        public Inventory()
        {
            this.ResourceElements = new Dictionary<string, ResourceItem>();
        }

        /// <summary>
        /// Adds ResourceItems to the Inventory
        /// </summary>
        /// <param name="">Item to be added</param>
        public void AddItem(ResourceItem item)
        {
            if(ResourceElements.ContainsKey(item.Type.ID))
            {
                ResourceElements[item.Type.ID].Ammount += item.Ammount;
            } else
            {
                ResourceElements.Add(item.Type.ID, item);
            }
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
                ResourceElements[item.Type.ID].Ammount -= item.Ammount;
                if (ResourceElements[item.Type.ID].Ammount == 0)
                {
                    ResourceElements.Remove(item.Type.ID);
                }
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the inventory contains atleast the specified ResourceItem
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool CheckContains(ResourceItem item)
        {
            return (ResourceElements.ContainsKey(item.Type.ID) &&
                    ResourceElements[item.Type.ID].Ammount >= item.Ammount);
        }

    }
}
