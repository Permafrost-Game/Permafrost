
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
        private readonly Dictionary<Resource, List<ResourceItem>> prices;

        public Merchant(Vector2 position, TextureSetTypes textureSetType = TextureSetTypes.colonist) : base
        (
            position, "Merchant", Textures.MapSet[textureSetType], 0.05f, null
        )
        {
            Speed = 0.10f;

            //How much food for each item the Merchant wants
            prices = new Dictionary<Resource, List<ResourceItem>>
            {
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
                    Resource.BasicRifle,
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

            Resource r;
            InstructionTypes.Add(new InstructionType((r = Resource.Axe).ToString(), "Buy Axe", requiredResources: prices[r], onComplete: Trade));
            InstructionTypes.Add(new InstructionType((r = Resource.Hoe).ToString(), "Buy Hoe", requiredResources: prices[r], onComplete: Trade));
            InstructionTypes.Add(new InstructionType((r = Resource.Pickaxe).ToString(), "Buy Pickaxe", requiredResources: prices[r], onComplete: Trade));
            InstructionTypes.Add(new InstructionType((r = Resource.Backpack).ToString(), "Buy Backpack", requiredResources: prices[r], onComplete: Trade));
            InstructionTypes.Add(new InstructionType((r = Resource.BasicRifle).ToString(), "Buy Basic Rifle", requiredResources: prices[r], onComplete: Trade));
            InstructionTypes.Add(new InstructionType((r = Resource.Bow).ToString(), "Buy Bow", requiredResources: prices[r], onComplete: Trade));
            InstructionTypes.Add(new InstructionType((r = Resource.Cloth).ToString(), "Buy Cloth", requiredResources: prices[r], onComplete: Trade));
            InstructionTypes.Add(new InstructionType((r = Resource.Coat).ToString(), "Buy Coat", requiredResources: prices[r], onComplete: Trade));
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
