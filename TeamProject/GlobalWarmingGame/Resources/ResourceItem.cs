using System;

namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceItem
    {
        public ResourceType ResourceType { get; set; }
        public int Weight { get; set; }

        public ResourceItem(ResourceType Type, int weight = 0)
        {
            this.ResourceType = Type;
            this.Weight = weight;
        }

        public ResourceItem Clone()
        {
            return (ResourceItem)MemberwiseClone();
        }

        public override string ToString() 
        {
            return ResourceType.DisplayName;                    
        }

    }
}
