using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Lighting
{
    public class Light : GameObject, Engine.Drawing.IDrawable
    {
        public Color Color { get; set; }
        public RenderTarget2D RenderTarget { get; private set; }
        public float Radius { get; set; }

        public new Vector2 Position
        {
            get => base.Position;
            set => base.Position = value;
        }

        public Light(Vector2 position, GraphicsDevice graphicsDevice, float radius, Color color) : base(position, Vector2.Zero)
        {
            float baseSize = radius * 2f;

            this.Size = new Vector2(baseSize);
            this.RenderTarget = new RenderTarget2D(graphicsDevice, (int)baseSize, (int)baseSize);
            this.Radius = radius;
            this.Color = color;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(RenderTarget, Position - new Vector2(Radius), this.Color);
        }

    }
}
