using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// This class is for implementation specific UI, in this case GeoBit UI<br>
    /// This class is for creating buttons and panels based on the information provided by controller.<br>
    /// </summary>
    class View
    {

        private readonly Panel topPanel;

        private Panel menu;


        public bool Hovering { get; private set; } = false;

        public View()
        {
            UserInterface.Active.WhileMouseHoverOrDown = (Entity e) => { Hovering = true; };
            UserInterface.Active.         OnMouseLeave = (Entity e) => { Hovering = false; };

            topPanel = new Panel(new Vector2(0, 100), PanelSkin.Simple, Anchor.TopCenter)
            {
                Opacity = 192
            };
            UserInterface.Active.AddEntity(topPanel);
        }


        public void CreateMenu<T>(Point location, List<ButtonHandler<T>> options)
        {
            if (menu != null) UserInterface.Active.RemoveEntity(menu);

            menu = new Panel(new Vector2(150, 75 + (options.Count * 30)), PanelSkin.Default, Anchor.TopLeft, location.ToVector2());
            UserInterface.Active.AddEntity(menu);

            Label label = new Label("Choose Action", Anchor.TopCenter, new Vector2(500, 50))
            {
                Scale = 0.7f
            };

            menu.AddChild(label);
            int counter = 0;
            foreach (ButtonHandler<T> option in options)
            {
                Button newButton = new Button(option.Tag.ToString(), ButtonSkin.Default, Anchor.TopCenter, new Vector2(125, 25), new Vector2(0, (counter + 1) * 30));
                newButton.ButtonParagraph.Scale = 0.5f;
                newButton.Padding = Vector2.Zero;
                menu.AddChild(newButton);

                newButton.OnClick = (Entity btn) =>
                {
                    option.action(option.Tag);
                    menu.Visible = false;
                    Hovering = false;
                };
                counter++;
            }

        }

        public void CreateDropDown<T>(string label, List<ButtonHandler<T>> options)
        {
            DropDown menu = new DropDown(new Vector2(225, 75), Anchor.CenterLeft, new Vector2(250 * topPanel.Children.Count, 4), PanelSkin.ListBackground, PanelSkin.ListBackground, true)
            {
                DefaultText = label,
                AutoSetListHeight = true,
                DontKeepSelection = true,
            };

            foreach (ButtonHandler<T> option in options)
            {
                menu.AddItem(option.Tag.ToString());
            }

            topPanel.AddChild(menu);

            menu.OnValueChange = (Entity e) => 
            {
                foreach (ButtonHandler<T> option in options)
                {
                    if (option.Tag.ToString().Equals(menu.SelectedValue)) {
                        option.action(option.Tag);
                        break;
                    }
                }
            };

        }
    }
}
