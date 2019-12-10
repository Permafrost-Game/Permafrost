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
    public class Cloth : ResourceType, ICraftable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Fibers(), 4) };

        public Cloth() : base("cloth", "Cloth", "A piece of cloth", 1f/*, texture*/)
        {


        }
    }
}
