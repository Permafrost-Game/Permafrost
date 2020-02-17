using Engine;
using GlobalWarmingGame.Interactions;
using System;

namespace GlobalWarmingGame.Action
{
    /// <summary>
    /// An Instruction descrives a instance interaction between two interactable objects.
    /// </summary>
    public class Instruction : IComparable<Instruction>
    {
        public InstructionType Type { get; set; }
        public IInstructionFollower ActiveMember { get; set; }
        public GameObject PassiveMember { get; set; }
        public int Priority { get; }

        [Obsolete]
        public Instruction(IInstructionFollower activeMember)
        { //TODO DELETE ME
            ActiveMember = activeMember;
        }
        public Instruction() { }

        /// <summary>
        /// Creates a new Instruction with an overridden priority
        /// </summary>
        /// <param name="type">the <see cref="InstructionType"/> of the instruction</param>
        /// <param name="activeMember">the active member of the instruction, the <see cref="IInstructionFollower"/></param>
        /// <param name="passiveMember">the target member of the instruction</param>
        /// <param name="priorityOverride">the priority of the instruction, overriding the priority from the <see cref="InstructionType"/></param>
        public Instruction(InstructionType type, IInstructionFollower activeMember, GameObject passiveMember, int priorityOverride)
        {
            Type = type;
            ActiveMember = activeMember;
            PassiveMember = passiveMember;
            Priority = priorityOverride;
        }

        /// <summary>
        /// Creates a new Instruction<br/>
        /// The priority of this instruction is gained from the <paramref name="type"/>
        /// </summary>
        /// <param name="type">the <see cref="InstructionType"/> of the instruction</param>
        /// <param name="activeMember">the active member of the instruction, the <see cref="IInstructionFollower"/></param>
        /// <param name="passiveMember">the target member of the instruction</param>
        public Instruction(InstructionType type, IInstructionFollower activeMember, GameObject passiveMember) : this (type, activeMember, passiveMember, type.Priority) { }

        public int CompareTo(Instruction obj)
        {
            return this.Priority.CompareTo(obj.Priority);
        }

        public override string ToString()
        {
            return Type.Name;
        }

    }
}
