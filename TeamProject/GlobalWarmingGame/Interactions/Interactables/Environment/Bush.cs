
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
    public class Bush : Sprite, IInteractable, IUpdatable, IReconstructable
    {
        private readonly InstructionType forrage;

        [PFSerializable]
        public bool _isHarvestable;

        private static readonly float timeToHarvestable = 6000f;

        [PFSerializable]
        public float timeUnitlHarvestable;

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

        [PFSerializable]
        public new Vector2 Position { get; set; }

        [PFSerializable]
        public readonly int textureHarvestableID;

        [PFSerializable]
        public readonly int textureHarvestedID;

        public Bush() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public Bush(Vector2 position, TextureTypes textureTypeHarvestable, TextureTypes textureTypeHarvested, bool isHarvestable = true, float timeUnitlHarvestable = 0) : base
        (
            position: position,
            size: new Vector2(Textures.Map[textureTypeHarvestable].Width, Textures.Map[textureTypeHarvestable].Height),
            rotation: 0f,
            origin: new Vector2(Textures.Map[textureTypeHarvestable].Width / 2f, Textures.Map[textureTypeHarvestable].Height / 2f),
            tag: "Bush",
            texture: Textures.Map[textureTypeHarvestable]
        )
        {
            Position = base.Position;
            textureHarvestableID = (int)textureTypeHarvestable;
            textureHarvestedID = (int)textureTypeHarvested;

            InstructionTypes = new List<InstructionType>();
            forrage = new InstructionType("forrage", "Forrage", "Forrage for berries", onComplete: Forrage);
            this.textureHarvestable = Textures.Map[textureTypeHarvestable];
            this.textureHarvested = Textures.Map[textureTypeHarvested];

            InstructionTypes.Add(new InstructionType("chop", "Chop", "Chop for wood", onComplete: Chop));

            if (IsHarvestable = isHarvestable)
                InstructionTypes.Add(forrage);
            else
                this.timeUnitlHarvestable = timeUnitlHarvestable;
        }

        private void Chop(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(ResourceTypeFactory.GetResource(Resource.Wood), 1));
            GameObjectManager.Remove(this);
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

        public object Reconstruct()
        {
            return new Bush(Position, (TextureTypes)textureHarvestableID, (TextureTypes)textureHarvestedID, _isHarvestable, timeUnitlHarvestable);
        }
    }
}
