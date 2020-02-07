using Engine;
using GlobalWarmingGame.Interactions;

namespace GlobalWarmingGame.Action
{
    /// <summary>
    /// An Instruction descrives a instance interaction between two interactable objects.
    /// </summary>
    class Instruction
    {
        public InstructionType Type { get; set; }
        public IInstructionFollower ActiveMember { get; set; }
        public GameObject PassiveMember { get; set; }

        public Instruction(IInstructionFollower activeMember)
        {
            ActiveMember = activeMember;
        }
        public Instruction() { }

    }
}
