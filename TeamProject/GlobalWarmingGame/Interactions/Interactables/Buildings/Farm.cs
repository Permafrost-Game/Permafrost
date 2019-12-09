
using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Farm : InteractableGameObject, IUpdatable, IBuildable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(new Wood(), 4)};
        public Temperature Temperature { get; set; } = new Temperature(10);
        public new Vector2 Position { get; set; }
        public new Vector2 Size { get; set; }

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
            texture: texture,
            instructionTypes: new List<InstructionType>() { }
        )
        {
            Position = position;
            Size = new Vector2(texture.Width, texture.Height);
            plant = new InstructionType("plant", "Plant", "Plant", Plant);
            harvest = new InstructionType("harvest", "Harvest", "Harvest", new ResourceItem(new Food(), 10), Harvest);
            timeUntilGrown = 20000f;
            InstructionTypes.Add(plant);
        }

        private void Harvest(Colonist colonist)
        {
            //Harvest wheat
            InstructionTypes.Remove(harvest);
            InstructionTypes.Add(plant);
        }

        private void Plant(Colonist colonist)
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
