using GlobalWarmingGame.ResourceItems;

namespace GlobalWarmingGame.Resources
{
    public interface IStorage
    {
        Inventory Inventory { get; }
    }
}
