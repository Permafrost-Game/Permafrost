
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
    public class Bush : Sprite, IInteractable, Engine.IUpdatable, IReconstructable
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

        #region PFSerializable
        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        [PFSerializable]
        public readonly int textureHarvestableID;

        [PFSerializable]
        public readonly int textureHarvestedID;
        #endregion
        public Bush() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public Bush(Vector2 position, TextureTypes textureTypeHarvestable = TextureTypes.BushH, TextureTypes textureTypeHarvested = TextureTypes.BushN, bool isHarvestable = true, float timeUnitlHarvestable = 0) : base
        (
            position: position,
            texture: Textures.Map[textureTypeHarvestable]
        )
        {
            textureHarvestableID = (int)textureTypeHarvestable;
            textureHarvestedID = (int)textureTypeHarvested;

            InstructionTypes = new List<InstructionType>();
            
            this.textureHarvestable = Textures.Map[textureTypeHarvestable];
            this.textureHarvested = Textures.Map[textureTypeHarvested];

            forrage = new InstructionType(
                id: "forrage",
                name: "Forrage",
                checkValidity: (Instruction i) => IsHarvestable,
                onComplete: Forrage
                );
            InstructionTypes.Add(new InstructionType(
                id: "chop",
                name: "Chop",
                checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                onComplete: Chop)
                );

            if (IsHarvestable = isHarvestable)
                InstructionTypes.Add(forrage);
            else
                this.timeUnitlHarvestable = timeUnitlHarvestable;
        }

        private void Chop(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Wood, 1));
            Dispose();
        }

        private void Forrage(Instruction instruction)
        {
            instruction.ActiveMember.Inventory.AddItem(new ResourceItem(Resource.Food, 2));
            //This is tempory and should be replaced by the resource system
            if (IsHarvestable)
            {
                IsHarvestable = false;
                InstructionTypes.Remove(forrage);
                timeUnitlHarvestable = timeToHarvestable;
            }
        }

        private void Dispose()
        {
            GameObjectManager.Remove(this);
            this.InstructionTypes.Clear();
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
            return new Bush(PFSPosition, (TextureTypes)textureHarvestableID, (TextureTypes)textureHarvestedID, _isHarvestable, timeUnitlHarvestable);
        }
    }
}
