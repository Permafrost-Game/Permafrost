
using Engine;
using Engine.Drawing;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public class WorkBench : Sprite, IInteractable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 4), new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 8)};
        public Panel ResourceNotification { get; set; }

        public List<InstructionType> InstructionTypes { get; }

        public WorkBench(Vector2 position, Texture2D texture) : base
        (
            position: position,
            texture: texture
        )
        {
            #region Teir 1 crafting costs
            //Axe
            List<ResourceItem> AxeCraftingCosts = new List<ResourceItem>() {
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 1),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Fibers), 2),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 1),
            };

            //Hoe
            List<ResourceItem> HoeCraftingCosts = new List<ResourceItem>() {
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 1),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Fibers), 2),
            };

            //Pickaxe
            List<ResourceItem> PickaxeCosts = new List<ResourceItem>() {
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 1),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Fibers), 2),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 2),
            };

            //Backpack
            List<ResourceItem> BackpackCraftingCosts = new List<ResourceItem>() {
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Cloth), 2),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Leather), 5),
            };

            //BasicRifle
            List<ResourceItem> BasicRifleCraftingCosts = new List<ResourceItem>() {
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 8),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Leather), 2),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.MachineParts), 4),
            };

            //Bow
            List<ResourceItem> BowCraftingCosts = new List<ResourceItem>() {
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 4),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Fibers), 6),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 1),
            };

            //Cloth
            List<ResourceItem> ClothCraftingCosts = new List<ResourceItem>() {
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Fibers), 4),
            };

            //Coat
            List<ResourceItem> CoatCraftingCosts = new List<ResourceItem>() {
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Cloth), 4),
                new ResourceItem(ResourceTypeFactory.GetResource(Resource.Leather), 2),
            };
            #endregion

            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("craftaxe", "Axe", "Craft axe", 0, requiredResources: AxeCraftingCosts, onComplete: CraftAxe),
                new InstructionType("crafthoe", "Hoe", "Craft hoe", 0, requiredResources: HoeCraftingCosts, onComplete: CraftHoe),
                new InstructionType("craftpickaxe", "Pickaxe", "Craft pickaxe", 0, requiredResources: PickaxeCosts, onComplete: CraftPickaxe),
                new InstructionType("craftbackpack", "Backpack", "Craft backpack", 0, requiredResources: BackpackCraftingCosts, onComplete: CraftBackPack),
                new InstructionType("craftbasicrifle", "Basic Rifle", "Craft basic rifle", 0, requiredResources: BasicRifleCraftingCosts, onComplete: CraftBasicRifle),
                new InstructionType("craftbow", "Bow", "Craft bow", 0, requiredResources: BowCraftingCosts, onComplete: CraftBow),
                new InstructionType("craftcloth", "Cloth", "Craft cloth", 0, requiredResources: ClothCraftingCosts, onComplete: CraftCloth),
                new InstructionType("craftcoat", "Coat", "Craft coat", 0, requiredResources: CoatCraftingCosts, onComplete: CraftCoat)
            };
        }

        //TODO Make the tier one crafting items be children of a common parent which can be used to reduce code.

        private void CraftCloth(Instruction instruction)
        {
            WorkBenchCrafter(instruction, Resource.Cloth);
        }

        private void CraftAxe(Instruction instruction)
        {
            WorkBenchCrafter(instruction, Resource.Axe);
        }

        private void CraftBackPack(Instruction instruction)
        {
            WorkBenchCrafter(instruction, Resource.Backpack);
        }

        private void CraftCoat(Instruction instruction)
        {
            WorkBenchCrafter(instruction, Resource.Coat);
        }

        private void CraftBow(Instruction instruction)
        {
            WorkBenchCrafter(instruction, Resource.Bow);
        }

        private void CraftHoe(Instruction instruction)
        {
            WorkBenchCrafter(instruction, Resource.Hoe);
        }

        private void CraftPickaxe(Instruction instruction)
        {
            WorkBenchCrafter(instruction, Resource.Pickaxe);
        }

        private void CraftBasicRifle(Instruction instruction)
        {
            WorkBenchCrafter(instruction, Resource.BasicRifle);
        }

        private void WorkBenchCrafter(Instruction instruction, Resource resourceEnum)
        {
            Colonist colonist = (Colonist) instruction.ActiveMember;

            ResourceType craftable = ResourceTypeFactory.GetResource(resourceEnum);
            if (colonist.Inventory.ContainsAll(instruction.Type.RequiredResources))
            {
                foreach (ResourceItem item in instruction.Type.RequiredResources)
                {
                    colonist.Inventory.RemoveItem(item);
                    //Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                //Console.WriteLine("Added "+ craftable.ID + " amount: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(craftable, 1));
            }
            else
            {
                //ResourceNotification.Visible = true;
            }
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
