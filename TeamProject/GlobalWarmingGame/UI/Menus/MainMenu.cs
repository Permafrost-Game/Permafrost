using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GeonBit.UI.Entities;
using GeonBit.UI;

namespace GlobalWarmingGame.UI.Menus
{
    class MainMenu : Entity
    {
        public Panel Menu { get; private set; }
        public Button MainToIntro { get; private set; }
        public Button MainToGame { get; private set; }
        public Button MainToQuit { get; private set; }

        public MainMenu(Texture2D texture)
        {
            Menu = new Panel(new Vector2(0,0), PanelSkin.Simple, Anchor.Center);

            Panel frame = new Panel(new Vector2(400, 400), PanelSkin.Golden, Anchor.TopCenter, new Vector2(0, 35));
            Menu.AddChild(frame);

            Image img = new Image(texture);
            frame.AddChild(img);

            MainToIntro = new Button("New Game", ButtonSkin.Default, Anchor.BottomCenter, new Vector2(300, 50), new Vector2(0, 185));
            Menu.AddChild(MainToIntro);

            MainToGame = new Button("Load Game", ButtonSkin.Default, Anchor.BottomCenter, new Vector2(300, 50), new Vector2(0, 110));
            Menu.AddChild(MainToGame);

            MainToQuit = new Button("Quit", ButtonSkin.Default, Anchor.BottomCenter, new Vector2(300, 50), new Vector2(0, 35));
            Menu.AddChild(MainToQuit);

            this.AddChild(Menu);
        }
    }
}
