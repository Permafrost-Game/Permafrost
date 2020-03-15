using Microsoft.Xna.Framework;
using SimplexNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.TileGrid
{
    public static class TileMapGenrator
    {

        /// <summary>
        /// Generates a <see cref="TileMap"/>
        /// </summary>
        /// <param name="seed">The Seed for noise generation</param>
        /// <param name="scale">The Scale for noise generation</param>
        /// <param name="xOffset">The number of tile to ofset in the X direction</param>
        /// <param name="yOffset">The number of tile to ofset in the Y direction</param>
        /// <param name="width">The number of tiles to be generated in the X direction</param>
        /// <param name="height">The number of tiles to be generated in the Y direction</param>
        /// <param name="tileSet">The TileSet that is to be used</param>
        /// <returns>A TileMap</returns>
        public static TileMap GenerateTileMap(int seed, float scale, int xOffset, int yOffset, int width, int height, TileSet tileSet, float globalTemperature)
        {
            Noise.Seed = seed;
            Tile[,] tiles = new Tile[width, height];

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    float value = Noise.CalcPixel2D(x + xOffset, y + yOffset, scale);
                    int tileCount = tileSet.tileSetTextures.Count - 1; //-1 because of texture 0 is for errors

                    for (int counter = 1; counter <= tileCount; counter++)
                    {
                        if (value <= (255f / tileCount) * counter)
                        {
                            tiles[x, y] = new Tile(
                                texture: tileSet.tileSetTextures[counter],
                                position: new Vector2(x * tileSet.textureSize.X, y * tileSet.textureSize.Y),
                                size: tileSet.textureSize,
                                walkable: !tileSet.tileSetTextures[counter].Name.Equals("textures/tiles/main_tileset/water"),
                                initialTemperature: globalTemperature
                                );
                            break;
                        }
                    }

                    if(tiles[x, y] == null)
                    {
                        tiles[x, y] = new Tile( //Display an error texture
                                texture: tileSet.tileSetTextures[0],
                                position: new Vector2(x * tileSet.textureSize.X, y * tileSet.textureSize.Y),
                                size: tileSet.textureSize,
                                walkable: true,
                                initialTemperature: globalTemperature
                                );
                    }

                }
                    

            return new TileMap(tiles);
        }

    }
}
