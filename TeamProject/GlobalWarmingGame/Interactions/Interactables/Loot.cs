using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Loot : Sprite, IInteractable, IReconstructable
    {
        public List<InstructionType> InstructionTypes { get; }

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        [PFSerializable]
        public List<ResourceItem> lootDrop;

        List<InstructionType> IInteractable.InstructionTypes => InstructionTypes;

        [PFSerializable]
        public readonly int textureID;


        public Loot() : base(Vector2.Zero, Vector2.Zero)
        {
            
            

        }

        public Loot(List<ResourceItem> loot,Vector2 position, TextureTypes textureType = TextureTypes.Loot) : base
        (
            position: position,
            texture: Textures.Map[textureType]
        )
        {
            textureID = (int)textureType;
            lootDrop = loot;
           InstructionTypes = new List<InstructionType>
            {
                new InstructionType(
                    id: "loot",
                    name: "Loot",
                    timeCost: 1000f,
                    checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                    onStart: StartLooting,
                    onComplete: EndLooting
                    )
            };
        }

        

        private void StartLooting(Instruction instruction)
        {
            //TODO Stone mine sound
            //SoundFactory.PlaySoundEffect(Sound.stone_mine);
        }

        private void EndLooting(Instruction instruction)
        {
            bool failedToAdd = false;
            List<ResourceItem> takenItems = new List<ResourceItem>();
            //SoundFactory.PlaySoundEffect(Sound.looting);
           
            foreach (ResourceItem item in lootDrop)
            {
                if (instruction.ActiveMember.Inventory.CanAddItem(item))
                {
                    instruction.ActiveMember.Inventory.AddItem(item);
                    takenItems.Add(item);
                    
                }
                else {
                    failedToAdd = true;
                }       
            }
            foreach (ResourceItem i in takenItems)
            {
                lootDrop.Remove(i);
            }
            if (!failedToAdd || lootDrop.Count==0)
            {
                Dispose();
            }
        }

        private void Dispose()
        {
            GameObjectManager.Remove(this);
            this.InstructionTypes.Clear();
        }

        public object Reconstruct()
        {
            return new Loot(lootDrop,PFSPosition, (TextureTypes)textureID);
        }
    }
}
