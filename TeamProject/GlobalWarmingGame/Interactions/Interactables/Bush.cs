
using Engine;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables
{
    class Bush : InteractableGameObject, IUpdatable
    {
        private bool _isHarvestable;

        private float timeToHarvestable = 3000f;
        private float timeUnitlHarvestable;

        private Texture2D textureHarvestable;
        private Texture2D textureHarvested;
        private bool IsHarvestable
        {
            get { return _isHarvestable; } 
            set
            {
                _isHarvestable = value;
                 texture = _isHarvestable ? textureHarvestable : textureHarvested;
            }
        }

        public Bush(Vector2 position, Texture2D harvestable, Texture2D harvested) : base
        (
            position: position,
            size: new Vector2(harvestable.Width, harvestable.Height),
            rotation: 0f,
            rotationOrigin: new Vector2(0, 0),
            tag: "Bush",
            depth: 0.7f,
            texture: harvestable,
            instructionTypes: new List<InstructionType>()
        )
        {
            
            this.textureHarvestable = harvestable;
            this.textureHarvested = harvested;
            IsHarvestable = true;
            InstructionTypes.Add(new InstructionType("forrage", "Forrage", "Forrage for berries", Forrage));
        }

        public void Forrage()
        {
            //This is tempory and should be replaced by the resource system
            if(IsHarvestable)
            {
                ((DisplayLabel)GameObjectManager.GetObjectsByTag("lblFood")[0]).Value += 1;
                IsHarvestable = false;
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
                    IsHarvestable = true;
                    
                }
            }
            
        }
    }
}
