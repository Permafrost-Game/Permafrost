using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    public class TundraTree : Sprite, IInteractable, IReconstructable
    {
        [PFSerializable]
        public bool _choppable;

        private readonly InstructionType chop;
        private readonly Texture2D texture;

        private bool Choppable
        {
            get { return _choppable; }
            set
            {
                _choppable = value;
                Texture = texture;
            }
        }

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public List<InstructionType> InstructionTypes { get; }

        public TundraTree() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public TundraTree(Vector2 position, bool choppable = true) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.TundraTree]
        )
        {
            InstructionTypes = new List<InstructionType>();

            this.texture = Textures.Map[TextureTypes.TundraTree];

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
                    timeCost: 4000f
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
            Dispose();
        }

        private void Dispose()
        {
            GameObjectManager.Remove(this);
            this.InstructionTypes.Clear();
        }

        public object Reconstruct()
        {
            return new Tree(PFSPosition, _choppable);
        }
    }
}
