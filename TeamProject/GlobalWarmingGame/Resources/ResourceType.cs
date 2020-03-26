using Microsoft.Xna.Framework.Graphics;

namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceType : IReconstructable
    {
        [PFSerializable]
        public readonly int resourceID;

        public Resource ResourceID { get => (Resource)resourceID; }

        public Texture2D Texture { get; }

        [PFSerializable]
        public readonly string displayName;

        [PFSerializable]
        public readonly string description;

        [PFSerializable]
        public readonly int textureIconID;

        public ResourceType()
        {

        }

        public ResourceType(string displayName, string description, TextureIconTypes textureIconType)
        {
            this.displayName = displayName;
            this.description = description;
            this.Texture = Textures.MapIcon[textureIconType];

            textureIconID = (int)textureIconType;
        }

        public ResourceType(Resource resourceID, string displayName, string description, TextureIconTypes textureIconType) :
            this(displayName, description, textureIconType)
        {
            this.resourceID = (int)resourceID;
        }
        public override bool Equals(object obj)
        {
            if (obj is ResourceType type)
            {
                return this.resourceID == type.resourceID;
            }

            return false;
        }
        public override int GetHashCode() => this.resourceID.GetHashCode();

        public object Reconstruct()
        {
            return new ResourceType((Resource)resourceID, displayName, description, (TextureIconTypes)textureIconID);
        }
    }
}
