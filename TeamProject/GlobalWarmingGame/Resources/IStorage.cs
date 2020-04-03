using GlobalWarmingGame.ResourceItems;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Resources
{
    public interface IStorage
    {

        event EventHandler<ResourceItem> InventoryChange;

        Inventory Inventory { get; }
        Dictionary<Resource,int> InventoryRules { get; }
    }
}
