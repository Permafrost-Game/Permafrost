
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Forge : InteractableGameObject
    {
        private Inventory inventory;
        
        public Forge(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Forge",
            depth: 0.7f,
            texture: texture,
            instructionTypes: new List<InstructionType>() { }
        )
        {
            InstructionTypes.Add(new InstructionType("forge", "Forge", "Forge iron item", ForgeItem));
        }

        public void ForgeItem(PathFindable findable)
        {
            //Check colonist's inventory for iron
            //Force the colonist to wait at the station until job is done
        }
    }
}
