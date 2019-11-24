
using Microsoft.Xna.Framework;
using System;

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
            get => value;
            set => SetTemp(value);
        }

        public Temperature(double t) {
            SetTemp(t);
        }

        public void SetTemp(double temp)
        {
            value = MathHelper.Clamp((float)temp, -100, 100);

            if (("" + value).Contains("."))
            {
                String sBefore = ("" + value).Split('.')[0];
                String sAfter = ("" + value).Split('.')[1];

                if (sAfter.Length > 2) {
                    if (sAfter.ToCharArray()[0] != '5')
                    {
                        value = Math.Round(value);
                    }
                    else {
                        value = double.Parse(sBefore) + 0.5;
                    }
                }
            }
        }

    }
}
