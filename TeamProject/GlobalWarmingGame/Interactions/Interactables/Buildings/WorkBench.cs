
using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class WorkBench : InteractableGameObject, IBuildable
    {
        private Inventory inventory;
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Stone(), 4),
                                                                                                   new ResourceItem(new Wood(), 8)};
        public Temperature Temperature { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        Vector2 IBuildable.Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        Vector2 IBuildable.Size { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
            inventory = new Inventory(10);
            InstructionTypes.Add(new InstructionType("craft", "Craft", "Craft items", CraftItem));
        }

        public void CraftItem(Colonist colonist)
        {
            //Open craft menu
            //Force the colonist to wait at the station until job is done
        }

        //Other methods for selected crafting recipe
    }
}
