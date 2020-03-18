using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.UI.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GlobalWarmingGame.UI.Views
{

    class MainMenuView<S>
    {
        private MainMenu mainMenu;
        private LoadMenu<S> loadMenu;

        /// <summary>True if the current mouse position is over a UI entity</summary>
        internal bool Hovering { get; set; }


        public MainMenuView()
        {
        }

        internal void Initalise(ContentManager content)
        {
            UserInterface.Initialize(content, "hd");
            UserInterface.Active.WhileMouseHoverOrDown = (Entity e) => { Hovering = true; };
        }

        internal void Reset()
        {
            UserInterface.Active.Clear();
        }

        internal void CreateUI(Texture2D MainMenuLogo)
        {
            mainMenu = new MainMenu(MainMenuLogo);

            mainMenu.MainToIntro.OnClick = (Entity button) => Game1.GameState = GameState.Intro;
            mainMenu.MainToLoad.OnClick = (Entity button) => { SetLoadMenuVisiblity(true);  SetMainMenuVisiblity(false); };
            mainMenu.MainToQuit.OnClick = (Entity button) => Game1.GameState = GameState.Exiting;

            UserInterface.Active.AddEntity(mainMenu);

            loadMenu = new LoadMenu<S>()
            {
                Visible = false
            };
            loadMenu.LoadToMain.OnClick = (Entity button) => { SetLoadMenuVisiblity(false); SetMainMenuVisiblity(true); };
            UserInterface.Active.AddEntity(loadMenu);
        }

        internal void CreateLoadMenu()
        {
            loadMenu = new LoadMenu<S>();
            UserInterface.Active.AddEntity(loadMenu);
        }

        internal void AddLoadSaveGame(string name, TimeSpan playTime, int numberOfTowersCaptured, ButtonHandler<S> onLoad, ButtonHandler<S> onDelete)
        {
            loadMenu.AddLoadSaveGame(
                name: name,
                playTime: playTime,
                numberOfTowersCaptured: numberOfTowersCaptured,
                onClick: onLoad,
                onDelete: onDelete
                );
        }

        internal void RemoveSaveGame(S tag)
        {
            loadMenu.RemoveSave(tag);
        }

        internal void SetUIScale(float scale)
        {
            UserInterface.Active.GlobalScale = scale;
        }

        internal void Update(GameTime gameTime)
        {
            Hovering = false;
            UserInterface.Active.Update(gameTime);
        }

        internal void SetMainMenuVisiblity(bool show) => mainMenu.Visible = show;
        internal void SetLoadMenuVisiblity(bool show) => loadMenu.Visible = show;

        internal void Draw(SpriteBatch spriteBatch)
        {
            UserInterface.Active.Draw(spriteBatch);
        }
    }
}
