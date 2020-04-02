using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
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

        private static readonly float timeToHarvestable = 20000f;

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

        #endregion
        public Bush() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public Bush(Vector2 position, bool isHarvestable = true, float timeUnitlHarvestable = 0) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.BushH]
        )
        {
            InstructionTypes = new List<InstructionType>();
            
            this.textureHarvestable = Textures.Map[TextureTypes.BushH];
            this.textureHarvested = Textures.Map[TextureTypes.BushN];

            forrage = new InstructionType(
                id: "forrage",
                name: "Forrage",
                checkValidity: (Instruction i) => IsHarvestable,
                timeCost: 1000f,
                onComplete: Forrage
                );
            InstructionTypes.Add(new InstructionType(
                id: "chop",
                name: "Chop",
                checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                timeCost: 2000f,
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
            return new Bush(PFSPosition, _isHarvestable, timeUnitlHarvestable);
        }
    }
}
