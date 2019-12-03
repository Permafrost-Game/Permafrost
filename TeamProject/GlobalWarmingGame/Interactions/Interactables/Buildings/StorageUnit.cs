
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class StorageUnit : InteractableGameObject
    {
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
            InstructionTypes.Add(new InstructionType("store", "Store", "Store items", Store));
            InstructionTypes.Add(new InstructionType("retrieve", "Retrieve", "Retrieve items", Retrieve));
        }

        public void Store(Colonist colonist)
        {

        }
        public void Retrieve(Colonist colonist)
        {

        }
    }
}
