using GlobalWarmingGame.ResourceItems;
using System.Collections.Generic;

namespace GlobalWarmingGame.Action
{
    public delegate void InstructionEvent(Instruction instruction);

    /// <summary>
    /// This class descrives a class of Instruction
    /// </summary>
    public class InstructionType
    {
        public string ID { get; }
        public string Name { get; }
        public string Description { get; }
        public int Priority { get; }
        public float TimeCost { get; }

        public readonly InstructionEvent onStart;
        public readonly InstructionEvent onComplete;


        public List<ResourceItem> RequiredResources { get; }

        /// <param name="id">Unique ID</param>
        /// <param name="name">Display name</param>
        /// <param name="description">Display description</param>
        /// <param name="priority">The priority  given to <see cref="Instruction"/>s of this type</param> //TODO: range
        /// <param name="requiredResources">The resources required to start <see cref="Instruction"/>s of this type</param>
        /// <param name="timeCost">The time in ms that <see cref="Instruction"/>s of this type take to execute</param>
        /// <param name="onStart">The method that is called when the instruction has started</param>
        /// <param name="onComplete">The method that is called when the instruction has finnished</param>
        public InstructionType(string id, string name, string description, int priority = 0, List<ResourceItem> requiredResources = null, float timeCost = 0, InstructionEvent onStart = default, InstructionEvent onComplete = default)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Priority = priority;
            this.RequiredResources = requiredResources;
            this.TimeCost = timeCost;
            this.onStart = onStart;
            this.onComplete = onComplete;
        }
        
    }
}

