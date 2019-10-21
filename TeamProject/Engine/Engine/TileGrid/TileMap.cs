using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
