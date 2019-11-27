namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceItem
    {
        public ResourceType Type { get; set; }
        public int Amount { get; set; }

        public ResourceItem(ResourceType Type, int Amount = 0)
        {
            this.Type = Type;
            this.Amount = Amount;
        }
    }
}
