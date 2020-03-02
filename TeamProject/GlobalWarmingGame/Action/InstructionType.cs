using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.ResourceItems;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Action
{
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

        public delegate void InstructionEvent(Instruction instruction);

        public readonly InstructionEvent onStart;
        public readonly InstructionEvent onComplete;


        public List<ResourceItem> RequiredResources { get; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="name">Display name</param>
        /// <param name="description">Display description</param>
        /// <param name="onComplete">The method that is called when the instruction has started</param>
        /// /// <param name="onCompletee">The method that is called when the instruction has started</param>
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

