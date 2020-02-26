using Microsoft.Xna.Framework.Graphics;

namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceType : IReconstructable
    {
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

        public object Reconstruct()
        {
            return new ResourceType(displayName, description, (TextureIconTypes)textureIconID);
        }
    }
}
