
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources.ResourceTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class StorageUnit : InteractableGameObject, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Stone(), 4),
                                                                                                   new ResourceItem(new Wood(), 8)};
        private Inventory inventory;
        
        public StorageUnit(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "StorageUnit",
            depth: 0.7f,
            texture: texture,
            instructionTypes: new List<InstructionType>() { }
        )
        {
            inventory = new Inventory(100f);
            InstructionTypes.Add(new InstructionType("store", "Store", "Store items", Store));
        }

        private void Store(Colonist colonist)
        {
            //Open menu to either store or retrieve items
        }
    }
}
