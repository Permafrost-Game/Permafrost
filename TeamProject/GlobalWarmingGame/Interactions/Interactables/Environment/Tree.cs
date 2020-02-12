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
        private InstructionType chop;
        private Texture2D textureTree;
        private Texture2D textureStump;
        private bool choppable
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
            rotationOrigin: new Vector2(0, 0),
            tag: "Tree",
            depth: 0.7f,
            texture: textureTree
        )
        {
            InstructionTypes = new List<InstructionType>();
            this.textureTree = textureTree;
            this.textureStump = textureStump;
            choppable = true;
            chop =new InstructionType("chop", "Chop", "Chop for wood", onStart: Chop);
            InstructionTypes.Add(chop);
        }

        private void Chop(IInstructionFollower follower)
        {
            follower.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.MakeResource(Resource.wood), 4));
            choppable = false;
            InstructionTypes.Remove(chop);
        }
    }
}
