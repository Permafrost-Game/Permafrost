using Engine;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using System;

namespace GlobalWarmingGame.UI
{
    class FloatingHealthBar : IUpdatableUI
    {
        private readonly ProgressBar progressBar;
        private readonly IHealthbased parent;
        private readonly Color lowColor, highColor;
        private readonly float decayTime;

        private float timeUntilInvisible;
        private readonly bool visibleWhenNotMax;
        private float _Health;
        private float Health {
            get
            {
                return _Health;
            }
            set
            {
                this._Health = value;
                progressBar.Value = (int)_Health;
                if(_Health >= parent.MaxHealth / 2)
                {
                    progressBar.ProgressFill.FillColor = highColor;
                }
                else
                {
                    progressBar.ProgressFill.FillColor = lowColor;
                }
            }
        }

        public bool IsActive { get => progressBar.Parent == UserInterface.Active.Root; }

        public FloatingHealthBar(IHealthbased parent, float decayTime, bool visibleWhenNotMax, Color highColor, Color lowColor)
        {
            progressBar = new ProgressBar(
                min: 0,
                max: Convert.ToUInt32(parent.MaxHealth),
                size: CalculateSize((GameObject)parent),
                anchor: Anchor.TopLeft,
                offset: new Vector2(0, 30)
                )
            {
                Locked = true,
            };
            UserInterface.Active.AddEntity(progressBar);

            this.parent = parent;
            this.visibleWhenNotMax = visibleWhenNotMax;
            this.decayTime = decayTime;
            this.lowColor = lowColor;
            this.highColor = highColor;
            this.timeUntilInvisible = 0f;
            this.Health = parent.Health;
        }

        public void Update(GameTime gameTime)
        {
            progressBar.Offset = CalculatePosition((GameObject)parent);
            progressBar.Size = CalculateSize((GameObject)parent);

            if (Health != parent.Health)
            {
                Health = parent.Health;
                timeUntilInvisible = decayTime;

                if (parent.Health <= 0 && timeUntilInvisible <= 0)
                {
                    UserInterface.Active.RemoveEntity(progressBar);
                    return;
                }
            }


            if(!visibleWhenNotMax || progressBar.Value == progressBar.Max || parent.Health <=0)
            {
                timeUntilInvisible -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            progressBar.Visible = timeUntilInvisible > 0;
            

        }

        private static Vector2 CalculatePosition(GameObject parent)
        {
            return Vector2.Transform(new Vector2(parent.Position.X - parent.Size.X / 2, parent.Position.Y - 10f - parent.Size.Y / 2) / UserInterface.Active.GlobalScale, GameObjectManager.Camera.Transform);
        }

        private static Vector2 CalculateSize(GameObject parent)
        {
            return new Vector2(parent.Size.X * GameObjectManager.Camera.Zoom, 6f * GameObjectManager.Camera.Zoom);
        }
    }
}
