using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace GlobalWarmingGame
{
    public class PauseMenu : Panel
    {
        public MenuItem PauseToGame { get; set; }
        public MenuItem PauseToMain { get; set; }
        public MenuItem PauseToQuit { get; set; }

        VerticalMenu pauseMenu;

        public PauseMenu()
        {
            BuildPauseMenu();
        }

        void BuildPauseMenu()
		{
            PauseToGame = new MenuItem
            {
                Id = "_pauseToGame",
                Text = "Resume Game",
                ShortcutText = ""
            };

            PauseToMain = new MenuItem
            {
                Id = "_pauseToMain",
                Text = "Quit to Main Menu",
                ShortcutText = ""
            };

            PauseToQuit = new MenuItem
            {
                Id = "_pauseToQuit",
                Text = "Quit to Desktop",
                ShortcutText = ""
            };

            pauseMenu = new VerticalMenu
            {
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center,
                VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center
            };

            pauseMenu.Items.Add(PauseToGame);
			pauseMenu.Items.Add(PauseToMain);
			pauseMenu.Items.Add(PauseToQuit);

            Widgets.Add(pauseMenu);
        }

        public void DrawPauseMenu(Desktop desktop, Point position)
        {
            desktop.ShowContextMenu(pauseMenu, position);
        }   
    }
}