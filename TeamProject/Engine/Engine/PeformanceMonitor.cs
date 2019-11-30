using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class PeformanceMonitor : IUpdatable
    {
        private static float frameTime;

        public PeformanceMonitor()
        {

        }

        public string GetPrintString()
        {
            string str = String.Empty;

            str += $"{frameTime}ms\n";
            float frameRate = 1 / (float)frameTime;
            str += $"{frameRate}fps\n";

            return str;
        }

        public void Update(GameTime gameTime)
        {
            frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
