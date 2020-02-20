﻿using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.ResourceItems;

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

        public delegate void OnStart(IInstructionFollower follower);
        private readonly OnStart onStart;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="name">Display name</param>
        /// <param name="description">Display description</param>
        /// <param name="onStart">The method that is called when the instruction has started</param>
        /// /// <param name="onComplete">The method that is called when the instruction has started</param>
        public InstructionType(string id, string name, string description, int priority = 0, OnStart onStart = default)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.Priority = priority;

            this.onStart = onStart;
        }
        
        public void Start(IInstructionFollower follower)
        {
            onStart?.Invoke(follower);
        }
    }
}

