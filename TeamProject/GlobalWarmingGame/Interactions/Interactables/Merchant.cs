
using Engine;
using Engine.Drawing;
using Engine.PathFinding;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Priority_Queue;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Animals
{
    public class Merchant : PassiveAnimal
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
                        new ResourceItem(Resource.Food, 2)
                    }
                },
                {
                    Resource.MachineParts,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 6)
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
                        new ResourceItem(Resource.Food, 4)
                    }
                },
                {
                    Resource.Backpack,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 8)
                    }
                },
                {
                    Resource.Shotgun,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 12)
                    }
                },
                {
                    Resource.Bow,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Food, 8)
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
                        new ResourceItem(Resource.Food, 4)
                    }
                }
            };
        }

        private readonly Random rand;

        public Merchant(Vector2 position, TextureSetTypes textureSetType = TextureSetTypes.colonist) : base
        (
            position, "Merchant", Textures.MapSet[textureSetType], 0.05f, null
        )
        {
            Speed = 0.10f;

            rand = new Random();

            Resource r;
            switch (rand.Next(0,2))
            {
                //Merchant with weapon related goods
                case 0:
                    InstructionTypes.Add(new InstructionType((r = Resource.Leather).ToString(), "Buy Leather", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Cloth).ToString(), "Buy Cloth", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.MachineParts).ToString(), "Buy MachineParts", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Shotgun).ToString(), "Buy Shotgun", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Bow).ToString(), "Buy Bow", requiredResources: prices[r], onComplete: Trade));
                    break;

                //Merchant with tool related goods
                case 1:
                    InstructionTypes.Add(new InstructionType((r = Resource.Axe).ToString(), "Buy Axe", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Hoe).ToString(), "Buy Hoe", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Pickaxe).ToString(), "Buy Pickaxe", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Backpack).ToString(), "Buy Backpack", requiredResources: prices[r], onComplete: Trade));
                    InstructionTypes.Add(new InstructionType((r = Resource.Coat).ToString(), "Buy Coat", requiredResources: prices[r], onComplete: Trade));
                    break;
            }

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
    }
}
