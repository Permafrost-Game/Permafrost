using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using GeonBit.UI;
using GeonBit.UI.Entities;
using Engine;
using Microsoft.Xna.Framework.Graphics;
using GlobalWarmingGame.Resources;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Action;

namespace GlobalWarmingGame.Menus
{
    class MainUI : Entity
    {
        public Panel TopPanel { get; private set; }
        public Panel BottomPanel { get; private set; }
        public DropDown BuildMenu { get; private set; }
        public DropDown SpawnMenu { get; private set; }

        Label foodLabel;

        Icon[] itemSlots = new Icon[24];
        bool open;

        public MainUI()
        {
            //Top Panel
            TopPanel = new Panel(new Vector2(0, 100), PanelSkin.Simple, Anchor.TopCenter)
            {
                Opacity = 192
            };

            BuildMenu = new DropDown(new Vector2(225, 75), Anchor.CenterLeft, new Vector2(0, 4), PanelSkin.ListBackground, PanelSkin.ListBackground, true)
            {
                DefaultText = "Buildings",
                AutoSetListHeight = true
            };
            TopPanel.AddChild(BuildMenu);

            SpawnMenu = new DropDown(new Vector2(225, 75), Anchor.CenterLeft, new Vector2(250, 4), PanelSkin.ListBackground, PanelSkin.ListBackground, true)
            {
                DefaultText = "Spawn",
                AutoSetListHeight = true
            };
            TopPanel.AddChild(SpawnMenu);

            Icon foodIcon = new Icon(IconType.Apple, Anchor.CenterRight, 1f, false);
            TopPanel.AddChild(foodIcon);
            foodLabel = new Label("Food Counter", Anchor.CenterRight, null, new Vector2(75,0));
            TopPanel.AddChild(foodLabel);

            UserInterface.Active.AddEntity(TopPanel);

            //Bottom Panel
            BottomPanel = new Panel(new Vector2(0, 100), PanelSkin.Simple, Anchor.BottomCenter)
            {
                Opacity = 192
            };

            Icon collectiveInventoryButton = new Icon(IconType.Sack, Anchor.CenterLeft, 1f, true);
            BottomPanel.AddChild(collectiveInventoryButton);

            Panel collectiveInventory = new Panel(new Vector2(282, 400), PanelSkin.Simple, Anchor.TopLeft, new Vector2(-26, -426))
            {
                Opacity = 192,
                Visible = open
            };
            BottomPanel.AddChild(collectiveInventory);
            

            collectiveInventoryButton.OnClick = (Entity btn) => { open = !open; collectiveInventory.Visible = open; };

            for (int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i] = new Icon(IconType.None, Anchor.AutoInline, 0.75f, true);
                collectiveInventory.AddChild(itemSlots[i]);
            }

            UserInterface.Active.AddEntity(BottomPanel);

            TopPanel.Visible = false;
            BottomPanel.Visible = false;
        }

        public void Update(GameTime gameTime)
        {
            foodLabel.Text = CollectiveInventory.TotalFood.ToString();
        }
    }
}
