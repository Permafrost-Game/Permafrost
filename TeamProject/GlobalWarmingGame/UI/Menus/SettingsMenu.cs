using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalWarmingGame.UI.Menus
{
    class SettingsMenu : OverlaidOptionMenu<GameState>
    {
        private const string name = "Settings";

        private static readonly List<ButtonHandler<GameState>> buttons = new List<ButtonHandler<GameState>>
        {
            new ButtonHandler<GameState>(GameState.Paused, "Done", ApplyState),
        };

        private static void ApplyState(GameState state) => Game1.GameState = state;

        public SettingsMenu() : base(name, buttons) { }
    }
}
