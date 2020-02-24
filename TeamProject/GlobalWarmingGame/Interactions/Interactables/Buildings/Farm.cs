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
            size: new Vector2(texture.Width, texture.Height),
            rotation: 0f,
            origin: new Vector2(texture.Width / 2f, texture.Height / 2f),
            tag: "Farm",
            texture: texture
        )
        {
            InstructionTypes = new List<InstructionType>();
            plant = new InstructionType("plant", "Plant", "Plant", onStart: Plant);
            harvest = new InstructionType("harvest", "Harvest", "Harvest", onStart: Harvest);
            timeUntilGrown = 20000f;
            InstructionTypes.Add(plant);
        }

        private void Harvest(IInstructionFollower follower)
        {
            follower.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Food), 10));
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

        public void Build()
        {
            GameObjectManager.Add(this);
        }
    }
}
