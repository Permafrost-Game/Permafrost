using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Action
{
    class InvalidInstruction : Exception
    {
        public readonly Instruction instruction;
        public InvalidInstruction(Instruction instruction, string message) : base($"Instruction {instruction.ToString()} can not start because it is not currently valid\n{message}" )
        {
            this.instruction = instruction;
        }
    }
}
