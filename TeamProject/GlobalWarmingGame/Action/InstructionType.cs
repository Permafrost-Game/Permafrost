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

        public InstructionType(string id, string name, string description, Action action)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
            this.action = action;
        }
        /// <summary>
        /// Creates a new InstructionType with no action
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="name">Display Name</param>
        /// <param name="description">Display Description</param>
         public InstructionType(string id, string name, string description)
        {
            this.ID = id;
            this.Name = name;
            this.Description = description;
        }


        public void Act()
        {
            action();
        }

        
    }
}

