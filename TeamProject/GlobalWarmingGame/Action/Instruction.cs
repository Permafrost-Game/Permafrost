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
        public Colonist ActiveMember { get; set; }
        public IInteractable PassiveMember { get; set; }

        public Instruction(Colonist activeMember)
        {
            ActiveMember = activeMember;
        }
        public Instruction() { }

    }
}
