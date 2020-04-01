using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    /// <summary>
    /// Convert oven into a Mill for wheat.
    /// </summary>
    class Mill : Sprite, IInteractable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.MachineParts, 1),
                                                                                                   new ResourceItem(Resource.Stone, 8),
                                                                                                   new ResourceItem(Resource.Wood, 16)};

        public List<InstructionType> InstructionTypes { get; }

        public Mill(Vector2 position, Texture2D texture) : base
        (
            position: position,
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
