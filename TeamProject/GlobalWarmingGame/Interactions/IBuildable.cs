using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions
{
    interface IBuildable
    { 
        Temperature Temperature { get; }
        Vector2 Size { get; }
        Vector2 Position { get; }
    }
}
