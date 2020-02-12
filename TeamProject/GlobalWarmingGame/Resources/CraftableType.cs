using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Resources
{
    public class CraftableType : ResourceType, ICraftable
    {
        public List<ResourceItem> CraftingCosts { get; set; }
        public CraftableType(string id, string displayName, string description, float weight, Texture2D texture) : base(id, displayName, description, weight, texture)
        {

        }
    }
}
