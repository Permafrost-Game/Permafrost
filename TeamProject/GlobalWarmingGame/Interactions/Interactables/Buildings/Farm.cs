using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources.ResourceTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    class Farm : Sprite, IInteractable, IUpdatable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Wood(), 4)};

        public List<InstructionType> InstructionTypes { get; }

        private InstructionType plant;
        private InstructionType harvest;
        private bool growing;
        private float timeUntilGrown;
        private float growTime = 20000f;

        public Farm(Vector2 position, Texture2D texture) : base
        (
            position: position,
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Farm",
            depth: 0.7f,
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
            plant = new InstructionType("plant", "Plant", "Plant", Plant);
            harvest = new InstructionType("harvest", "Harvest", "Harvest", Harvest);
            timeUntilGrown = 20000f;
            InstructionTypes.Add(plant);
        }

        private void Harvest(IInstructionFollower follower)
        {
            follower.Inventory.AddItem(new ResourceItem(new Food(), 10));
            //Harvest wheat
            InstructionTypes.Remove(harvest);
            InstructionTypes.Add(plant);
        }

        private void Plant(IInstructionFollower follower)
        {
            //Plant wheat seeds
            InstructionTypes.Remove(plant);
            growing = true;
        }

        public void Update(GameTime gameTime)
        {
            //Grow crops over time
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
    }
}
