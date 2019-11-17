using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Microsoft.Xna.Framework;

namespace Engine
{
    /// <summary>
    /// Wrapper class that puts limits on the temperature
    /// </summary>
    public class Temperature
    {
        private double value;

        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                TempSetter(value);
            }
        }

        public Temperature(Double t) {
            TempSetter(t);
        }

        private void TempSetter(Double temp)
        {
            if (temp < -100)
            {
                value = -100;
            }
            else if (temp > 100)
            {
                value = 100;
            }
            else
            {
                value = temp;
            }
        }

    }
}
