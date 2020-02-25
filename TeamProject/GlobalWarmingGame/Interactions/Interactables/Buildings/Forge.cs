
using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class Forge : Sprite, IInteractable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.GetResource(Resource.MachineParts), 10),
                                                                                                   new ResourceItem(ResourceTypeFactory.GetResource(Resource.Stone), 6) };
        public List<InstructionType> InstructionTypes { get; }

        public Forge(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            origin: new Vector2(texture.Width / 2f, texture.Height / 2f),
            tag: "Forge",
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("forge", "Forge", "Forge iron item", onStart: ForgeItem)
            };
        }

        private void ForgeItem(Instruction instruction)
        {
            //Open craft menu
            //Force the colonist to wait at the station until job is done
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }

        //Other methods for selected crafting recipe
    }
}
