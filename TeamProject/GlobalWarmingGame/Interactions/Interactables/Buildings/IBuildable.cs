using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
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
        Temperature Temperature { get; set; }
        Vector2 Position { get; }
        Vector2 Size { get; }
    }
}
