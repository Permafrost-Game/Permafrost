using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    interface IBuildable
    {
        List<ResourceItem> CraftingCosts { get; }


    }
}
