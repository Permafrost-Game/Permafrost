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
        public Tile[,] Tiles { get; }
        public Vector2 TileSize { get; }

        private float timeToTemperatureUpdate;
        private readonly float timeUntilTemperatureUpdate = 2000f;

        public Vector2 Size
        {
            get { return new Vector2(Tiles.GetLength(0), Tiles.GetLength(1)); }
        }

        public TileMap(Tile[,] tiles)
        {
            TileSize = tiles[0, 0].Size;
            this.Tiles = tiles;
            timeToTemperatureUpdate = timeUntilTemperatureUpdate;
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

        #region Update Tiles Temperature
        /// <summary>
        /// Update each tile's temperature
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="globalTemperature"></param>
        public void UpdateTilesTemperatures(GameTime gameTime, float globalTemperature) 
        {
            timeToTemperatureUpdate -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeToTemperatureUpdate <= 0)
            {
                foreach (Tile tile in Tiles)
                {
                    //if tile is being heated by a structure it's temperature isn't affected by the cold
                    if (tile.Heated)
                    {
                        continue;
                    }

                    //Calculate new temperature as an average of surrounding tiles
                    List<Tile> adjacentTiles = AdjacentTiles(tile);
                    float sumTemperature = 0;

                    foreach (Tile t in adjacentTiles)
                    {
                        sumTemperature += t.Temperature.Value;
                    }

                    float averageTemperature = sumTemperature / adjacentTiles.Count;

                    //Adjust the temperature based on the global temperature
                    //Formula decreases temperature when sumTemperature > globalTemperature
                    //and increases the temperature when sumTemperature < globalTemperature
                    tile.Temperature.Value = averageTemperature + ((globalTemperature - averageTemperature) / 128);

                }
                timeToTemperatureUpdate = timeUntilTemperatureUpdate;
            }
        }

        /// <summary>
        /// Calculate the adjacent tiles to the center tile
        /// </summary>
        /// <param name="tile">Center tile</param>
        /// <returns>List of adjacent tiles</returns>
        private List<Tile> AdjacentTiles(Tile tile) 
        {
            List<Tile> adjTiles = new List<Tile>();

            Vector2 v;
            float tileSize = tile.Size.X;

            if ((tile.Position.X - tileSize) >= 0)
            {
                v = new Vector2((tile.Position.X - tileSize), tile.Position.Y);
                adjTiles.Add(GetTileAtPosition(v));
            }
            
            if ((tile.Position.X + tileSize) < Tiles.GetLength(0) * tileSize)
            {

                v = new Vector2((tile.Position.X + tileSize), tile.Position.Y);
                adjTiles.Add(GetTileAtPosition(v));

            }

            if ((tile.Position.Y - tileSize) >= 0)
            {

                v = new Vector2(tile.Position.X, (tile.Position.Y - tileSize));
                adjTiles.Add(GetTileAtPosition(v));

            }

            if ((tile.Position.Y + tileSize) < Tiles.GetLength(0) * tileSize)
            {

                v = new Vector2(tile.Position.X, (tile.Position.Y + tileSize));
                adjTiles.Add(GetTileAtPosition(v));

            }

            return adjTiles;
        }
        #endregion
    }
}
