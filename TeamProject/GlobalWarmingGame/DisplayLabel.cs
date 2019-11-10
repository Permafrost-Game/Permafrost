
using Engine;
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace GlobalWarmingGame
{

    class DisplayLabel : GameObject, IUpdatable
    {
        public int Value { get; set; }
        public string Message { get; set; }
        Label titleLabel = null;

        private Desktop desktop;
        public DisplayLabel(int value, string message, Desktop desktop, string tag) : base (new Vector2(0), new Vector2(1), tag)
        {
            Value = value;
            Message = message;
            this.desktop = desktop;
        }

        public void Update(GameTime gameTime)
        {
            
            if(titleLabel != null)
            {
                desktop.Widgets.Remove(titleLabel);
            }

            titleLabel = new Label
            {
                Text = $"{Message} : {Value}",
                HorizontalAlignment = HorizontalAlignment.Center,
                TextColor = Color.HotPink
            };

            desktop.Widgets.Add(titleLabel);
        }

    }
}
