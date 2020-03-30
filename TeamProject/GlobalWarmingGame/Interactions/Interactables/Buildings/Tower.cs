using Engine;
using Engine.Drawing;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables.Enemies;
using GlobalWarmingGame.UI.Controllers;
using GlobalWarmingGame.UI.Views;
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
        private List<Robot> robots;
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


        [PFSerializable]
        public bool _isCaptured;

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

        public Tower(Vector2 position, TextureTypes capturedTextureType = TextureTypes.TowerC, TextureTypes hostileTextureType = TextureTypes.TowerH, bool captured = false, int seed = 0) : base
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

            int tileSize = (int)GameObjectManager.ZoneMap.TileSize.Y;

            Random rand = null;
            if (seed != 0)
            {
                rand = new Random(seed);
            }
            else
            {
                rand = new Random();
            }

            robots = new List<Robot>();
            for (int i = 0; i < rand.Next(1, 5); i++)
            {
                Robot robot = (Robot)((GameObject)InteractablesFactory.MakeInteractable(Interactable.Robot,
                    GameObjectManager.ZoneMap.GetTileAtPosition(this.Position).Type.Equals("textures/tiles/main_tileset/water") ?
                    this.Position :
                    this.Position + new Vector2(rand.Next(-tileSize * 3, tileSize * 3), rand.Next(-tileSize * 3, tileSize * 3))
                    ));
                GameObjectManager.Add(robot);
                robots.Add(robot);
            }
        }

        private void Capture(Instruction instruction)
        {
            bool capturable = true;
            foreach(Robot robot in robots)
            {
                if(robot.Health > 0)
                {
                    capturable = false;
                    break; 
                }
            }
            if (capturable)
            {
                IsCaptured = true;
                Heating = true;
                InstructionTypes.Clear();
                GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Colonist, new Vector2(this.Position.X, this.Position.Y + GameObjectManager.ZoneMap.TileSize.Y)));
            }
            else
            {
                //notification to kill all robots before capturing tower. 
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
