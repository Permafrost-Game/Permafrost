using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Animals
{
    class Fox : PassiveAnimal, IReconstructable
    {
        private static readonly RandomAI FoxAI = new RandomAI(63f, 64f);

        #region PFSerializable
        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        [PFSerializable]
        public readonly int textureSetID;

        public Fox() : this(Vector2.Zero) { }
        #endregion
        public Fox(Vector2 position, TextureSetTypes textureSetType = TextureSetTypes.Fox) : base
        (
            position, Textures.MapSet[textureSetType], 0.05f, FoxAI, FoxAI.MoveDistance * 3
        )
        {
            textureSetID = (int)textureSetType;

            this.InstructionTypes.Add(
                new InstructionType("hunt", "Hunt", "Hunt the Fox", onComplete: Hunt)
            );
        }

        public void Hunt(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Food, 4));
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Leather, 1));
            Dispose();
            SoundFactory.PlaySoundEffect(Sound.FoxDeath);
        }

        private void Dispose()
        {
            InstructionTypes.Clear();
            GameObjectManager.Remove(this);
        }

        public object Reconstruct()
        {
            return new Fox(PFSPosition, (TextureSetTypes)textureSetID);
        }
    }
}
