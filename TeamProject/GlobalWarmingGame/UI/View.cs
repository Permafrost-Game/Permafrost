using System;
using System.Collections.Generic;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// This class is for implementation specific UI, in this case GeoBit UI<br>
    /// This class is for creating buttons and panels based on the information provided by controller.<br>
    /// </summary>
    class View
    {

        private Panel menu;

        public void CreateInstructionMenu(Point location, List<ButtonHandler<T>> options)
        {
            if(menu != null) UserInterface.Active.RemoveEntity(menu);

            menu = new Panel(new Vector2(150, 75 + (options.Count * 30)), PanelSkin.Default, Anchor.TopLeft, location.ToVector2());
            UserInterface.Active.AddEntity(menu);

            Label label = new Label("Choose Action", Anchor.TopCenter, new Vector2(500, 50));
            label.Scale = 0.7f;
            menu.AddChild(label);
            int counter = 0;
            foreach (ButtonHandler<T> option in options)
            {
                Button newButton = new Button(option.instruction.Type.Name, ButtonSkin.Default, Anchor.TopCenter, new Vector2(125, 25), new Vector2(0, (counter + 1) * 30));
                newButton.ButtonParagraph.Scale = 0.5f;
                newButton.Padding = Vector2.Zero;
                menu.AddChild(newButton);

                newButton.OnClick = (Entity btn) =>
                {
                    option.action(option.instruction);
                    menu.Visible = false;
                };
                counter++;
            }


        }

    }
}
