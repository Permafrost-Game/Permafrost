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
    public class Farm : Sprite, IInteractable, Engine.IUpdatable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.Wood, 8), 
                                                                                                   new ResourceItem(Resource.Hoe, 1)};

        public List<InstructionType> InstructionTypes { get; }

        private readonly InstructionType plant;
        private readonly InstructionType harvest;
        private bool growing;
        private float timeUntilGrown;
        private static readonly float growTime = 30000f;

        public Farm(Vector2 position, TextureTypes type = TextureTypes.Farm) : base
        (
            position: position,
            depth: CalculateDepth(position, -1),
            texture: Textures.Map[type]
        )
        {
            InstructionTypes = new List<InstructionType>();
            plant = new InstructionType(
                id: "plant",
                name: "Plant",
                description: "Plant",
                checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                timeCost: 4000f,
                onComplete: Plant
                );
            harvest = new InstructionType(
                id: "harvest",
                name: "Harvest",
                description: "Harvest",
                checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                timeCost: 4000f,
                onComplete: Harvest);

            timeUntilGrown = growTime;
            InstructionTypes.Add(plant);
        }

        private void Harvest(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Food, 20));

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
