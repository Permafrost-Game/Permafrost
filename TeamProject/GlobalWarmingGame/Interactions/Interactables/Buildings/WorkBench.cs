
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
    class WorkBench : Sprite, IInteractable, IBuildable, IUpdatable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.MakeResource(Resource.stone), 4), new ResourceItem(ResourceTypeFactory.MakeResource(Resource.wood), 8)};
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
            CraftableType cloth = ResourceTypeFactory.MakeCraftable(Craft.cloth);
            if (follower.Inventory.CheckContainsList(cloth.CraftingCosts))
            {
                foreach (ResourceItem item in cloth.CraftingCosts)
                {
                    follower.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added cloth" + " cloth: " + 1);
                follower.Inventory.AddItem(new ResourceItem(cloth, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            //follower.Goals.Clear();
        }

        private void CraftAxe(IInstructionFollower follower)
        {
            CraftableType axe = ResourceTypeFactory.MakeCraftable(Craft.axe);
            if (follower.Inventory.CheckContainsList(axe.CraftingCosts))
            {
                foreach (ResourceItem item in axe.CraftingCosts) 
                {
                    follower.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added axe" + " amount: " + 1);
                follower.Inventory.AddItem(new ResourceItem(axe, 1));                
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            //follower.Goals.Clear();
        }

        private void CraftBackPack(IInstructionFollower follower)
        {
            CraftableType backpack = ResourceTypeFactory.MakeCraftable(Craft.backpack);
            if (follower.Inventory.CheckContainsList(backpack.CraftingCosts))
            {
                foreach (ResourceItem item in backpack.CraftingCosts)
                {
                    follower.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added backpack" + " amount: " + 1);
                follower.Inventory.AddItem(new ResourceItem(backpack, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            //follower.Goals.Clear();
        }

        private void CraftCoat(IInstructionFollower follower)
        {
            CraftableType coat = ResourceTypeFactory.MakeCraftable(Craft.coat);
            if (follower.Inventory.CheckContainsList(coat.CraftingCosts))
            {
                foreach (ResourceItem item in coat.CraftingCosts)
                {
                    follower.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added coat" + " amount: " + 1);
                follower.Inventory.AddItem(new ResourceItem(coat, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            //follower.Goals.Clear();
        }

        private void CraftBow(IInstructionFollower follower)
        {
            CraftableType bow = ResourceTypeFactory.MakeCraftable(Craft.bow);
            if (follower.Inventory.CheckContainsList(bow.CraftingCosts))
            {
                foreach (ResourceItem item in bow.CraftingCosts)
                {
                    follower.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added bow" + " amount: " + 1);
                follower.Inventory.AddItem(new ResourceItem(bow, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            //follower.Goals.Clear();
        }

        private void CraftHoe(IInstructionFollower follower)
        {
            CraftableType hoe = ResourceTypeFactory.MakeCraftable(Craft.hoe);
            if (follower.Inventory.CheckContainsList(hoe.CraftingCosts))
            {
                foreach (ResourceItem item in hoe.CraftingCosts)
                {
                    follower.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added hoe" + " amount: " + 1);
                follower.Inventory.AddItem(new ResourceItem(hoe, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            //follower.Goals.Clear();
        }

        private void CraftPickaxe(IInstructionFollower follower)
        {
            CraftableType pickaxe = ResourceTypeFactory.MakeCraftable(Craft.pickaxe);
            if (follower.Inventory.CheckContainsList(pickaxe.CraftingCosts))
            {
                foreach (ResourceItem item in pickaxe.CraftingCosts)
                {
                    follower.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added pickaxe" + " amount: " + 1);
                follower.Inventory.AddItem(new ResourceItem(pickaxe, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            //follower.Goals.Clear();
        }

        private void CraftBasicRifle(IInstructionFollower follower)
        {
            CraftableType basicRifle = ResourceTypeFactory.MakeCraftable(Craft.basicRifle);
            if (follower.Inventory.CheckContainsList(basicRifle.CraftingCosts))
            {
                foreach (ResourceItem item in basicRifle.CraftingCosts)
                {
                    follower.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added BasicRifle" + " amount: " + 1);
                follower.Inventory.AddItem(new ResourceItem(basicRifle, 1));
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
