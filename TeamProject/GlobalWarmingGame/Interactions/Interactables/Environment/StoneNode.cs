
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    class StoneNode : Sprite, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; }

        public StoneNode(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "StoneNode",
            depth: 0.7f,
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
            InstructionTypes.Add(new InstructionType("mine", "Mine", "Mine stone", onStart: Mine));
        }

        private void Mine(IInstructionFollower follower)
        {
            follower.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.MakeResource(Resource.stone), 4));
            //Maybe destory the node or allow 3 more mine operations
            GameObjectManager.Remove(this);
        }
    }
}
