using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    public class Tree : Sprite, IInteractable, IReconstructable
    {
        [PFSerializable]
        public bool _choppable;

        private readonly InstructionType chop;
        private readonly Texture2D textureTree;
        private readonly Texture2D textureStump;

        private bool Choppable
        {
            get { return _choppable; }
            set
            {
                _choppable = value;
                Texture = _choppable ? textureTree : textureStump;
            }
        }

        [PFSerializable]
        public new Vector2 Position { get; set; }

        [PFSerializable]
        public readonly int textureTreeID;

        [PFSerializable]
        public readonly int textureStumpID;

        public List<InstructionType> InstructionTypes { get; }

        public Tree() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public Tree(Vector2 position, TextureTypes textureTypeTree, TextureTypes textureTypeStump, bool choppable = true) : base
        (
            position: position,
            size: new Vector2(Textures.Map[textureTypeTree].Width, Textures.Map[textureTypeTree].Height),
            rotation: 0f,
            origin: new Vector2(Textures.Map[textureTypeTree].Width / 2f, Textures.Map[textureTypeTree].Height / 2f),
            tag: "Tree",
            texture: Textures.Map[textureTypeTree]
        )
        {
            Position = base.Position;
            textureTreeID = (int)textureTypeTree;
            textureStumpID = (int)textureTypeStump;

            InstructionTypes = new List<InstructionType>();
            this.textureTree = Textures.Map[textureTypeTree];
            this.textureStump = Textures.Map[textureTypeStump];

            Choppable = choppable;

            if (Choppable)
            {
                chop = new InstructionType("chop", "Chop", "Chop for wood", onStart: Chop);
                InstructionTypes.Add(chop);
            }
        }

        private void Chop(IInstructionFollower follower)
        {
            follower.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 4));
            Choppable = false;
            InstructionTypes.Remove(chop);
        }

        public object Reconstruct()
        {
            // System.Console.WriteLine(_choppable);
            return new Tree(Position, (TextureTypes)textureTreeID, (TextureTypes)textureStumpID, _choppable);
        }
    }
}
