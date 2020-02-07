using Engine;
using GlobalWarmingGame.Interactions;
using System;

namespace GlobalWarmingGame.Action
{
    /// <summary>
    /// An Instruction descrives a instance interaction between two interactable objects.
    /// </summary>
    public class Instruction
    {
        public InstructionType Type { get; set; }
        public IInstructionFollower ActiveMember { get; set; }
        public GameObject PassiveMember { get; set; }

        [Obsolete]
        public Instruction(IInstructionFollower activeMember)
        { //TODO DELETE ME
            ActiveMember = activeMember;
        }
        public Instruction() { }

        public Instruction(InstructionType type, IInstructionFollower activeMember, GameObject passiveMember)
        {
            Type = type;
            ActiveMember = activeMember;
            PassiveMember = passiveMember;
        }

    }
}
