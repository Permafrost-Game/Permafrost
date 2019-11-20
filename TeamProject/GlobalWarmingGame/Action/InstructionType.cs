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

        public delegate void Action();
        private Action action;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="name">Display name</param>
        /// <param name="description">Display description</param>
        /// <param name="action">Method that should be called on <see cref="InstructionType.Act"/></param>
        public InstructionType(string id, string name, string description, Action action = default)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.action = action;
        }


        public void Act()
        {
            action();
        }

        
    }
}

