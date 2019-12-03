using GlobalWarmingGame.ResourceItems;

namespace GlobalWarmingGame.Action
{
    /// <summary>
    /// This class descrives a class of Instruction
    /// </summary>
    class InstructionType
    {
        public string ID { get; }
        public string Name { get; }
        public string Description { get; }
        public ResourceItem ResourceItem { get; }

        public delegate void Action(PathFindable pathFindable);
        private readonly Action action;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="name">Display name</param>
        /// <param name="description">Display description</param>
        /// <param name="action">Method that should be called on <see cref="InstructionType.Act"/></param>
        public InstructionType(string id, string name, string description, Action action = default)
        {
            ID = id;
            Name = name;
            Description = description;

            this.action = action;
        }

        public InstructionType(string id, string name, string description, ResourceItem resourceItem, Action action = default)
        {
            ID = id;
            Name = name;
            Description = description;
            ResourceItem = resourceItem;

            this.action = action;
        }
        
        public void Act(PathFindable pathFindable)
        {
            action(pathFindable);
        }

        
    }
}

