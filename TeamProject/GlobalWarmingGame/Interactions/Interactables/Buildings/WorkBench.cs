
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources.Craftables;
using GlobalWarmingGame.Resources.ResourceTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class WorkBench : InteractableGameObject, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Stone(), 4),
                                                                                                   new ResourceItem(new Wood(), 8)};

        public WorkBench(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "WorkBench",
            depth: 0.7f,
            texture: texture,
            instructionTypes: new List<InstructionType>() { }
        )
        {
            InstructionTypes.Add(new InstructionType("craft axe", "Craft axe", "Craft axe", CraftAxe));
        }

        private void CraftAxe(Colonist colonist)
        {
            if (colonist.Inventory.CheckContainsList(CraftingCosts))
            {
                foreach (ResourceItem item in CraftingCosts) 
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                Console.WriteLine("Added axe" + " amount: " + 1);
                colonist.Inventory.AddItem(new ResourceItem(new Axe(), 1));                
            }
            else
            {
                Console.WriteLine("failed");
            }
        }

        //Other methods for selected crafting recipe
    }
}
