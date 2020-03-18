using GlobalWarmingGame.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GlobalWarmingGame.UI.Controllers
{
    static class MainMenuUIController
    {
        private static MainMenuView<string> view;
        private static Texture2D mainMenuLogo;

        public static void LoadContent(ContentManager content)
        {
            mainMenuLogo = content.Load<Texture2D>(@"logo");
        }

        public static void CreateUI(float uiScale = 1f)
        {
            view = new MainMenuView<string>();
            view.SetUIScale(uiScale);
            view.CreateUI(mainMenuLogo);
            view.SetMainMenuVisiblity(true);

            //Add your saves from a file here
            string save = "save1";
            AddSave(save, new TimeSpan(4, 20, 0), 4);


        }


        private static void AddSave(string save, TimeSpan playTime, int numberOfTowersCaptured)
        {
            view.AddLoadSaveGame(
                name: save,
                playTime: playTime,
                numberOfTowersCaptured: numberOfTowersCaptured,
                onLoad: new ButtonHandler<string>(
                    tag: save,
                    LoadSaveGame),
                onDelete: new ButtonHandler<string>(
                    tag: save,
                    DeleteSaveGame)
                );
        }

        private static void LoadSaveGame(string saveGame)
        {
            Game1.GameState = GameState.Playing;
        }

        private static void DeleteSaveGame(string saveGame)
        {
            view.RemoveSaveGame(saveGame);
        }


        public static void Update(GameTime gameTime)
        {
            view.Update(gameTime);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            view.Draw(spriteBatch);
        }
    }
}
