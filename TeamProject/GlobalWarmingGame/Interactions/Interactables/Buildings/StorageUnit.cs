
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
                                                                                      new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood),  8)};

        public List<InstructionType> InstructionTypes { get; }

        public ResourceItem ResourceItem { get; private set; }

        public StorageUnit(Vector2 position) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.StorageUnit]
        )
        {
            InstructionTypes = CreateSetResourceInstructionTypes();
        }

        private List<InstructionType> CreateSetResourceInstructionTypes()
        {
            return Enum.GetValues(typeof(Resource)).Cast<Resource>()
                .Select(r => new InstructionType(
                    id: r.ToString(),
                    name: $"Set {r.ToString()}",
                    description: $"Set {r.ToString()} as the active resource",
                    requiredResources: new List<ResourceItem> { new ResourceItem(ResourceTypeFactory.GetResource(r)) },
                    onComplete: SetResource
                )).ToList();
        }


        private void SetResource(Instruction instruction)
        {
            if(instruction.Type.RequiredResources.Count != 1) throw new Exception($"{this.GetType().ToString()} expected RequiredResources.Count to be 1, but was {instruction.Type.RequiredResources.Count}");
            ResourceItem = instruction.Type.RequiredResources[0];
            InstructionTypes.Clear();
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if(ResourceItem != null)
            {
                Texture2D resourceTexture = ResourceItem.ResourceType.Texture;
                spriteBatch.Draw(
                    texture: resourceTexture,
                    position: Position,
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: Rotation,
                    origin: CalculateOrigin(new Vector2(resourceTexture.Width, resourceTexture.Height)),
                    scale: 1f,
                    effects: SpriteEffect,
                    layerDepth: CalculateDepth(Position, 0.1f)
                );
            }
        }

    }
}
