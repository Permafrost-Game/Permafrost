using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public class CampFire : AnimatedSprite, IInteractable, IBuildable, IHeatSource, IReconstructable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.Wood, 2), 
                                                                                                   new ResourceItem(Resource.Fibers, 1) };
        public Temperature Temperature { get; set; } = new Temperature(50);

        [PFSerializable]
        public bool Heating { get; set; }
        public List<InstructionType> InstructionTypes { get; }

        #region PFSerializable
        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public CampFire() : base(Vector2.Zero, Textures.MapSet[TextureSetTypes.CampFire]) { }
        #endregion

        private readonly InstructionType fuel;

        [PFSerializable]
        public float timeUntilBurntout;
        private const float burnoutTime = 30000f;

        public CampFire(Vector2 position, bool heating = true, float timeUntilBurntout = burnoutTime) : base
        (
            position: position,
            textureSet: Textures.MapSet[TextureSetTypes.CampFire],
            frameTime: 50f
        )
        {
            InstructionTypes = new List<InstructionType>();
            fuel = new InstructionType("fuel", "Fuel", "Fuel campfire", requiredResources: new List<ResourceItem>() { new ResourceItem(Resource.Wood, 1) }, onComplete: Fuel);

            Heating = heating;

            if (Heating == true)
                TextureGroupIndex = 1;
            else
            {
                TextureGroupIndex = 2;
                InstructionTypes.Add(fuel);
            }

            this.timeUntilBurntout = timeUntilBurntout;
        }

        private void Fuel(Instruction instruction)
        {
            if (instruction.ActiveMember.Inventory.ContainsAll(instruction.Type.RequiredResources))
            {
                foreach (ResourceItem item in instruction.Type.RequiredResources)
                {
                    instruction.ActiveMember.Inventory.RemoveItem(item);
                }

                Heating = true;
                TextureGroupIndex = 1;
                InstructionTypes.Clear();

            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Heating == true)
            {
                timeUntilBurntout -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeUntilBurntout < 0f)
                {
                    TextureGroupIndex = 2;

                    InstructionTypes.Add(fuel);

                    Heating = false;
                    timeUntilBurntout = burnoutTime;
                }
            }
        }

        public void Build()
        {
            GameObjectManager.Add(this);
            InstructionTypes.Clear();
        }

        public object Reconstruct()
        {
            return new CampFire(PFSPosition, Heating, timeUntilBurntout);
        }
    }
}