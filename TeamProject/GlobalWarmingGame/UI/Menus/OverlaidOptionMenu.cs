using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.UI.Menus
{
    class OverlaidOptionMenu : Entity
    {

        private readonly Panel menu;
        private readonly Label label;
        public OverlaidOptionMenu(string menuText, Vector2 size = default, Anchor anchor = Anchor.Auto, Vector2 offset = default) :
            base(size, anchor, offset)
        {
            menu = new Panel(new Vector2(350, 450), PanelSkin.Simple)
            {
                Opacity = 192
            };

            label = new Label(menuText, Anchor.TopCenter)
            {
                Scale = 2f,
                Offset = new Vector2(0, 10)
            };
            menu.AddChild(label);


            AddChild(menu);
        }

        public void ClearButtons()
        {
            menu.ClearChildren();
            menu.AddChild(label);
        }

        public void AddButtons<T>(IEnumerable<ButtonHandler<T>> buttons, bool displayTag = false)
        {
            foreach (ButtonHandler<T> b in buttons)
            {
                AddButton(b, displayTag);
            }
        }

        public void AddButton<T>(ButtonHandler<T> button, bool displayTag = false)
        {
            string name = displayTag ? $"{button.ToString()} : {button.Tag.ToString()}" : button.ToString();

            Button currentButton = new Button(name, ButtonSkin.Default, Anchor.AutoCenter, new Vector2(250, 50), new Vector2(0, 0))
            {
                OnClick = (Entity e) => { button.action(button.Tag); }
            };

            currentButton.ButtonParagraph.Scale = 0.8f;
            menu.AddChild(currentButton);
        }
    }
}
