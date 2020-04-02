using Engine.Drawing;
using Engine.TileGrid;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class BridgeTile : Sprite, IInteractable, IBuildable, IReconstructable
    {
        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.Wood, 4)};

        public List<InstructionType> InstructionTypes { get; private set; } = new List<InstructionType>();

        //CHANGE TEXTURETYPE TO NEW ENUM
        public BridgeTile(Vector2 position, TextureTypes texture = TextureTypes.BigStoneNode) : base (position: position, texture: Textures.Map[texture], depth: CalculateDepth(position, -5))
        {
            float tileSize = GameObjectManager.ZoneMap.TileSize.X;
            float[] xDirections = new float[] { tileSize, -tileSize, 0, 0, tileSize, -tileSize, tileSize, -tileSize };
            float[] yDirections = new float[] { 0, 0, tileSize, -tileSize, tileSize, tileSize, -tileSize, -tileSize };

            for (int i = 0; i < xDirections.Length; i++)
            {
                Vector2 tilePosition = new Vector2((Position.X + xDirections[i]), (Position.Y + yDirections[i]));
                Tile tile = GameObjectManager.ZoneMap.GetTileAtPosition(tilePosition);
                tile.Walkable = true;
            }
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }

        public object Reconstruct()
        {
            return new BridgeTile(PFSPosition);
        }
    }
}
