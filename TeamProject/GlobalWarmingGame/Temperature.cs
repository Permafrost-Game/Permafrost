using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Microsoft.Xna.Framework;

namespace GlobalWarmingGame
{
    /// <summary>
    /// Wrapper class that puts limits on the temperature
    /// </summary>
    class Temperature
    {
        private int value;
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value < -100)
                {
                    this.value = -100;
                }
                else if (value > 100)
                {
                    this.value = 100;
                }
                else
                {
                    this.value = value;
                }
            }
        }
    }
}
