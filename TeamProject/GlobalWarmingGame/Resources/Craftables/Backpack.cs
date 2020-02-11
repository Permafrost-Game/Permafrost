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
    public class Backpack : ResourceType, ICraftable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Cloth(), 2), new ResourceItem(new Leather(), 5)};

        public Backpack() : base("backpack", "Backpack", "A backpack", 2f/*, texture*/)
        {


        }
    }
}
