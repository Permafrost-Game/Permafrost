
using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Environment
{
    public class Bush : Sprite, IInteractable, IUpdatable
    {
        private readonly InstructionType forrage;
        private bool _isHarvestable;

        private static readonly float timeToHarvestable = 6000f;
        private float timeUnitlHarvestable;

        private readonly Texture2D textureHarvestable;
        private readonly Texture2D textureHarvested;
        private bool IsHarvestable
        {
            get { return _isHarvestable; } 
            set
            {
                _isHarvestable = value;
                 Texture = _isHarvestable ? textureHarvestable : textureHarvested;
            }
        }

        public List<InstructionType> InstructionTypes { get; }

        public Bush(Vector2 position, Texture2D harvestable, Texture2D harvested) : base
        (
            position: position,
            size: new Vector2(harvestable.Width, harvestable.Height),
            rotation: 0f,
            origin: new Vector2(harvestable.Width / 2f, harvestable.Height / 2f),
            tag: "Bush",
            texture: harvestable
        )
        {
            InstructionTypes = new List<InstructionType>();
            forrage = new InstructionType("forrage", "Forrage", "Forrage for berries", onStart: Forrage);
            this.textureHarvestable = harvestable;
            this.textureHarvested = harvested;
            IsHarvestable = true;
            InstructionTypes.Add(forrage);
        }

        private void Forrage(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Food), 2));
            //This is tempory and should be replaced by the resource system
            if (IsHarvestable)
            {
                IsHarvestable = false;
                InstructionTypes.Remove(forrage);
                timeUnitlHarvestable = timeToHarvestable;
            }
        }

        public void Update(GameTime gameTime)
        {
            if(!IsHarvestable)
            {
                timeUnitlHarvestable -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeUnitlHarvestable <= 0f)
                {
                    InstructionTypes.Add(forrage);
                    IsHarvestable = true;
                }
            }
        }
    }
}
