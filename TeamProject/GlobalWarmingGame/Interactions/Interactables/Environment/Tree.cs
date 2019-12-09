
using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    class Tree : InteractableGameObject
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
                texture = _choppable ? textureTree : textureStump;
            }
        }

        public Tree(Vector2 position, Texture2D textureTree, Texture2D textureStump) : base
        (
            position: position,
            size: new Vector2(textureTree.Width, textureTree.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Tree",
            depth: 0.7f,
            texture: textureTree,
            instructionTypes: new List<InstructionType>()
        )
        {
            this.textureTree = textureTree;
            this.textureStump = textureStump;
            choppable = true;
            chop =new InstructionType("chop", "Chop", "Chop for wood", new ResourceItem(new Wood(), 4), Chop);
            InstructionTypes.Add(chop);
        }

        private void Chop(Colonist colonist)
        {
            //+Wood
            choppable = false;
            InstructionTypes.Remove(chop);
        }
    }
}
