using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.UI.Menus
{
    class OverlaidOptionMenu<T> : Entity
    {
        public OverlaidOptionMenu(string menuText, IEnumerable<ButtonHandler<T>> buttons, Vector2 size = default, Anchor anchor = Anchor.Auto, Vector2 offset = default) :
            base(size, anchor, offset)
        {
            Panel Menu = new Panel(new Vector2(350, 450), PanelSkin.Simple)
            {
                Opacity = 192
            };

            Label label = new Label(menuText, Anchor.TopCenter)
            {
                Scale = 2f,
                Offset = new Vector2(0, 10)
            };
            Menu.AddChild(label);

            foreach(ButtonHandler<T> b in buttons)
            {
                Button currentButton = new Button(b.ToString(), ButtonSkin.Default, Anchor.AutoCenter, new Vector2(250, 50), new Vector2(0, 0))
                {
                    OnClick = (Entity e) => { b.action(b.Tag); }
                };

                currentButton.ButtonParagraph.Scale = 0.8f;
                Menu.AddChild(currentButton);
            }

            AddChild(Menu);
        }
    }
}
