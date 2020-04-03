using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.TileGrid
{
    /// <summary>
    /// This is class will store a 2D array of <see cref="Tile"/>
    /// </summary>
    public class TileMap : Engine.Drawing.IDrawable
    {

        public bool TemperatureMode { get; set; } = false;
        public Tile[,] Tiles { get; set; }
        public Vector2 TileSize { get; }

        public Vector2 Size
        {
            get { return new Vector2(Tiles.GetLength(0), Tiles.GetLength(1)); }
        }

        public TileMap(Tile[,] tiles)
        {
            TileSize = tiles[0, 0].Size;
            this.Tiles = tiles;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in Tiles)
            {
                if(TemperatureMode)
                {
                    tile.DrawTemperatureMode(spriteBatch);
                }
                else
                {
                    tile.Draw(spriteBatch);
                    
                }
                
            }
        }


        /// <summary>
        /// Gets a <see cref="Engine.TileGrid.Tile"/> within a given position by rounding <paramref name="position"/>
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The <see cref="Engine.TileGrid.Tile"/> within the region of <paramref name="position"/></returns>
        public Tile GetTileAtPosition(Vector2 position)
        {
            int x = (Int32) Math.Round(position.X / TileSize.X);
            int y = (Int32) Math.Round(position.Y / TileSize.Y);

            Tile t = null;
            if(x >= 0 &&
               y >= 0 &&
               x < Tiles.GetLength(0) &&
               y < Tiles.GetLength(1))
            {
                t = Tiles[x, y];
            }            

            return t;
        }
    }
}
