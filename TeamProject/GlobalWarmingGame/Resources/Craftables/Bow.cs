using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Resources.Craftables
{
    public class Bow : ResourceType, ICraftable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Wood(), 4), new ResourceItem(new Fibers(), 6),
                                                                                                   new ResourceItem(new Stone(), 1) };

        public Bow() : base("bow", "bow", "A bow with arrows", 5f/*, texture*/)
        {


        }
    }
}
