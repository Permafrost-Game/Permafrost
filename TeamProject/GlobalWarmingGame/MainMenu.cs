using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace GlobalWarmingGame
{
	public class MainMenu: Panel
    {
        public MenuItem MainToGame { get; set; }
        public MenuItem MainToOptions { get; set; }
        public MenuItem MainToQuit { get; set; }

        VerticalMenu mainMenu;

        public MainMenu()
        {
            BuildMainMenu();
        }

		void BuildMainMenu()
		{
            MainToGame = new MenuItem
            {
                Id = "mainToGame",
                Text = "Start Game"
            };

            MainToOptions = new MenuItem
            {
                Id = "mainToOptions",
                Text = "Options"
            };

            MainToQuit = new MenuItem
            {
                Id = "mainToQuit",
                Text = "Quit"
            };

            mainMenu = new VerticalMenu
            {
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center,
                VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center
            };
            mainMenu.Items.Add(MainToGame);
			mainMenu.Items.Add(MainToOptions);
			mainMenu.Items.Add(MainToQuit);

            Widgets.Add(mainMenu);
        }

        public void DrawMainMenu(Desktop desktop, Point position)
        {
            desktop.ShowContextMenu(mainMenu, position);
        }
	}
}