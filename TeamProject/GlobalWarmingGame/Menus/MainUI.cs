using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using GeonBit.UI;
using GeonBit.UI.Entities;


namespace GlobalWarmingGame.Menus
{
    class MainUI : Entity
    {
        public Panel TopPanel { get; private set; }
        public Panel BottomPanel { get; private set; }

        public MainUI()
        {
            TopPanel = new Panel(new Vector2(1000, 100), PanelSkin.Default, Anchor.TopCenter);

            BottomPanel = new Panel(new Vector2(1000, 100), PanelSkin.Default, Anchor.BottomCenter);

            UserInterface.Active.AddEntity(TopPanel);
            UserInterface.Active.AddEntity(BottomPanel);
        }
    }
}
