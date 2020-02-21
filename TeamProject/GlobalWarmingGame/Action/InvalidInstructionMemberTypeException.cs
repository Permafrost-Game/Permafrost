using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Action
{

    class InvalidInstructionMemberTypeException : Exception
    {
        private readonly Instruction instruction;
        private readonly GameObject member;
        public InvalidInstructionMemberTypeException(Instruction instruction, GameObject member, Type type) : base("Unexpected type in instruction: \'" + instruction.ToString() + "\' expected member \'" + member.ToString() + "\' to be of type \'" + type + "\'")
        {
            this.instruction = instruction;
            this.member = member;
        }
    }

}
