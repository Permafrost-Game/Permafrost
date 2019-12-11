using Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.TileGrid
{
    /// <summary>
    /// This class reperesents a Tile Map Tile Game Object
    /// </summary>
    public class Tile : GameObject, Engine.Drawing.IDrawable
    {
        private readonly Texture2D texture;
        public Vector2 size { get; }
        public new Vector2 Position { get; }
        public Temperature temperature;
        public bool Heated { get; set; }

        ///<summary>Default tag, walkable boolean</summary>
        private readonly int tag = -1;
        public bool Walkable { get; }

        public Tile(Texture2D texture, Vector2 Position, Vector2 size, bool Walkable) : base(Position, size)
        {
            this.Position = Position;
            this.texture = texture;
            this.size = size;
            this.Walkable = Walkable;
            temperature = new Temperature(ZoneManager.GlobalTemperature);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texture, new Rectangle(base.Position.ToPoint(), size.ToPoint()), Color.White);
            spriteBatch.Draw(
                texture,
                new Rectangle(base.Position.ToPoint(), size.ToPoint()),
                new Rectangle(
                    new Point( (Position.X/32) % 2 == 0? 0 : 32, (Position.Y/32) % 2 == 0? 0 : 32),
                    size.ToPoint()),
                Color.White
                );
        }

        ///<summary>Equality testing</summary>
        public override bool Equals(object t)
        {
            if (t is Tile tile)
            {
                tile = (Tile)t;
                if (this.size.Equals(tile.size) && this.Position.Equals(tile.Position))
                {
                    return true;
                }
            }
            return false;
        }

        ///<summary>Unique hashcode based on tag</summary>
        public override int GetHashCode()
        {
            return (base.GetHashCode() + tag);
        }
    }
}
