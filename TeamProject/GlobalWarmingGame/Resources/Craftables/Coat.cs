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
    public class Coat : ResourceType, ICraftable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Cloth(), 4), new ResourceItem(new Leather(), 2) };

        public Coat() : base("coat", "Coat", "A basic coat", 5f/*, texture*/)
        {


        }
    }
}
