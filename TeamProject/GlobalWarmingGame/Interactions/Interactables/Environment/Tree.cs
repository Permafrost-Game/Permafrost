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
    public class Tree : Sprite, IInteractable
    {
        private bool _choppable;
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

        public List<InstructionType> InstructionTypes { get; }

        public Tree(Vector2 position, Texture2D textureTree, Texture2D textureStump) : base
        (
            position: position,
            size: new Vector2(textureTree.Width, textureTree.Height),
            rotation: 0f,
            origin: new Vector2(textureTree.Width / 2f, textureTree.Height / 2f),
            tag: "Tree",
            texture: textureTree
        )
        {
            InstructionTypes = new List<InstructionType>();
            this.textureTree = textureTree;
            this.textureStump = textureStump;
            Choppable = true;
            chop =new InstructionType("chop", "Chop", "Chop for wood", onStart: Chop);
            InstructionTypes.Add(chop);
        }

        private void Chop(IInstructionFollower follower)
        {
            follower.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 4));
            Choppable = false;
            InstructionTypes.Remove(chop);
        }
    }
}
