namespace GlobalWarmingGame.Action
{
    class InstructionType
    {
        public string ID { get; }
        public string Name { get; }
        public string Description { get; }
        public int FoodEffect { get; }

        public InstructionType(string id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
        public InstructionType(string id, string name, string description, int foodEffect) : this(id, name, description)
        { 
            FoodEffect = foodEffect;
        }
    }
}

