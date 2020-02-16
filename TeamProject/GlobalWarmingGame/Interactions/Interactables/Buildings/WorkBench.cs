
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
    public class WorkBench : Sprite, IInteractable, IBuildable, IUpdatable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.MakeResource(Resource.Stone), 4), new ResourceItem(ResourceTypeFactory.MakeResource(Resource.Wood), 8)};
        public Panel ResourceNotification { get; set; }

        public List<InstructionType> InstructionTypes { get; }

        MouseState currentMouseState;
        MouseState previousMouseState;

        public WorkBench(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "WorkBench",
            depth: 0.7f,
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();

            ResourceNotification = new Panel(new Vector2(175, 75), PanelSkin.Default, Anchor.TopCenter, new Vector2(0, 100));
            ResourceNotification.Padding = Vector2.Zero;
            ResourceNotification.Visible = false;

            UserInterface.Active.AddEntity(ResourceNotification);

            Label label = new Label("Not Enough Resources", Anchor.Center);
            ResourceNotification.AddChild(label);

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

        private void CraftCloth(IInstructionFollower follower)
        {
            WorkBenchCrafter(follower, Craftable.Cloth);
        }

        private void CraftAxe(IInstructionFollower follower)
        {
            WorkBenchCrafter(follower, Craftable.Axe);
        }

        private void CraftBackPack(IInstructionFollower follower)
        {
            WorkBenchCrafter(follower, Craftable.Backpack);
        }

        private void CraftCoat(IInstructionFollower follower)
        {
            WorkBenchCrafter(follower, Craftable.Coat);
        }

        private void CraftBow(IInstructionFollower follower)
        {
            WorkBenchCrafter(follower, Craftable.Bow);
        }

        private void CraftHoe(IInstructionFollower follower)
        {
            WorkBenchCrafter(follower, Craftable.Hoe);
        }

        private void CraftPickaxe(IInstructionFollower follower)
        {
            WorkBenchCrafter(follower, Craftable.Pickaxe);
        }

        private void CraftBasicRifle(IInstructionFollower follower)
        {
            WorkBenchCrafter(follower, Craftable.BasicRifle);
        }

        private void WorkBenchCrafter(IInstructionFollower follower, Craftable craftableEnum)
        {
            CraftableType craftable = ResourceTypeFactory.MakeCraftable(craftableEnum);
            if (follower.Inventory.CheckContainsList(craftable.CraftingCosts))
            {
                foreach (ResourceItem item in craftable.CraftingCosts)
                {
                    follower.Inventory.RemoveItem(item);
                    //Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                //Console.WriteLine("Added "+ craftable.ID + " amount: " + 1);
                follower.Inventory.AddItem(new ResourceItem(craftable, 1));
            }
            else
            {
                ResourceNotification.Visible = true;
            }
            //follower.Goals.Clear();
        }

        public void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();

            if (previousMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
            {
                if (ResourceNotification != null && ResourceNotification.Visible)
                    ResourceNotification.Visible = false;
            }

            previousMouseState = currentMouseState;
        }
    }
}
