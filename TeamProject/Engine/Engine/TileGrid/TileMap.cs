﻿using Microsoft.Xna.Framework;
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
        public Tile[,] Tiles { get; }

        private float timeToTempTick;
        private readonly float timeUntilTempTick = 2000f;

        public Vector2 Size
        {
            get { return new Vector2(Tiles.GetLength(0), Tiles.GetLength(1)); }
        }

        public TileMap(Tile[,] tiles)
        {
            this.Tiles = tiles;
            timeToTempTick = timeUntilTempTick;
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
            Vector2 tileSize = Tiles[0, 0].Size;
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

        #region Update Tiles Temperature
        public void UpdateTilesTemperatures(GameTime gameTime, float globalTemperature) 
        {
            timeToTempTick -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeToTempTick <= 0)
            {
                foreach (Tile tile in Tiles)
                {
                    //if tile is being heated by a structure it's temperature isn't affected by the cold
                    if (tile.Heated)
                    {
                        continue;
                    }

                    Tile current = tile;
                    float sumTemperature = current.temperature.Value;
                    float count = 1;
                    foreach (Tile adjT in AdjacentTiles(tile))
                    {
                        sumTemperature += adjT.temperature.Value;
                        count++;
                    }

                    current.temperature.Value = (sumTemperature / (count));

                    //Try to lower/raise the tile temp to the global temp
                    if (tile.temperature.Value < globalTemperature)
                    {
                        float Temperature = tile.temperature.Value;
                        tile.temperature.SetTemp(Temperature + (globalTemperature - Temperature) / 8);
                    }
                    else if (tile.temperature.Value > globalTemperature)
                    {
                        float Temperature = tile.temperature.Value;
                        tile.temperature.SetTemp(Temperature + (globalTemperature - Temperature) / 8);
                    }

                }
                timeToTempTick = timeUntilTempTick;
            }
        }

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
