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

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public class Farm : Sprite, IInteractable, IUpdatable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 4)};

        public List<InstructionType> InstructionTypes { get; }

        private readonly InstructionType plant;
        private readonly InstructionType harvest;
        private bool growing;
        private float timeUntilGrown;
        private static readonly float growTime = 20000f;

        public Farm(Vector2 position, Texture2D texture) : base
        (
            position: position,
            depth: CalculateDepth(position, -1),
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
            plant = new InstructionType("plant", "Plant", "Plant", timeCost: 3000f, onComplete: Plant);
            harvest = new InstructionType("harvest", "Harvest", "Harvest", timeCost: 3000f, onComplete: Harvest);

            timeUntilGrown = 20000f;
            InstructionTypes.Add(plant);
        }

        private void Harvest(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Food), 10));

            InstructionTypes.Remove(harvest);
            InstructionTypes.Add(plant);
        }

        private void Plant(Instruction instruction)
        {
            InstructionTypes.Remove(plant);
            growing = true;
        }

        public void Update(GameTime gameTime)
        {
            if (growing == true)
            {
                timeUntilGrown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeUntilGrown < 0f)
                {
                    InstructionTypes.Add(harvest);
                    growing = false;
                    timeUntilGrown = growTime;
                }
            }
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
