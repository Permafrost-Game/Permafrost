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
    public class Pickaxe : ResourceType, ICraftable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Wood(), 1), new ResourceItem(new Fibers(), 2),
                                                                                                   new ResourceItem(new Stone(), 2) };

        public Pickaxe() : base("pickaxe", "Pickaxe", "A Pickaxe", 10f/*, texture*/)
        {


        }
    }
}
