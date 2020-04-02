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
        private const int FINAL_TOWER_INDEX = 10;
        private static int capturedCount;
        private static readonly Random rand = new Random();

        #region Temperature
        public Temperature Temperature { get; set; } = new Temperature(1000);
        public bool Heating { get => IsCaptured; }
        #endregion

        public List<InstructionType> InstructionTypes { get; }
        
        #region PFSerializable
        [PFSerializable]
        public Vector2 PFSPosition
        {
            get { return Position; }
            set { Position = value; }
        }

        #endregion

        private readonly Texture2D hostileTexture;
        private readonly Texture2D capturedTexture;

        [PFSerializable]
        public int robots;

        [PFSerializable]
        public bool isFinal;

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

        /// <summary>
        /// Creates a tower without robots
        /// </summary>
        /// <param name="captured"></param>
        /// <param name="pos"></param>
        /// <param name="robots"></param>
        /// <param name="isFinal"></param>
        private Tower(Vector2 pos, bool captured, int robots, bool isFinal) 
        : base(
              position: pos,
              texture: captured? isFinal? Textures.Map[TextureTypes.TowerF] : Textures.Map[TextureTypes.TowerH] : Textures.Map[TextureTypes.TowerC]
        )
        {
            this.hostileTexture = Textures.Map[TextureTypes.TowerH];
            this.capturedTexture = Textures.Map[TextureTypes.TowerC];

            this.robots = robots;
            this.isFinal = isFinal;
            this.IsCaptured = captured;
            
            GameObjectManager.ObjectRemoved += ObjectRemovedEventHandler;

            InstructionTypes = new List<InstructionType>();

            if (!IsCaptured)
            {
                InstructionTypes.Add(new InstructionType(
                    id: "capture",
                    name: "Capture",
                    description: "Capture",
                    checkValidity: (Instruction i) => InstructionTypes.Contains(i.Type),
                    onComplete: Capture)
                    );
            }
        }

        /// <summary>
        /// Creates a tower with randomized robots
        /// </summary>
        /// <param name="position">The position the tower should be</param>
        /// <param name="isCaptured">whether the tower is a captured one</param>
        public Tower(Vector2 position, bool isCaptured = false)
        : this(
            pos: position,
            captured: isCaptured,
            robots: rand.Next(capturedCount + 1, (capturedCount + 1) * 2),
            isFinal: capturedCount == FINAL_TOWER_INDEX
        )
        {
            int tileSize = (int)GameObjectManager.ZoneMap.TileSize.Y;
            for (int i = 0; i < robots; i++)
            {
                GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Robot,
                    GameObjectManager.ZoneMap.GetTileAtPosition(position).Walkable ?
                    position + new Vector2(rand.Next(-tileSize * 3, tileSize * 3))
                    : position
                    ));
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
                InstructionTypes.Clear();
                GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Colonist, new Vector2(this.Position.X, this.Position.Y + GameObjectManager.ZoneMap.TileSize.Y)));
                if (capturedCount < FINAL_TOWER_INDEX)
                {
                    capturedCount++;
                }
                if (isFinal)
                {
                    CutSceneFactory.PlayVideo(VideoN.Intro);
                }
            }
            else
            {
                //alert you must kill all robots to capture tower. 
            }
        }

        public void ResetCapture()
        {
            IsCaptured = false;
            InstructionTypes.Clear();
            InstructionTypes.Add(new InstructionType(
                     id: "capture",
                     name: "Capture",
                     onComplete: Capture)
                     );
        }

        public object Reconstruct()
        {
            return new Tower(PFSPosition, _isCaptured, robots, isFinal);
        }
    }
}
