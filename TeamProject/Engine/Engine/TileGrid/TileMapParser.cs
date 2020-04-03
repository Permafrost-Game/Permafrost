using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.TileGrid
{
    /// <summary>
    ///  TODO: This class is just for testing purposes!!
    ///  This class should be replaced proably with a <see cref="Microsoft.Xna.Framework.Content.Pipeline.ContentImporter"/>
    /// </summary>
    [Obsolete]
    public static class TileMapParser
    {

        /// <param name="filePath">The Map file that is to be loaded</param>
        /// <param name="tileSet">The Tile set that is to be loaded</param>
        /// <returns></returns>
        public static TileMap ParseTileMap(string filePath, TileSet tileSet)
        {
            List<string> rows = ContentReader.LoadText(filePath);

            //var tiles = new List<List<Tile>>();
            int height = rows.Count();
            int width = rows[0].Split(',').Count();

            Tile[,] tiles = new Tile[width, height];

            for(int y = 0; y < height; y++)
            {
                string[] cols = rows[y].Split(',');

                if(cols.Count() != width)
                {
                    //TODO custom exception
                    throw new Exception("File has irregular widths");
                }

                for(int x = 0; x < cols.Count(); x++)
                {
                    Vector2 position = new Vector2(x * tileSet.textureSize.X, y * tileSet.textureSize.Y);

                    string key = rows[y].Split(',')[x];
                    Texture2D texture = tileSet.TileSetTextures[Int32.Parse(key)];

                    tiles[x,y] = new Tile(texture, position, tileSet.textureSize, !texture.Name.Equals("textures/tiles/main_tileset/water"),-2);
                    
                    
                }
               
            }

            return new TileMap(tiles);
        }

    }
}
