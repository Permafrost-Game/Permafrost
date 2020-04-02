using GlobalWarmingGame.ResourceItems;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public interface IBuildable
    {
        List<ResourceItem> CraftingCosts { get; }

        void Build();
    }
}
