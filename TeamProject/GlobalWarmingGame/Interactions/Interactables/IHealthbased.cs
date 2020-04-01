using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables
{
    interface IHealthbased
    {
        float MaxHealth { get; }
        float Health { get; set; }
    }
}
