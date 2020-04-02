using System.Collections.Generic;

namespace GlobalWarmingGame.UI.Menus
{
    class PauseMenu : OverlaidOptionMenu
    {
        private const string name = "Paused";

        private static void ApplyState(GameState state) => Game1.GameState = state;

        public PauseMenu() : base (name)
        {
            AddButtons(new List<ButtonHandler<GameState>>
            {
                new ButtonHandler<GameState>(GameState.Playing, "Resume", ApplyState),
                new ButtonHandler<GameState>(GameState.Settings, "Settings", ApplyState),
                new ButtonHandler<GameState>(GameState.MainMenu, "Quit to Main Menu", ApplyState),
                new ButtonHandler<GameState>(GameState.Exiting, "Quit to Desktop", ApplyState),
            });
        }
    }
}
