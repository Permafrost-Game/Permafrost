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
    public class Tower : Sprite, IInteractable
       {
        public List<InstructionType> InstructionTypes { get;}
        private readonly Texture2D hostileTexture;
        private readonly Texture2D capturedTexture;
        public Tower(Vector2 position, Texture2D capturedTexture, Texture2D hostileTexture) : base
        (
            position: position,
            size: new Vector2(hostileTexture.Width, hostileTexture.Height),
            rotation: 0f,
            origin: new Vector2(hostileTexture.Width / 2f, hostileTexture.Height / 2f),
            tag: "Farm",
            texture: hostileTexture
        )
        {
            this.hostileTexture = hostileTexture;
            this.capturedTexture = capturedTexture;
            InstructionTypes = new List<InstructionType>();
            InstructionTypes.Add(new InstructionType("capture", "Capture", "Capture", onStart: Capture));
        }

        private void Capture(IInstructionFollower follower)
        {
            InstructionTypes.Clear();
            this.Texture = capturedTexture;
            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Colonist, new Vector2 (this.Position.X, this.Position.Y + 32)));
        }
    }
}
