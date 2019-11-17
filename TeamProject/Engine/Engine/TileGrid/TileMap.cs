using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.TileGrid
{
    /// <summary>
    /// This is class will store a 2D array of <see cref="Tile"/>
    /// </summary>
    public class TileMap : IDrawable, IUpdatable
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

        /// <summary>
        /// Gets a <see cref="Engine.TileGrid.Tile"/> within a given position by rounding <paramref name="position"/>
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The <see cref="Engine.TileGrid.Tile"/> within the region of <paramref name="position"/></returns>
        public Tile GetTileAtPosition(Vector2 position)
        {
            Vector2 tileSize = Tiles[0, 0].size;
            int x = (Int32) Math.Round(position.X / tileSize.X);
            int y = (Int32) Math.Round(position.Y / tileSize.Y);

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

        public void Update(GameTime gameTime)
        {
            if ((gameTime.ElapsedGameTime.Ticks % 60) == 0)
            {
                foreach (Tile t in Tiles)
                {
                    Tile current = t;
                    Double sumTemperature = 0;
                    Double count = 0;
                    foreach (Tile adjT in AdjacentTiles(t)) {
                        sumTemperature = sumTemperature + adjT.temperature.Value;
                        count++;
                    }
                    current.temperature.Value = (sumTemperature / count);
                }
            }
        }

        private List<Tile> AdjacentTiles(Tile tile) 
        {
            List<Tile> adjTiles = new List<Tile>();

            if ((tile.Position.X - 16) >= 0)
            {

                adjTiles.Add(Tiles[((int)tile.Position.X - 16)/16, ((int)tile.Position.Y)/16]);

            }
            
            if ((tile.Position.X + 16) < Tiles.GetLength(0)*16)
            {
                
                adjTiles.Add(Tiles[((int)tile.Position.X + 16)/16, ((int)tile.Position.Y)/16]);

            }

            if ((tile.Position.Y - 16) >= 0)
            {
                
                adjTiles.Add(Tiles[((int)tile.Position.X)/16, ((int)tile.Position.Y - 16)/16]);

            }

            if ((tile.Position.Y + 16) < Tiles.GetLength(0)*16)
            {
                
                adjTiles.Add(Tiles[((int)tile.Position.X)/16, ((int)tile.Position.Y + 16)/16]);

            }

            return adjTiles;
        }
    }
}
