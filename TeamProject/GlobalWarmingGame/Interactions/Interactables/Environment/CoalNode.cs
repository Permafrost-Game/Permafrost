
using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources.ResourceTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    class CoalNode : Sprite, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; }

        public CoalNode(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "CoalNode",
            depth: 0.7f,
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
            InstructionTypes.Add(new InstructionType("mine", "Mine", "Mine coal", new ResourceItem(new Coal(), 4), Mine));
        }

        private void Mine(Colonist colonist)
        {
            //Maybe destory the node or allow 3 more mine operations
        }
    }
}
