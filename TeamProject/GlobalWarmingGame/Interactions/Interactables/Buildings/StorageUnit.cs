
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

        private readonly Inventory inventory;
        
        public StorageUnit(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "StorageUnit",
            depth: 0.7f,
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
            inventory = new Inventory(100f);
            InstructionTypes.Add(new InstructionType("store", "Store", "Store items", onStart: Store));
        }

        private void Store(IInstructionFollower follower)
        {
            //Open menu to either store or retrieve items
        }

        public void StoreItem(ResourceItem item) 
        {
            inventory.AddItem(item);
        }

        public ResourceItem RetrieveItem(ResourceType type)
        {
            return inventory.Resources[type];
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
