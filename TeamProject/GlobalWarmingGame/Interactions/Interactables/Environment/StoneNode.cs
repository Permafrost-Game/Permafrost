using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    public class StoneNode : Sprite, IInteractable, IReconstructable
    {
        public List<InstructionType> InstructionTypes { get; }

        [PFSerializable]
        public new Vector2 Position { get; set; }

        [PFSerializable]
        public readonly int textureID;

        public StoneNode() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public StoneNode(Vector2 position, TextureTypes textureType) : base
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
                new InstructionType("mine", "Mine", "Mine stone", onStart: Mine)
            };
        }

        private void Mine(IInstructionFollower follower)
        {
            follower.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 4));
            //Maybe destory the node or allow 3 more mine operations
            GameObjectManager.Remove(this);
        }

        public object Reconstruct()
        {
            return new StoneNode(Position, (TextureTypes)textureID);
        }
    }
}