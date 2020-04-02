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
    class Mill : Sprite, IInteractable, IBuildable
    {


        private static readonly Dictionary<Resource, List<ResourceItem>> millCrafting;
        static Mill()
        {
            millCrafting = new Dictionary<Resource, List<ResourceItem>>
            {
                {
                    Resource.Food,
                    new List<ResourceItem>()
                    {
                        new ResourceItem(Resource.Wheat, 4),
                    }
                }
            };
        }

        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.MachineParts, 1),
                                                                                                   new ResourceItem(Resource.Stone, 8),
                                                                                                   new ResourceItem(Resource.Wood, 16)};

        public List<InstructionType> InstructionTypes { get; }

        public Mill(Vector2 position, Texture2D texture) : base
        (
            position: position,
            texture: texture
        )
        {
            Resource r;
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType((r = Resource.Food).ToString(), "Refine wheat", requiredResources: millCrafting[r], onComplete: MillItem),
            };
        }

        private void MillItem(Instruction instruction)
        {
            Resource resource = (Resource)Enum.Parse(typeof(Resource), instruction.Type.ID);
            Colonist colonist = (Colonist)instruction.ActiveMember;

            if (colonist.Inventory.ContainsAll(instruction.Type.RequiredResources))
            {
                foreach (ResourceItem item in instruction.Type.RequiredResources)
                {
                    colonist.Inventory.RemoveItem(item);
                }
                colonist.Inventory.AddItem(new ResourceItem(resource, 4));
            }
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
