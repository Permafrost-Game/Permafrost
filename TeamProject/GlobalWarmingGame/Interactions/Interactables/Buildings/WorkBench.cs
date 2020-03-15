using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public class WorkBench : Sprite, IInteractable, IBuildable
    {

        private static readonly Dictionary<Resource, List<ResourceItem>> crafting;
        static WorkBench()
        {
            crafting = new Dictionary<Resource, List<ResourceItem>>
            {
                {
                    Resource.Axe,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Wood, 1),
                        new ResourceItem(Resource.Fibers, 2),
                        new ResourceItem(Resource.Stone, 1),
                    }
                },
                {
                    Resource.Hoe,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Wood, 1),
                        new ResourceItem(Resource.Fibers, 2),
                    }
                },
                {
                    Resource.Pickaxe,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Wood, 1),
                        new ResourceItem(Resource.Fibers, 2),
                        new ResourceItem(Resource.Stone, 2),
                    }
                },
                {
                    Resource.Backpack,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Cloth, 2),
                        new ResourceItem(Resource.Leather, 5),
                    }
                },
                {
                    Resource.BasicRifle,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Wood, 8),
                        new ResourceItem(Resource.Leather, 2),
                        new ResourceItem(Resource.MachineParts, 4),
                    }
                },
                {
                    Resource.Bow,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Wood, 4),
                        new ResourceItem(Resource.Fibers, 6),
                        new ResourceItem(Resource.Stone, 1),
                    }
                },
                {
                    Resource.Cloth,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Fibers, 4),
                    }
                },
                {
                    Resource.Coat,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Cloth, 4),
                        new ResourceItem(Resource.Leather, 2),
                    }
                }
            };
        }


        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.Stone, 4), new ResourceItem(Resource.Wood, 8)};

        public List<InstructionType> InstructionTypes { get; }



        public WorkBench(Vector2 position, Texture2D texture) : base
        (
            position: position,
            texture: texture
        )
        {
            Resource r;
            InstructionTypes = new List<InstructionType>
            {

                new InstructionType((r = Resource.Axe).ToString(), "Axe", requiredResources: crafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Hoe).ToString(), "Hoe", requiredResources: crafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Pickaxe).ToString(), "Pickaxe", requiredResources: crafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Backpack).ToString(), "Backpack", requiredResources: crafting[r], onComplete: Craft),
                new InstructionType((r = Resource.BasicRifle).ToString(), "Basic Rifle", requiredResources: crafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Bow).ToString(), "Bow", requiredResources: crafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Cloth).ToString(), "Cloth", requiredResources: crafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Coat).ToString(), "Coat", requiredResources: crafting[r], onComplete: Craft)
            };
        }

        private static void Craft(Instruction instruction)
        {
            Resource resource = (Resource) Enum.Parse(typeof(Resource), instruction.Type.ID);
            Colonist colonist = (Colonist) instruction.ActiveMember;

            if (colonist.Inventory.ContainsAll(instruction.Type.RequiredResources))
            {
                foreach (ResourceItem item in instruction.Type.RequiredResources)
                {
                    colonist.Inventory.RemoveItem(item);
                }
                colonist.Inventory.AddItem(new ResourceItem(resource, 1));
            }
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
