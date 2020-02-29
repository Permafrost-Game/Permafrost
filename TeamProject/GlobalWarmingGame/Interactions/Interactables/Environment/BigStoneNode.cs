
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    public class BigStoneNode : Sprite, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; }

        public BigStoneNode(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            origin: new Vector2(texture.Width / 2f, texture.Height / 2f),
            tag: "StoneNode",
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("mine", "Mine", "Mine stone", 0,
                                       new List<ResourceItem>() {new ResourceItem(ResourceTypeFactory.GetResource(Resource.Pickaxe), 1)}, onStart: Mine)
            };
        }

        private void Mine(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 8));
            //Maybe destory the node or allow 3 more mine operations
            GameObjectManager.Remove(this);
        }
    }
}
