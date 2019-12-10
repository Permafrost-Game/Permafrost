
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources.ResourceTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class StoneNode : InteractableGameObject
    {        
        public StoneNode(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "StoneNode",
            depth: 0.7f,
            texture: texture,
            instructionTypes: new List<InstructionType>() { }
        )
        {
            InstructionTypes.Add(new InstructionType("mine", "Mine", "Mine stone", new ResourceItem(new Stone(), 5), Mine));
        }

        private void Mine(Colonist colonist)
        {
            //Maybe destory the node or allow 3 more mine operations
        }
    }
}
