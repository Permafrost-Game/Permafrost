
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
                                       new List<ResourceItem>() {new ResourceItem(ResourceTypeFactory.GetResource(Resource.Pickaxe), 1)}, timeCost: 3000f, onStart: StartMine, onComplete: EndMine)
            };
        }

        private void StartMine(Instruction instruction)
        {
            //TODO Stone mine sound
            //SoundFactory.PlaySoundEffect(Sound.stone_mine);
        }

        private void EndMine(Instruction instruction)
        {
            SoundFactory.PlaySoundEffect(Sound.StonePickup);
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 8));
            GameObjectManager.Remove(this);
        }
    }
}
