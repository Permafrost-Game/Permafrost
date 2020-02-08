
using Engine;
using Engine.Drawing;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources.Craftables;
using GlobalWarmingGame.Resources.ResourceTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class WorkBench : Sprite, IInteractable, IBuildable, IUpdatable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Stone(), 4), new ResourceItem(new Wood(), 8)};
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

            InstructionTypes.Add(new InstructionType("craftcloth", "Cloth", "Craft cloth", CraftCloth));
            InstructionTypes.Add(new InstructionType("craftaxe", "Axe", "Craft axe", CraftAxe));
            InstructionTypes.Add(new InstructionType("craftbackpack", "Backpack", "Craft backpack", CraftBackPack));
            InstructionTypes.Add(new InstructionType("craftcoat", "Coat", "Craft coat", CraftCoat));
            InstructionTypes.Add(new InstructionType("craftbow", "Bow", "Craft bow", CraftBow));
            InstructionTypes.Add(new InstructionType("crafthoe", "Hoe", "Craft hoe", CraftHoe));
            InstructionTypes.Add(new InstructionType("craftpickaxe", "Pickaxe", "Craft pickaxe", CraftPickaxe));
            InstructionTypes.Add(new InstructionType("craftbasicrifle", "Basic Rifle", "Craft basic rifle", CraftBasicRifle));
        }

        //TODO Make the tier one crafting items be children of a common parent which can be used to reduce code.

        private void CraftCloth(Colonist colonist)
        {
            Cloth cloth = new Cloth();
            if (colonist.Inventory.CheckContainsList(cloth.CraftingCosts))
            {
                foreach (ResourceItem item in cloth.CraftingCosts)
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added cloth" + " cloth: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(cloth, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            colonist.Goals.Clear();
        }

        private void CraftAxe(Colonist colonist)
        {
            Axe axe = new Axe();
            if (colonist.Inventory.CheckContainsList(axe.CraftingCosts))
            {
                foreach (ResourceItem item in axe.CraftingCosts) 
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added axe" + " amount: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(axe, 1));                
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            colonist.Goals.Clear();
        }

        private void CraftBackPack(Colonist colonist)
        {
            Backpack backpack = new Backpack();
            if (colonist.Inventory.CheckContainsList(backpack.CraftingCosts))
            {
                foreach (ResourceItem item in backpack.CraftingCosts)
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added backpack" + " amount: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(backpack, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            colonist.Goals.Clear();
        }

        private void CraftCoat(Colonist colonist)
        {
            Coat coat = new Coat();
            if (colonist.Inventory.CheckContainsList(coat.CraftingCosts))
            {
                foreach (ResourceItem item in coat.CraftingCosts)
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added coat" + " amount: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(coat, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            colonist.Goals.Clear();
        }

        private void CraftBow(Colonist colonist)
        {
            Bow bow = new Bow();
            if (colonist.Inventory.CheckContainsList(bow.CraftingCosts))
            {
                foreach (ResourceItem item in bow.CraftingCosts)
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added bow" + " amount: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(bow, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            colonist.Goals.Clear();
        }

        private void CraftHoe(Colonist colonist)
        {
            Hoe hoe = new Hoe();
            if (colonist.Inventory.CheckContainsList(hoe.CraftingCosts))
            {
                foreach (ResourceItem item in hoe.CraftingCosts)
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added hoe" + " amount: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(hoe, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            colonist.Goals.Clear();
        }

        private void CraftPickaxe(Colonist colonist)
        {
            Pickaxe pickaxe = new Pickaxe();
            if (colonist.Inventory.CheckContainsList(pickaxe.CraftingCosts))
            {
                foreach (ResourceItem item in pickaxe.CraftingCosts)
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added pickaxe" + " amount: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(pickaxe, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            colonist.Goals.Clear();
        }

        private void CraftBasicRifle(Colonist colonist)
        {
            BasicRifle basicRifle = new BasicRifle();
            if (colonist.Inventory.CheckContainsList(basicRifle.CraftingCosts))
            {
                foreach (ResourceItem item in basicRifle.CraftingCosts)
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added BasicRifle" + " amount: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(basicRifle, 1));
            }

            else
            {
                ResourceNotification.Visible = true;
            }
            colonist.Goals.Clear();
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
