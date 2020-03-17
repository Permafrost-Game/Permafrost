using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GeonBit.UI.Entities;
using GeonBit.UI;

namespace GlobalWarmingGame.UI.Menus
{
    class MainMenu : Entity
    {
        public Button MainToIntro { get; private set; }
        public Button MainToGame { get; private set; }
        public Button MainToQuit { get; private set; }

        private const float MARGIN = 35f;
        private static readonly Vector2 BUTTON_SIZE = new Vector2(300f, 50f);
        private static readonly Vector2 IMAGE_SIZE = new Vector2(400, 400);

        public MainMenu(Texture2D texture)
        {
            Panel menu = new Panel(Vector2.Zero, PanelSkin.Simple, Anchor.Center);
            this.AddChild(menu);

            Panel frame = new Panel(IMAGE_SIZE, PanelSkin.Golden, Anchor.TopCenter, new Vector2(0, MARGIN));
            menu.AddChild(frame);

            Image img = new Image(texture);
            frame.AddChild(img);

            int numberOfButtons = 3;

            MainToIntro = new Button("New Game", ButtonSkin.Default, Anchor.BottomCenter, BUTTON_SIZE, new Vector2(0, MARGIN + (BUTTON_SIZE.Y * 1.5f) * --numberOfButtons));
            menu.AddChild(MainToIntro);

            MainToGame = new Button("Load Game", ButtonSkin.Default, Anchor.BottomCenter, BUTTON_SIZE, new Vector2(0, MARGIN + (BUTTON_SIZE.Y * 1.5f) * --numberOfButtons));
            menu.AddChild(MainToGame);

            MainToQuit = new Button("Quit", ButtonSkin.Default, Anchor.BottomCenter, BUTTON_SIZE, new Vector2(0, MARGIN + (BUTTON_SIZE.Y * 1.5f)  * --numberOfButtons));
            menu.AddChild(MainToQuit);

            
        }
    }
}
