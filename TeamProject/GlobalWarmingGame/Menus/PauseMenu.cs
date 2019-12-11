using Microsoft.Xna.Framework;

using GeonBit.UI;
using GeonBit.UI.Entities;

namespace GlobalWarmingGame.Menus
{
    class PauseMenu : Entity
    {
        public Panel Menu { get; private set; }
        public Button PauseToGame { get; private set; }
        public Button PauseToOptions { get; private set; }
        public Button PauseToMain { get; private set; }
        public Button PauseToQuit { get; private set; }

        public PauseMenu()
        {
            Menu = new Panel(new Vector2(350, 450), PanelSkin.Simple)
            {
                Opacity = 192
            };

            Label label = new Label("Paused", Anchor.TopCenter)
            {
                Scale = 2f,
                Offset = new Vector2(0, 10)
            };
            Menu.AddChild(label);

            PauseToGame = new Button("Resume", ButtonSkin.Default, Anchor.Center, new Vector2(250, 50), new Vector2(0, -85));
            PauseToGame.ButtonParagraph.Scale = 0.8f;
            Menu.AddChild(PauseToGame);

            PauseToOptions = new Button("Options", ButtonSkin.Default, Anchor.Center, new Vector2(250, 50), new Vector2(0, -10));
            PauseToOptions.ButtonParagraph.Scale = 0.8f;
            Menu.AddChild(PauseToOptions);

            PauseToMain = new Button("Quit to Main Menu", ButtonSkin.Default, Anchor.Center, new Vector2(250, 50), new Vector2(0, 60));
            PauseToMain.ButtonParagraph.Scale = 0.8f;
            Menu.AddChild(PauseToMain);

            PauseToQuit = new Button("Quit to Desktop", ButtonSkin.Default, Anchor.Center, new Vector2(250, 50), new Vector2(0, 135));
            PauseToQuit.ButtonParagraph.Scale = 0.8f;
            Menu.AddChild(PauseToQuit);

            UserInterface.Active.AddEntity(Menu);

            Menu.Visible = false;
        }
    }
}
