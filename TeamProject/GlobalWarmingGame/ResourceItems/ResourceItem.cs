namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceItem
    {
        public ResourceType Type { get; set; }
        public int Ammount { get; set; }

        private ResourceItem(ResourceType Type, int Ammount = 0)
        {
            this.Type = Type;
            this.Ammount = Ammount;
        }

    }
}
