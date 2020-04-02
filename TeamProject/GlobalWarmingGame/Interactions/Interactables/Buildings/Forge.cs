
using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public class Forge : Sprite, IInteractable, IBuildable
    {

        private static readonly Dictionary<Resource, List<ResourceItem>> forgeCrafting;
        static Forge()
        {
            forgeCrafting = new Dictionary<Resource, List<ResourceItem>>
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
                    Resource.IronIngot,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.IronOre, 1),
                        new ResourceItem(Resource.Coal, 2)
                    }
                },
                {
                    Resource.CombatKnife,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Stone, 2),
                        new ResourceItem(Resource.Fibers, 4),
                        new ResourceItem(Resource.IronIngot, 2)
                    }
                },
                {
                    Resource.MKIIShotgun,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Shotgun, 1),
                        new ResourceItem(Resource.RobotCore, 1),
                        new ResourceItem(Resource.MachineParts, 4),
                        new ResourceItem(Resource.IronIngot, 4)
                    }
                },
            };
        }

        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.MachineParts, 8),
                                                                                                   new ResourceItem(Resource.Stone, 6) };
        public List<InstructionType> InstructionTypes { get; }

        public Forge(Vector2 position, TextureTypes type = TextureTypes.Forge) : base
        (
            position: position,
            texture: Textures.Map[type]
        )
        {
            Resource r;
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType((r = Resource.Axe).ToString(), "Axe", requiredResources: forgeCrafting[r], onComplete: ForgeItem),
                new InstructionType((r = Resource.Hoe).ToString(), "Hoe", requiredResources: forgeCrafting[r], onComplete: ForgeItem),
                new InstructionType((r = Resource.Pickaxe).ToString(), "Pickaxe", requiredResources: forgeCrafting[r], onComplete: ForgeItem),
                new InstructionType((r = Resource.IronIngot).ToString(), "IronIngot", requiredResources: forgeCrafting[r], onComplete: ForgeItem),
                new InstructionType((r = Resource.CombatKnife).ToString(), "CombatKnife", requiredResources: forgeCrafting[r], onComplete: ForgeItem),
                new InstructionType((r = Resource.MKIIShotgun).ToString(), "MKIIShotgun", requiredResources: forgeCrafting[r], onComplete: ForgeItem)
            };
        }

        private void ForgeItem(Instruction instruction)
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

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
