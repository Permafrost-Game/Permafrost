
using Microsoft.Xna.Framework.Graphics;

namespace GlobalWarmingGame.UI
{
    class ItemElement
    {
        public Texture2D Texture { get; set; }
        public string Label { get; set; }

        public ItemElement(Texture2D texture, string label)
        {
            Texture = texture;
            Label = label;
        }
    }
}
