
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    public class TallGrass : Sprite, IInteractable, IReconstructable
    {
        public List<InstructionType> InstructionTypes { get; }

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public TallGrass() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public TallGrass(Vector2 position) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.TallGrass]
        )
        {
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType(
                    id: "trim",
                    name:"Trim grass",
                    checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                    onComplete: Trim
                    )
            };
        }

        private void Trim(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Fibers, 4));
            Dispose();
        }

        private void Dispose()
        {
            GameObjectManager.Remove(this);
            this.InstructionTypes.Clear();
        }

        public object Reconstruct()
        {
            return new TallGrass(PFSPosition);
        }
    }
}
