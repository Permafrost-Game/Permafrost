
namespace Engine
{
    /// <summary>
    /// Wrapper class that puts limits on the temperature
    /// </summary>
    public class Temperature
    {
        private int value;

        public int Value
        {
            get => value;
            set => SetTemp(value);
        }

        public Temperature(int t) {
            SetTemp(t);
        }

        public void SetTemp(int temp)
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
