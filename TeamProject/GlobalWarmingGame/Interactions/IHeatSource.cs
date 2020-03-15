using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions
{
    interface IHeatSource
    {
        Temperature Temperature { get; set; }
        bool Heating { get; }
    }
}
