﻿using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    interface IHeatable
    {
        Temperature Temperature { get; set; }
        bool Heating { get; }
        Vector2 Position { get; }
        Vector2 Size { get; }
    }
}
