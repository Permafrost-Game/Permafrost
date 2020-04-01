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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public class CampFire : AnimatedSprite, IInteractable, IBuildable, IHeatSource, IReconstructable
    {
        public List<ResourceItem> CraftingCosts { get; private set; } = new List<ResourceItem>() { new ResourceItem(Resource.Wood, 2), new ResourceItem(Resource.Fibers, 1) };
        public Temperature Temperature { get; set; } = new Temperature(50);
        public bool Heating { get; private set; }
        public List<InstructionType> InstructionTypes { get; }

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        public CampFire() : base(Vector2.Zero, Textures.MapSet[TextureSetTypes.campFire])
        {

        }

        public CampFire(Vector2 position) : base
        (
            position: position,
            textureSet: Textures.MapSet[TextureSetTypes.campFire],
            frameTime: 50f
        )
        {
            Heating = true;

            TextureGroupIndex = 1;
            InstructionTypes = new List<InstructionType>
            {
                new InstructionType("fuel", "Fuel", "Fuel campfire", onComplete: Fuel)
            };
        }

        private void Fuel(Instruction instruction)
        {
        }

        public void Build()
        {
            GameObjectManager.Add(this);
        }

        public object Reconstruct()
        {
            return new CampFire(PFSPosition);
        }
    }
}
