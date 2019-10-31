using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.TileGrid
{
    /// <summary>
    /// This is class will store a 2D array of <see cref="Tile"/>
    /// </summary>
    public class TileMap : IDrawable
    {

        public Tile[,] Tiles { get; }

        public TileMap(Tile[,] tiles)
        {
            this.Tiles = tiles;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in Tiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        public Tile GetTileAtPosition(Vector2 position)
        {
            Vector2 tileSize = Tiles[0, 0].size;
            int x = (Int32) Math.Round(position.X / tileSize.X);
            int y = (Int32) Math.Round(position.Y / tileSize.Y);
            return Tiles[x, y];
        }

    }
}
