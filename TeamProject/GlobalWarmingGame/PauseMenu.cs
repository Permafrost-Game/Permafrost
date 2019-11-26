using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace GlobalWarmingGame
{
    public class PauseMenu : Panel
    {
        public MenuItem PauseToGame { get; set; }
        public MenuItem PauseToMain { get; set; }
        public MenuItem PauseToQuit { get; set; }

        VerticalMenu verticalMenu;
        
        public PauseMenu()
        {
            BuildUI();
        }

        void BuildUI()
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

            verticalMenu = new VerticalMenu
            {
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center,
                VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center
            };

            verticalMenu.Items.Add(PauseToGame);
			verticalMenu.Items.Add(PauseToMain);
			verticalMenu.Items.Add(PauseToQuit);

			Widgets.Add(verticalMenu);
		}

        public void ShowPauseMenu(Desktop desktop, Point position)
        {
            desktop.ShowContextMenu(verticalMenu, position);
        }   
    }
}