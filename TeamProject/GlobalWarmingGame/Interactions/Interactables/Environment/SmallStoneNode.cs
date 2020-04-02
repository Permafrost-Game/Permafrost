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
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public SmallStoneNode() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public SmallStoneNode(Vector2 position) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.SmallStoneNode]
        )
        {
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType(
                    id: "mine",
                    name: "Mine",
                    timeCost: 500f,
                    checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                    onStart: StartMine,
                    onComplete: EndMine
                    )
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
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Stone, 2));
            Dispose();
        }

        private void Dispose()
        {
            GameObjectManager.Remove(this);
            this.InstructionTypes.Clear();
        }

        public object Reconstruct()
        {
            return new SmallStoneNode(PFSPosition);
        }
    }
}