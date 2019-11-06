using Engine;

namespace GlobalWarmingGame.Action
{
    class Instruction
    {
        public InstructionType Type { get; set; }
        public Colonist ActiveMember { get; set; }
        public Building PassiveMember { get; set; }

        public Instruction(InstructionType type, Colonist activeMember, Building passiveMember)
        {
            Type = type;
            ActiveMember = activeMember;
            PassiveMember = passiveMember;
        }
        public Instruction() { }

    }
}
