
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
        public new Vector2 Position { get; set; }

        [PFSerializable]
        public readonly int textureID;

        public TallGrass() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public TallGrass(Vector2 position, TextureTypes textureType) : base
        (
            position: position,
            size: new Vector2(Textures.Map[textureType].Width, Textures.Map[textureType].Height),
            rotation: 0f,
            origin: new Vector2(Textures.Map[textureType].Width / 2f, Textures.Map[textureType].Height / 2f),
            tag: "TallGrass",
            texture: Textures.Map[textureType]
        )
        {
            Position = base.Position;
            textureID = (int)textureType;

            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("trim", "Trim grass", "Trim grass", onComplete: Trim)
            };
        }

        private void Trim(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Fibers), 4));
            //Maybe destory the node or allow 3 more mine operations
            GameObjectManager.Remove(this);
        }

        public object Reconstruct()
        {
            return new TallGrass(Position, (TextureTypes)textureID);
        }
    }
}
