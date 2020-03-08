
using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class StorageUnit : Sprite, IInteractable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; } = new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 4),
                                                                                                   new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 8)};

        public List<InstructionType> InstructionTypes { get; }

        
        public StorageUnit(Vector2 position) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.StorageUnit]
        )
        {
            InstructionTypes = (Enum.GetValues(typeof(Resource)).Cast<Resource>()
                .Select(i => new InstructionType(Resource, Resource, Resource, ResourceTypeFactory.GetResource(Resource))).ToList());
        }


        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
