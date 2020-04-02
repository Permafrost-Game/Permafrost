using Microsoft.Xna.Framework;
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
        public static TileMap GenerateTileMap(int seed, int xOffset, int yOffset, int width, int height, TileSet tileSet, float globalTemperature)
        {
            Tile[,] tiles = new Tile[width, height];

            //1st Pass: Grass & Snow
            seed++;
            FastNoise noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
            noise.SetFrequency(0.015f);
            noise.SetFractalOctaves(2);

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    float value = noise.GetNoise(x + xOffset, y + yOffset);

                    if (value >= 0.1f)
                    {
                        //Display GRASS tile
                        tiles[x, y] = new Tile(
                                    texture: tileSet.TileSetTextures[2],
                                    position: new Vector2(x * tileSet.textureSize.X, y * tileSet.textureSize.Y),
                                    size: tileSet.textureSize,
                                    walkable: true,
                                    initialTemperature: globalTemperature
                                    );
                    }

                    else if (value < 0.1f)
                    {
                        //Display SNOW tile
                        tiles[x, y] = new Tile(
                                    texture: tileSet.TileSetTextures[3],
                                    position: new Vector2(x * tileSet.textureSize.X, y * tileSet.textureSize.Y),
                                    size: tileSet.textureSize,
                                    walkable: true,
                                    initialTemperature: globalTemperature
                                    );
                    }
                }

            //2nd Pass: Tundra & Stone
            seed++;
            noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.PerlinFractal);
            noise.SetFrequency(0.025f);
            noise.SetFractalOctaves(1);

            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                {
                    float value = noise.GetNoise(x + xOffset, y + yOffset);

                    if (value <= -0.4f)
                    {
                        //Display TUNDRA tile
                        tiles[x, y] = new Tile(
                                    texture: tileSet.TileSetTextures[1],
                                    position: new Vector2(x * tileSet.textureSize.X, y * tileSet.textureSize.Y),
                                    size: tileSet.textureSize,
                                    walkable: true,
                                    initialTemperature: globalTemperature
                                    );
                    }

                    else if (value >= 0.4f)
                    {
                        //Display STONE tile
                        tiles[x, y] = new Tile(
                                    texture: tileSet.TileSetTextures[4],
                                    position: new Vector2(x * tileSet.textureSize.X, y * tileSet.textureSize.Y),
                                    size: tileSet.textureSize,
                                    walkable: true,
                                    initialTemperature: globalTemperature
                                    );
                    }
                }

            //3rd Pass: Rivers
            seed++;
            noise = new FastNoise(seed);
            noise.SetNoiseType(FastNoise.NoiseType.CubicFractal);
            noise.SetFrequency(0.015f);
            noise.SetFractalType(FastNoise.FractalType.Billow);
            noise.SetFractalOctaves(1);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    float value = noise.GetNoise(x + xOffset, y + yOffset);

                    if (value < -0.9f)
                    {
                        tiles[x, y] = new Tile(
                            texture: tileSet.TileSetTextures[5],
                            position: new Vector2(x * tileSet.textureSize.X, y * tileSet.textureSize.Y),
                            size: tileSet.textureSize,
                            walkable: false,
                            initialTemperature: globalTemperature
                            );
                    }
                }
            }

            return new TileMap(tiles);
        }
    }
}
