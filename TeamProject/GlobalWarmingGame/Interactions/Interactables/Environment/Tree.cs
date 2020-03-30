﻿using Engine;
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

        public List<InstructionType> InstructionTypes { get; }

        public Tree() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public Tree(Vector2 position, bool choppable = true) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.Tree]
        )
        {
            InstructionTypes = new List<InstructionType>();

            this.textureTree = Textures.Map[TextureTypes.Tree];
            this.textureStump = Textures.Map[TextureTypes.TreeStump];

            Choppable = choppable;

            if (Choppable)
            {
                chop = new InstructionType(
                    id: "chop",
                    name: "Chop",
                    description: "Chop for wood",
                    requiredResources: new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.GetResource(Resource.Axe), 1) },
                    //checkValidity: (Instruction i) => i.ActiveMember.Inventory.ContainsType(Resource.Axe),
                    onStart: StartChop,
                    onComplete: EndChop,
                    timeCost: 3500f
                    );
                InstructionTypes.Add(chop);
            }
        }

        private void StartChop(Instruction instruction)
        {
            SoundFactory.PlaySoundEffect(Sound.WoodChop);

        }
        private void EndChop(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Wood, 4));
            Choppable = false;
            InstructionTypes.Remove(chop);
        }

        public object Reconstruct()
        {
            return new Tree(PFSPosition, _choppable);
        }
    }
}
