using GlobalWarmingGame.ResourceItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Resources
{
    interface ICraftable
    {
        List<ResourceItem> CraftingCosts { get; }
    }
}
