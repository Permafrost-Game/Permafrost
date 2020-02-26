using System;

namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceItem : IReconstructable
    {
        [PFSerializable]
        public ResourceType ResourceType { get; set; }

        [PFSerializable]
        public int Weight { get; set; }

        public ResourceItem()
        {

        }

        public ResourceItem(ResourceType Type, int weight = 0)
        {
            this.ResourceType = Type;
            this.Weight = weight;
        }

        public ResourceItem Clone()
        {
            return (ResourceItem)MemberwiseClone();
        }

        public object Reconstruct()
        {
            return new ResourceItem(ResourceType, Weight);
        }
    }
}
