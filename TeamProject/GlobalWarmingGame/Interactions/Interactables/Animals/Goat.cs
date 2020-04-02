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
    class Goat : PassiveAnimal, IReconstructable
    {
        private static readonly RandomAI GoatAI = new RandomAI(63f, 64f);

        #region PFSerializable
        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        [PFSerializable]
        public readonly int textureSetID;

        public Goat() : this(Vector2.Zero) { }
        #endregion
        public Goat(Vector2 position, TextureSetTypes textureSetType = TextureSetTypes.Goat) : base
        (
            position, Textures.MapSet[textureSetType], 0.05f, GoatAI, GoatAI.MoveDistance * 2
        )
        {
            textureSetID = (int)textureSetType;

            this.InstructionTypes.Add(
                new InstructionType("hunt", "Hunt", "Hunt the Goat", onComplete: Hunt)
            );
        }

        public void Hunt(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Food, 4));
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Leather, 1));
            Dispose();
            SoundFactory.PlaySoundEffect(Sound.GoatDeath);
        }

        private void Dispose()
        {
            InstructionTypes.Clear();
            GameObjectManager.Remove(this);
        }

        public object Reconstruct()
        {
            return new Goat(PFSPosition, (TextureSetTypes)textureSetID);
        }
    }
}
