
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
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            origin: new Vector2(texture.Width / 2f, texture.Height / 2f),
            tag: "WorkBench",
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();

            InstructionTypes.Add(new InstructionType("craftcloth", "Cloth", "Craft cloth", onStart: CraftCloth));
            InstructionTypes.Add(new InstructionType("craftaxe", "Axe", "Craft axe", onStart: CraftAxe));
            InstructionTypes.Add(new InstructionType("craftbackpack", "Backpack", "Craft backpack", onStart: CraftBackPack));
            InstructionTypes.Add(new InstructionType("craftcoat", "Coat", "Craft coat", onStart: CraftCoat));
            InstructionTypes.Add(new InstructionType("craftbow", "Bow", "Craft bow", onStart: CraftBow));
            InstructionTypes.Add(new InstructionType("crafthoe", "Hoe", "Craft hoe", onStart: CraftHoe));
            InstructionTypes.Add(new InstructionType("craftpickaxe", "Pickaxe", "Craft pickaxe", onStart: CraftPickaxe));
            InstructionTypes.Add(new InstructionType("craftbasicrifle", "Basic Rifle", "Craft basic rifle", onStart: CraftBasicRifle));
        }

        //TODO Make the tier one crafting items be children of a common parent which can be used to reduce code.

        private void CraftCloth(Instruction instruction)
        {
            WorkBenchCrafter(instruction.ActiveMember, Craftable.Cloth);
        }

        private void CraftAxe(Instruction instruction)
        {
            WorkBenchCrafter(instruction.ActiveMember, Craftable.Axe);
        }

        private void CraftBackPack(Instruction instruction)
        {
            WorkBenchCrafter(instruction.ActiveMember, Craftable.Backpack);
        }

        private void CraftCoat(Instruction instruction)
        {
            WorkBenchCrafter(instruction.ActiveMember, Craftable.Coat);
        }

        private void CraftBow(Instruction instruction)
        {
            WorkBenchCrafter(instruction.ActiveMember, Craftable.Bow);
        }

        private void CraftHoe(Instruction instruction)
        {
            WorkBenchCrafter(instruction.ActiveMember, Craftable.Hoe);
        }

        private void CraftPickaxe(Instruction instruction)
        {
            WorkBenchCrafter(instruction.ActiveMember, Craftable.Pickaxe);
        }

        private void CraftBasicRifle(Instruction instruction)
        {
            WorkBenchCrafter(instruction.ActiveMember, Craftable.BasicRifle);
        }

        private void WorkBenchCrafter(IInstructionFollower follower, Craftable craftableEnum)
        {
            Colonist colonist = (Colonist) follower;

            CraftableType craftable = ResourceTypeFactory.GetCraftable(craftableEnum);
            if (colonist.Inventory.ContainsAll(craftable.CraftingCosts))
            {
                foreach (ResourceItem item in craftable.CraftingCosts)
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
