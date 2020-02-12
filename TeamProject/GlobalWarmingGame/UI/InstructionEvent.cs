using GlobalWarmingGame.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.UI
{
    class InstructionHandler
    {
        public Instruction instruction;

        public delegate void Action(Instruction instruction);

        public Action action;

        public InstructionHandler(Instruction instruction, Action action)
        {
            this.instruction = instruction;
            this.action = action;
        }

    }
}
