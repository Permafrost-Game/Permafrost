
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    public class BigStoneNode : Sprite, IInteractable, IReconstructable
    {
        public List<InstructionType> InstructionTypes { get; }

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public BigStoneNode() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public BigStoneNode(Vector2 position) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.BigStoneNode]
        )
        {
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType(
                    id: "mine",
                    name: "Mine",
                    description: "Mine stone",
                    requiredResources: new List<ResourceItem>() {new ResourceItem(ResourceTypeFactory.GetResource(Resource.Pickaxe), 1)},
                    //checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type)
                    //                               && i.ActiveMember.Inventory.ContainsType(Resource.Pickaxe),
                    timeCost: 6000f,
                    onStart: StartMine,
                    onComplete: EndMine
                    )
            };
        }

        private void StartMine(Instruction instruction)
        {
            //TODO Stone mine sound
            //SoundFactory.PlaySoundEffect(Sound.stone_mine);
        }

        private void EndMine(Instruction instruction)
        {
            SoundFactory.PlaySoundEffect(Sound.StonePickup);
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Stone, 8));
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.IronOre, rand.Next(0,3)));
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Coal, rand.Next(0,6)));
            Dispose();
        }

        private void Dispose()
        {
            GameObjectManager.Remove(this);
            this.InstructionTypes.Clear();
        }

        public object Reconstruct()
        {
            return new BigStoneNode(PFSPosition);
        }
    }
}
