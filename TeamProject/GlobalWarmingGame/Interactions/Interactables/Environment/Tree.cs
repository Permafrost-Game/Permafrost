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
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        [PFSerializable]
        public readonly int textureTreeID;

        [PFSerializable]
        public readonly int textureStumpID;

        public List<InstructionType> InstructionTypes { get; }

        public Tree() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public Tree(Vector2 position, TextureTypes textureTypeTree = TextureTypes.Tree, TextureTypes textureTypeStump = TextureTypes.TreeStump, bool choppable = true) : base
        (
            position: position,
            texture: Textures.Map[textureTypeTree]
        )
        {
            textureTreeID = (int)textureTypeTree;
            textureStumpID = (int)textureTypeStump;

            InstructionTypes = new List<InstructionType>();
            this.textureTree = Textures.Map[textureTypeTree];
            this.textureStump = Textures.Map[textureTypeStump];

            Choppable = choppable;

            if (Choppable)
            {
                chop = new InstructionType("chop", "Chop", "Chop for wood", 0,
                           new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.GetResource(Resource.Axe), 1) }, onStart: StartChop, onComplete: EndChop, timeCost: 3500f);
                InstructionTypes.Add(chop);
            }
        }

        private void StartChop(Instruction instruction)
        {
            SoundFactory.PlaySoundEffect(Sound.WoodChop);

        }
        private void EndChop(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 4));
            Choppable = false;
            InstructionTypes.Remove(chop);        
        }

        public object Reconstruct()
        {
            return new Tree(PFSPosition, (TextureTypes)textureTreeID, (TextureTypes)textureStumpID, _choppable);
        }
    }
}
