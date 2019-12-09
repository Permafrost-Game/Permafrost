
using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Animals
{
    class Rabbit : PassiveMovingGameObject
    {
        public Rabbit(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Rabbit",
            depth: 0.9f,
            texture: texture,
            instructionTypes: new List<InstructionType>(),
            speed: 0.05f
        )
        {
            InstructionTypes.Add(new InstructionType("hunt", "Hunt", "Hunt the Rabbit", new ResourceItem(new Food(), 2), Hunt));
        }

        public void Hunt(Colonist colonist)
        {
            GameObjectManager.Remove(this);
        }
    }
}
