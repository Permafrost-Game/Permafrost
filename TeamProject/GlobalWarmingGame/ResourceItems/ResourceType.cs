
using Microsoft.Xna.Framework.Graphics;

namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceType
    {
        private Texture2D texture;
        public string ID { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }

        public ResourceType(string id, string displayName, string description, Texture2D texture)
        {
            this.ID = id;
            this.DisplayName = displayName;
            this.Description = description;
            this.texture = texture;
        }

    }
}
