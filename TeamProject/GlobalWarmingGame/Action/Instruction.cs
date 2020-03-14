using Engine;
using GlobalWarmingGame.Interactions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Action
{
    /// <summary>
    /// An Instruction descrives a instance interaction between two interactable objects.
    /// </summary>
    public class Instruction : IComparable<Instruction>, Engine.IUpdatable
    {
        public InstructionType Type { get; set; }
        public IInstructionFollower ActiveMember { get; set; }
        public GameObject PassiveMember { get; set; }
        public int Priority { get; }
        public bool IsStarted { get; private set; }
        public bool IsComplete { get; private set; }

        public readonly List<InstructionEvent> OnStart;
        public readonly List<InstructionEvent> OnComplete;

        public bool IsValid
        {
            get { return Type.checkValidity != null ? Type.checkValidity(this) : true; }
        }

        private float timeSpent;

        /// <summary>
        /// Creates a new Instruction with an overridden priority
        /// </summary>
        /// <param name="type">the <see cref="InstructionType"/> of the instruction</param>
        /// <param name="activeMember">the active member of the instruction, the <see cref="IInstructionFollower"/></param>
        /// <param name="passiveMember">the target member of the instruction</param>
        /// <param name="priorityOverride">the priority of the instruction, overriding the priority from the <see cref="InstructionType"/></param>
        public Instruction(InstructionType type, int priorityOverride, IInstructionFollower activeMember = null, GameObject passiveMember = null)
        {
            Type = type;
            ActiveMember = activeMember;
            PassiveMember = passiveMember;
            Priority = priorityOverride;
            IsStarted = false;

            OnStart = new List<InstructionEvent>();
            if (type.onStart != null)
                OnStart.Add(type.onStart);

            OnComplete = new List<InstructionEvent>();
            if (type.onComplete != null)
                OnComplete.Add(type.onComplete);

        }

        /// <summary>
        /// Creates a new Instruction<br/>
        /// The priority of this instruction is gained from the <paramref name="type"/>
        /// </summary>
        /// <param name="type">the <see cref="InstructionType"/> of the instruction</param>
        /// <param name="activeMember">the active member of the instruction, the <see cref="IInstructionFollower"/></param>
        /// <param name="passiveMember">the target member of the instruction</param>
        public Instruction(InstructionType type, IInstructionFollower activeMember = null, GameObject passiveMember = null) : this (type, type.Priority, activeMember, passiveMember) { }

        /// <summary>
        /// Starts the instruction
        /// </summary>
        public void Start() {
            if(!IsStarted)
            {
                if(IsValid)
                {
                    IsStarted = true;
                    OnStart.ForEach(e => e.Invoke(this));
                }
                else throw new InvalidInstruction(this, "Instruction is no longer valid");

            }
        }

        private void Complete()
        {
            if(IsStarted)
            {
                if (!IsComplete)
                {
                    IsComplete = true;
                    OnComplete.ForEach(e => e.Invoke(this));
                }
                else throw new Exception("Instruction has allready completed");
            }
            else throw new Exception("Instruction has not yet been started");
        }

        public void Update(GameTime gameTime)
        {
            if(IsStarted && !IsComplete)
            {
                timeSpent += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(timeSpent >= Type.TimeCost)
                {
                    if (IsValid)
                    {
                        Complete();
                    }
                }
            }
        }

        public int CompareTo(Instruction obj) => this.Priority.CompareTo(obj.Priority);

        public override string ToString() => Type.Name;

    }
}
