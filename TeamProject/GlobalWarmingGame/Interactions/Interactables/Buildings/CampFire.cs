using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class CampFire : InteractableGameObject, IBuildable, IHeatable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Wood(), 4), new ResourceItem(new Fibers(), 2) };
        public Temperature Temperature { get; set; } = new Temperature(50);
        public new Vector2 Position { get; private set; }
        public new Vector2 Size { get; private set; }
        public bool Heating { get; private set; }

        public CampFire(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "CampFire",
            depth: 0.7f,
            texture: texture,
            instructionTypes: new List<InstructionType>() { }
        )
        {
            Position = position;
            Size = new Vector2(texture.Width, texture.Height);
            Heating = true;
            InstructionTypes.Add(new InstructionType("fuel", "Fuel", "Fuel campfire", Fuel));
        }

        private void Fuel(Colonist colonist)
        {
        }
    }
}
