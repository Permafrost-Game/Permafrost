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
    /// This is a class for a tile set
    /// </summary>
    public class TileSet
    {
        public readonly List<Texture2D> tileSetTextures;
        public readonly Vector2 textureSize;

        public TileSet(List<Texture2D> tileSetTextures, Vector2 textureSize)
        {
            this.tileSetTextures = tileSetTextures;
            this.textureSize = textureSize;
        }

    }
}
