
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        [PFSerializable]
        public readonly int textureID;

        public TallGrass() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public TallGrass(Vector2 position, TextureTypes textureType = TextureTypes.TallGrass) : base
        (
            position: position,
            texture: Textures.Map[textureType]
        )
        {
            textureID = (int)textureType;

            InstructionTypes = new List<InstructionType>
            {
                new InstructionType(
                    id: "trim",
                    name:"Trim grass",
                    checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                    timeCost: 1000f,
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
            return new TallGrass(PFSPosition, (TextureTypes)textureID);
        }
    }
}
