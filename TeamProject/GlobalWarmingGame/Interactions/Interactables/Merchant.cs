using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Animals
{
    public class Merchant : PassiveAnimal, IReconstructable
    {
        private static readonly Dictionary<Resource, List<ResourceItem>> prices;
        static Merchant()
        {
            //The price in food for each item the merchant has
            prices = new Dictionary<Resource, List<ResourceItem>>
            {
                {
                    Resource.Leather,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 4)
                    }
                },
                {
                    Resource.MachineParts,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 4)
                    }
                },
                {
                    Resource.Axe,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 4)
                    }
                },
                {
                    Resource.Hoe,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 4)
                    }
                },
                {
                    Resource.Pickaxe,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 5)
                    }
                },
                {
                    Resource.Shotgun,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 24)
                    }
                },
                {
                    Resource.Cloth,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 2)
                    }
                },
                {
                    Resource.Coat,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 8)
                    }
                },
                /*{
                    Resource.CombatKnife,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 1)
                    }
                },
                {
                    Resource.MKIIShotgun,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 1)
                    }
                }*/
            };
        }

        private static readonly Random rand = new Random();

        #region PFSerializable
        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }
        public Merchant() : base(Vector2.Zero, Textures.MapSet[TextureSetTypes.Merchent], 0f, null) { }
        #endregion
        
        public Merchant(Vector2 position, TextureSetTypes textureSetType = TextureSetTypes.Merchent) : base
        (
            position, Textures.MapSet[textureSetType], 0.05f, null
        )
        {
            Speed = 0.10f;

            Resource r;
            switch (rand.Next(0,2))
            {
                //Merchant with weapon related goods
                case 0:
                    InstructionTypes.Add(new InstructionType((r = Resource.Leather).ToString(), "Buy Leather", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Cloth).ToString(), "Buy Cloth", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.MachineParts).ToString(), "Buy MachineParts", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Shotgun).ToString(), "Buy Shotgun", requiredResources: prices[r], onComplete: Trade));
                    break;

                //Merchant with tool related goods
                case 1:
                    InstructionTypes.Add(new InstructionType((r = Resource.Axe).ToString(), "Buy Axe", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Hoe).ToString(), "Buy Hoe", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Pickaxe).ToString(), "Buy Pickaxe", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Coat).ToString(), "Buy Coat", requiredResources: prices[r], onComplete: Trade));
                    break;
            }

            //InstructionTypes.Add(new InstructionType((r = Resource.CombatKnife).ToString(), "CombatKnife", requiredResources: prices[r], onComplete: Trade));
            //InstructionTypes.Add(new InstructionType((r = Resource.MKIIShotgun).ToString(), "MKIIShotgun", requiredResources: prices[r], onComplete: Trade));

        }

        public void Trade(Instruction instruction)
        {
            Resource resource = (Resource)Enum.Parse(typeof(Resource), instruction.Type.ID);
            Colonist colonist = (Colonist)instruction.ActiveMember;

            if (colonist.Inventory.ContainsAll(instruction.Type.RequiredResources))
            {
                foreach (ResourceItem item in instruction.Type.RequiredResources)
                {
                    colonist.Inventory.RemoveItem(item);
                }
                colonist.Inventory.AddItem(new ResourceItem(resource, 1));
            }
        }

        public object Reconstruct()
        {
            return new Merchant(PFSPosition);
        }
    }
}
