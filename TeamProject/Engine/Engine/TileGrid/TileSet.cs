using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.TileGrid
{
    /// <summary>
    /// This is a class for a tile set
    /// </summary>
    public class TileSet
    {
        public Dictionary<int, Texture2D> tileSetTextures { get; set; }
        public readonly Vector2 textureSize;

        public TileSet(Dictionary<int, Texture2D> tileSetTextures, Vector2 textureSize)
        {
            this.tileSetTextures = tileSetTextures;
            this.textureSize = textureSize;
        }

        



    }
}
