using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Enemies;
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
        public List<InstructionType> InstructionTypes { get; }

        #region PFSerializable
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
        #endregion

        private readonly Texture2D hostileTexture;
        private readonly Texture2D capturedTexture;

        public int robots;
        [PFSerializable]
        public bool _isCaptured;
        private Random rand; 
        private bool IsCaptured
        {
            get { return _isCaptured; }
            set
            {
                _isCaptured = value;
                Texture = _isCaptured ? capturedTexture : hostileTexture;
            }
        }

        public Tower() : base(Vector2.Zero, Vector2.Zero) { }

        public Tower(Vector2 position, TextureTypes capturedTextureType = TextureTypes.TowerC, TextureTypes hostileTextureType = TextureTypes.TowerH, bool captured = false) : base
        (
            position: position,
            texture: Textures.Map[hostileTextureType]
        )
        {
            hostileTextureID = (int)hostileTextureType;
            capturedTextureID = (int)capturedTextureType;

            hostileTexture = Textures.Map[hostileTextureType];
            capturedTexture = Textures.Map[capturedTextureType];
            InstructionTypes = new List<InstructionType>();

            rand = new Random();
            this.robots = rand.Next(0, 5);

            int tileSize = (int)GameObjectManager.ZoneMap.TileSize.Y;
            for (int i = 0; i < robots; i++)
            {
                GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Robot,
                    GameObjectManager.ZoneMap.GetTileAtPosition(position).Type.Equals("textures/tiles/main_tileset/water") ?
                    position :
                    position + new Vector2(rand.Next(-tileSize * 3, tileSize * 3), rand.Next(-tileSize * 3, tileSize * 3))
                    ));
            }

            GameObjectManager.ObjectRemoved += ObjectRemovedEventHandler; 

            if (IsCaptured = captured)
            {
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

        private void ObjectRemovedEventHandler(Object sender, GameObject GameObject)
        {
            if(GameObject is Robot)
            {
                this.robots--;
            }
        }
        private void Capture(Instruction instruction)
        {
            if (robots == 0)
            {
                IsCaptured = true;
                Heating = true;
                InstructionTypes.Clear();
                GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Colonist, new Vector2(this.Position.X, this.Position.Y + GameObjectManager.ZoneMap.TileSize.Y)));
            }
            else
            {
                //alert you must kill all robots to capture tower. 
            }
        }

        public void ResetCapture()
        {
            IsCaptured = false;
            Heating = false;
            InstructionTypes.Clear();
            InstructionTypes.Add(new InstructionType(
                     id: "capture",
                     name: "Capture",
                     description: "Capture",
                     checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                     onComplete: Capture)
                     );
        }

        public object Reconstruct()
        {
            return new Tower(PFSPosition, (TextureTypes)capturedTextureID, (TextureTypes)hostileTextureID, _isCaptured);
        }
    }
}
