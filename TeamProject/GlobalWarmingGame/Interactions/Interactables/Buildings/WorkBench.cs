using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public class WorkBench : Sprite, IInteractable, IBuildable, IReconstructable
    {

        private static readonly Dictionary<Resource, List<ResourceItem>> workbenchCrafting;

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        static WorkBench()
        {
            workbenchCrafting = new Dictionary<Resource, List<ResourceItem>>
            {
                {
                    Resource.Cloth,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Fibers, 4),
                    }
                },
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
                        new ResourceItem(Resource.Stone, 1),
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
                    Resource.Coat,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Cloth, 4),
                        new ResourceItem(Resource.Leather, 2),
                    }
                },
                {
                    Resource.ThickCoat,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Coat, 1),
                        new ResourceItem(Resource.Cloth, 2),
                        new ResourceItem(Resource.Leather, 2)
                    }
                },
                {
                    Resource.Shotgun,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Wood, 8),
                        new ResourceItem(Resource.Leather, 2),
                        new ResourceItem(Resource.MachineParts, 4),
                    }
                },
            };
        }


        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.Stone, 8),
                                                                                                   new ResourceItem(Resource.Wood, 8)};

        public List<InstructionType> InstructionTypes { get; }


        public WorkBench() : base(Vector2.Zero, Textures.Map[TextureTypes.WorkBench])
        {

        }

        public WorkBench(Vector2 position) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.WorkBench]
        )
        {
            Resource r;
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType((r = Resource.Cloth).ToString(), "Cloth", requiredResources: workbenchCrafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Axe).ToString(), "Axe", requiredResources: workbenchCrafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Hoe).ToString(), "Hoe", requiredResources: workbenchCrafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Pickaxe).ToString(), "Pickaxe", requiredResources: workbenchCrafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Coat).ToString(), "Coat", requiredResources: workbenchCrafting[r], onComplete: Craft),
                new InstructionType((r = Resource.ThickCoat).ToString(), "ThickCoat", requiredResources: workbenchCrafting[r], onComplete: Craft),
                new InstructionType((r = Resource.Shotgun).ToString(), "Shotgun", requiredResources: workbenchCrafting[r], onComplete: Craft),
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

        public object Reconstruct()
        {
            return new WorkBench(PFSPosition);
        }
    }
}
