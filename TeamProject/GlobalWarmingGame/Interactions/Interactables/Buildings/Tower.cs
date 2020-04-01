using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.Interactions.Interactables.Buildings
{
    public class Tower : Sprite, IInteractable, IReconstructable, IHeatSource
    {
        public Temperature Temperature { get; set; } = new Temperature(100);
        public bool Heating { get; private set; }
        public List<InstructionType> InstructionTypes { get;}
        private readonly Texture2D hostileTexture;
        private readonly Texture2D capturedTexture;

        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        [PFSerializable]
        public bool captured;

        public Tower() : base(Vector2.Zero, Vector2.Zero) { }

        public Tower(Vector2 position, bool captured = false) : base
        (
            position: position,
            texture: Textures.Map[TextureTypes.TowerH]
        )
        {
            hostileTexture = Textures.Map[TextureTypes.TowerH];
            capturedTexture = Textures.Map[TextureTypes.TowerC];
            InstructionTypes = new List<InstructionType>();

            if (this.captured = captured)
            {
                Texture = capturedTexture;
                Heating = true;
            }
            else
            {
                InstructionTypes.Add(new InstructionType(
                    id: "capture",
                    name: "Capture",
                    description: "Capture",
                    checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                    onComplete: Capture)
                    );
                Heating = false;
            }
        }

        private void Capture(Instruction instruction)
        {
            captured = true;
            InstructionTypes.Clear();
            this.Texture = capturedTexture;
            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Colonist, new Vector2 (this.Position.X, this.Position.Y + GameObjectManager.ZoneMap.TileSize.Y)));
            Heating = true;
        }

        public object Reconstruct()
        {
            return new Tower(PFSPosition, captured);
        }
    }
}
