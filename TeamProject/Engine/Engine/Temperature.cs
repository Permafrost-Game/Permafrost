
using Microsoft.Xna.Framework;
using System;

namespace Engine
{
    /// <summary>
    /// Wrapper class that puts limits on the temperature
    /// </summary>
    public class Temperature
    {
        private float value;

        public float Value
        {
            get => value;
            set => SetTemp(value);
        }

        public Temperature(float t) {
            SetTemp(t);
        }

        public void SetTemp(float temp)
        {
            value = MathHelper.Clamp(temp, -100, 100);
            value = (float)Math.Round(value, 2);
        }

    }
}
