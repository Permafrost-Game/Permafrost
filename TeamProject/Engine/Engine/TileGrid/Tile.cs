using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.TileGrid
{
    /// <summary>
    /// This class reperesents a Tile Map Tile Game Object
    /// </summary>
    public class Tile : GameObject, IDrawable
    {
        private readonly Texture2D texture;
        private readonly Vector2 size;

        public Tile(Texture2D texture, Vector2 Position, Vector2 size) : base(Position)
        {
            this.texture = texture;
            this.size = size;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(position.ToPoint(), size.ToPoint()), Color.White);
        }
    }
}
