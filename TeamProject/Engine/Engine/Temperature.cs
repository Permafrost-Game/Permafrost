
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

        public float Max { get; private set; } = 100;
        public float Min { get; private set; } = -10;

        public float Value
        {
            get => value;
            set => SetTemp(value);
        }

        public Temperature(float t) 
        {
            SetTemp(t);
        }

        private void SetTemp(float temp)
        {
            value = MathHelper.Clamp(temp, Min, Max);
            value = (float)Math.Round(value, 2);
        }

    }
}
