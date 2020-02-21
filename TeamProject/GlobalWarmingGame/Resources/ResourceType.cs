using Microsoft.Xna.Framework.Graphics;

namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceType
    {
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public Texture2D Texture { get; private set; }

        public ResourceType(string displayName, string description, Texture2D texture)
        {
            DisplayName = displayName;
            Description = description;
            this.Texture = texture;
        }
    }
}
