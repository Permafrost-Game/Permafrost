using Microsoft.Xna.Framework.Graphics;

namespace GlobalWarmingGame.ResourceItems
{
    public class ResourceType
    {
        public string ID { get; private set; }
        public string DisplayName { get; private set; }
        public string Description { get; private set; }
        public float Weight { get; private set;}

        //Texture2D texture; //TODO - Add Textures for each ResourceType

        public ResourceType(string id, string displayName, string description, float weight/*, Texture2D texture*/)
        {
            ID = id;
            DisplayName = displayName;
            Description = description;
            Weight = weight;

            //this.texture = texture;
        }
    }
}
