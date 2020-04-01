
using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Animals
{
    public class Rabbit : PassiveAnimal, IReconstructable
    {
        private static readonly RandomAI RabbitAI = new RandomAI(63f, 64f);

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        [PFSerializable]
        public readonly int textureSetID;

        public Rabbit() : this(Vector2.Zero)
        {

        }

        public Rabbit(Vector2 position, TextureSetTypes textureSetType = TextureSetTypes.Rabbit) : base
        (
            position, "Rabbit", Textures.MapSet[textureSetType], 0.05f, RabbitAI, RabbitAI.MoveDistance * 3
        )
        {
            textureSetID = (int)textureSetType;

            this.InstructionTypes.Add(
                new InstructionType("hunt", "Hunt", "Hunt the Rabbit", onComplete: Hunt)
            );
        }

        public void Hunt(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Food, 4));
            Dispose();
            SoundFactory.PlaySoundEffect(Sound.RabbitDeath);
        }

        private void Dispose()
        {
            InstructionTypes.Clear();
            GameObjectManager.Remove(this);
        }

        public object Reconstruct()
        {
            return new Rabbit(PFSPosition, (TextureSetTypes)textureSetID);
        }
    }
}
