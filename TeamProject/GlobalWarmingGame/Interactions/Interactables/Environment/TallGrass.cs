
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    public class TallGrass : Sprite, IInteractable
    {
        public List<InstructionType> InstructionTypes { get; }

        public TallGrass(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            origin: new Vector2(texture.Width / 2f, texture.Height / 2f),
            tag: "TallGrass",
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("trim", "Trim grass", "Trim grass", onStart: Trim)
            };
        }

        private void Trim(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Fibers), 4));
            //Maybe destory the node or allow 3 more mine operations
            GameObjectManager.Remove(this);
        }
    }
}
