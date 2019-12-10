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
    public class Hoe : ResourceType, ICraftable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Wood(), 1), new ResourceItem(new Fibers(), 2),
                                                                                                   new ResourceItem(new Stone(), 1) };

        public Hoe() : base("hoe", "Hoe", "A hoe", 5f/*, texture*/)
        {


        }
    }
}
