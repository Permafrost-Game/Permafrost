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
        public Vector2 size { get; }
        public Vector2 Position { get; }

        //Default tag, walkable boolean
        private readonly int tag = -1;
        public bool Walkable { get; }

        public Tile(Texture2D texture, Vector2 Position, Vector2 size) : base(Position, size)
        {
            this.texture = texture;
            this.size = size;
            Walkable = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(position.ToPoint(), size.ToPoint()), Color.White);
        }

        //Equality testing
        public override bool Equals(object t)
        {
            if (t is Tile tile)
            {
                tile = (Tile)t;
                if (this.size.Equals(tile.size) && this.Position.Equals(tile.Position) && this.GetHashCode() == tile.GetHashCode())
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() + tag);
        }

    }
}
