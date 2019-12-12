﻿using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources.ResourceTypes;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Resources.Craftables
{
    public class Axe : ResourceType, ICraftable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Wood(), 1), new ResourceItem(new Fibers(), 2),
                                                                                                   new ResourceItem(new Stone(), 1) };

        public Axe() : base("axe", "Axe", "An Axe", 5f/*, texture*/)
        {


        }
    }
}