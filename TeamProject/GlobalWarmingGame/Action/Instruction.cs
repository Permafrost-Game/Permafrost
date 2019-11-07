using Engine;
using GlobalWarmingGame.Interactions;

namespace GlobalWarmingGame.Action
{
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
