
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
    class StorageUnit : Sprite, IInteractable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 4),
                                                                                                   new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 8)};

        public List<InstructionType> InstructionTypes { get; }

        
        public StorageUnit(Vector2 position) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.storageUnit]
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
