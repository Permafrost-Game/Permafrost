using Engine;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GlobalWarmingGame.UI
{
    class InventoryTransactionMessage
    {
        private readonly Paragraph p;
        private readonly GameObject parent;
        private readonly Camera camera;
        private float timeUntilDispose;
        private const float DEFAULT_TIME_ALIVE = 3000f;
        public bool IsActive { get => p != null ? p.Parent == UserInterface.Active.Root : true; }
        public InventoryTransactionMessage(GameObject parent, Camera camera, string message, float timeAlive = DEFAULT_TIME_ALIVE)
        {
            this.parent = parent;
            this.camera = camera;
            this.timeUntilDispose = timeAlive;
            p = new Paragraph(
                text: message,
                anchor: Anchor.TopLeft
                );

            UserInterface.Active.AddEntity(p);
        }

        public void Update(GameTime gameTime)
        {
            p.Offset = CalculatePosition(parent, camera);
            p.Opacity = Convert.ToByte((timeUntilDispose / DEFAULT_TIME_ALIVE) * byte.MaxValue);

            timeUntilDispose -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeUntilDispose <= 0)
            {
                UserInterface.Active.RemoveEntity(p);
            }

        }

        private static Vector2 CalculatePosition(GameObject parent, Camera camera)
        {
            return Vector2.Transform(new Vector2(parent.Position.X - parent.Size.X / 2, parent.Position.Y - 15f - parent.Size.Y / 2) / UserInterface.Active.GlobalScale, camera.Transform);
        }

    }
}
