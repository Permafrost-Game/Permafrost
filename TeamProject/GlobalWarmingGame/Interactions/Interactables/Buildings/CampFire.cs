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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public class CampFire : AnimatedSprite, IInteractable, IBuildable, IHeatSource
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 2), new ResourceItem(ResourceTypeFactory.GetResource(Resource.Fibers), 1) };
        public Temperature Temperature { get; set; } = new Temperature(50);
        public bool Heating { get; private set; }
        public List<InstructionType> InstructionTypes { get; }

        public CampFire(Vector2 position, Texture2D[][] textureSet) : base
        (
            position: position,
            textureSet: textureSet,
            frameTime: 50f
        )
        {
            Heating = true;

            TextureGroupIndex = 1;
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("fuel", "Fuel", "Fuel campfire", onComplete: Fuel)
            };
        }

        private void Fuel(Instruction instruction)
        {
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
