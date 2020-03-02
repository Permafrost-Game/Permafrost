using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    public class SmallStoneNode : Sprite, IInteractable, IReconstructable
    {
        public List<InstructionType> InstructionTypes { get; }

        [PFSerializable]
        public new Vector2 Position { get; set; }

        [PFSerializable]
        public readonly int textureID;

        public SmallStoneNode() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public SmallStoneNode(Vector2 position, TextureTypes textureType) : base
        (
            position: position,
            size: new Vector2(Textures.Map[textureType].Width, Textures.Map[textureType].Height),
            rotation: 0f,
            origin: new Vector2(Textures.Map[textureType].Width / 2f, Textures.Map[textureType].Height / 2f),
            tag: "StoneNode",
            texture: Textures.Map[textureType]
        )
        {
            Position = base.Position;
            textureID = (int)textureType;

            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("mine", "Mine", "Mine stone", timeCost: 500f, onStart: StartMine, onComplete: EndMine)
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
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 2));
            GameObjectManager.Remove(this);
        }

        public object Reconstruct()
        {
            return new SmallStoneNode(Position, (TextureTypes)textureID);
        }
    }
}