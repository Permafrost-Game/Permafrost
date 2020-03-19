using GlobalWarmingGame.ResourceItems;
using System;

namespace GlobalWarmingGame.Resources
{
    public interface IStorage
    {

        event EventHandler<ResourceItem> InventoryChange;

        Inventory Inventory { get; }
    }
}
