using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;

namespace GlobalWarmingGame.UI.Menus
{
    class LoadMenu : Entity
    {

        private class LoadSaveEntity : Entity
        {
            public LoadSaveEntity(string saveName, TimeSpan playTime, int numberOfTowersCaptured, Vector2 size = default, Anchor anchor = Anchor.Auto, Vector2 offset = default) :
                base(size, anchor, offset)
            {
                string days  = playTime.TotalDays > 1 ? ((int)playTime.TotalDays)    + "d" : "";
                string hours = playTime.Hours     > 1 ? ((int)playTime.Hours % 24)   + "h" : "";
                string mins  = playTime.Minutes   > 1
                            && playTime.TotalDays < 1 ? ((int)playTime.Minutes % 60) + "m" : "";

                string text = $"{saveName} - Play Time: {days}{hours}{mins} - Towers Captured: {numberOfTowersCaptured}";
                Button p = new Button(text, ButtonSkin.Default, Anchor.CenterLeft, new Vector2(0, 120), new Vector2(0))
                {
                    Scale = 3f
                };
                
                this.AddChild(p);


                Button delete = new Button("Delete Save", anchor: Anchor.CenterRight, size: new Vector2(300,100))
                {
                    Scale = 3f
                };
                p.AddChild(delete);




            }
        }

        public LoadMenu()
        {
            Panel menu = new Panel(Vector2.Zero, PanelSkin.Simple, Anchor.Center);
            this.AddChild(menu);

            menu.AddChild(new LoadSaveEntity("Test", new TimeSpan(4, 10, 2), 3, new Vector2(0, 200)));


        }
    }
}
