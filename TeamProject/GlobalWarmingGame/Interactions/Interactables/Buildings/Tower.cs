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
    public class Tower : Sprite, IInteractable, IReconstructable, IHeatable
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
        public readonly int hostileTextureID;

        [PFSerializable]
        public readonly int capturedTextureID;

        [PFSerializable]
        public bool captured;

        public Tower() : base(Vector2.Zero, Vector2.Zero)
        {

        }

        public Tower(Vector2 position, TextureTypes capturedTextureType, TextureTypes hostileTextureType, bool captured = false) : base
        (
            position: position,
            size: new Vector2(Textures.Map[hostileTextureType].Width, Textures.Map[hostileTextureType].Height),
            rotation: 0f,
            origin: new Vector2(Textures.Map[hostileTextureType].Width / 2f, Textures.Map[hostileTextureType].Height / 2f),
            tag: "Farm",
            texture: Textures.Map[hostileTextureType]
        )
        {
            hostileTextureID = (int)hostileTextureType;
            capturedTextureID = (int)capturedTextureType;

            hostileTexture = Textures.Map[hostileTextureType];
            capturedTexture = Textures.Map[capturedTextureType];
            InstructionTypes = new List<InstructionType>();

            if (this.captured = captured)
            {
                Texture = capturedTexture;
                Heating = true;
            }
            else
            {
                InstructionTypes.Add(new InstructionType("capture", "Capture", "Capture", onComplete: Capture));
                Heating = false;
            }
        }

        private void Capture(Instruction instruction)
        {
            captured = true;
            InstructionTypes.Clear();
            this.Texture = capturedTexture;
            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Colonist, new Vector2 (this.Position.X, this.Position.Y + 32)));
            Heating = true;
        }

        public object Reconstruct()
        {
            return new Tower(PFSPosition, (TextureTypes)capturedTextureID, (TextureTypes)hostileTextureID, captured);
        }
    }
}
