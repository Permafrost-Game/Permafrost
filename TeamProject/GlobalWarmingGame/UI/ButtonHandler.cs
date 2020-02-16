using GlobalWarmingGame.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.UI
{
    class ButtonHandler<T>
    {
        public T instruction;

        public delegate void Action(T instruction);

        public Action action;

        public ButtonHandler(T instruction, Action action)
        {
            this.instruction = instruction;
            this.action = action;
        }

    }
}
