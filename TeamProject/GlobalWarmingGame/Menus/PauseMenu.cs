using Microsoft.Xna.Framework;

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
            Menu = new Panel();

            Label label = new Label("", Anchor.Center, new Vector2(500, 200));
            Menu.AddChild(label);

            PauseToGame = new Button("Resume", ButtonSkin.Default, Anchor.Center, new Vector2(300, 50), new Vector2(0, -110));
            Menu.AddChild(PauseToGame);

            PauseToOptions = new Button("Options", ButtonSkin.Default, Anchor.Center, new Vector2(300, 50), new Vector2(0, -35));
            Menu.AddChild(PauseToOptions);

            PauseToMain = new Button("Quit to Main Menu", ButtonSkin.Default, Anchor.Center, new Vector2(300, 50), new Vector2(0, 35));
            Menu.AddChild(PauseToMain);

            PauseToQuit = new Button("Quit to Desktop", ButtonSkin.Default, Anchor.Center, new Vector2(300, 50), new Vector2(0, 110));
            Menu.AddChild(PauseToQuit);
        }
    }
}
