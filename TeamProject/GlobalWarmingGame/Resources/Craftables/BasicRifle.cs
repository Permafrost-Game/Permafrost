using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using GlobalWarmingGame.Resources.ResourceTypes;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Resources.Craftables
{
    public class BasicRifle : ResourceType, ICraftable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Wood(), 8), new ResourceItem(new Fibers(), 2),
                                                                                                   new ResourceItem(new MachineParts(), 4) };

        public BasicRifle() : base("basicRifle", "BasicRifle", "A basic rifle", 10f/*, texture*/)
        {


        }
    }
}
