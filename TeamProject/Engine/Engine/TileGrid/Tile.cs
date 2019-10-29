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

        //Default tag, walkable boolean
        private readonly int tag = -1;
        private bool walkable = true;

        //Parent tile used for pathfinding
        private Tile parent; 


        public Tile(Texture2D texture, Vector2 Position, Vector2 size) : base(Position, size)
        {
            parent = null;
            this.texture = texture;
            this.size = size;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(position.ToPoint(), size.ToPoint()), Color.White);
        }
        
        //Equality testing
        public bool equals(object t)
        {
            if (t is Tile)
            {
                Tile tile = (Tile)t;
                if (this.size.Equals(tile.getSize()) && this.position.Equals(tile.getPosition()))
                {
                    return true;
                }
            }
            return false;
        }

        public bool hasParent() {
            return (!parent.equals(null));            
        }

        //Getter methods

        public int getTag() {
            return tag;
        }

        public Vector2 getSize()
        {
            return size;
        }

        public bool getWalkable() {
            return walkable;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public Tile getParent() {
            return parent;
        }

        //Setter methods

        public void setParent(Tile parent) {
            this.parent = parent;                        
        }

    }
}
