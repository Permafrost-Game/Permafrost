using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.UI.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GlobalWarmingGame.UI.Views
{

    class MainMenuView<S>
    {
        private MainMenu mainMenu;
        private LoadMenu<S,ButtonHandler<S>> loadMenu;

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

        internal void Clear()
        {
            UserInterface.Active.Clear();
        }


        internal delegate void NewGame();
        private NewGame newGame;
        private bool startNewGame = false;
        internal void CreateUI(Texture2D MainMenuLogo, NewGame newGame)
        {
            UserInterface.Active.UseRenderTarget = true;
            this.newGame = newGame;
            mainMenu = new MainMenu(MainMenuLogo);

            mainMenu.MainToIntro.OnClick = (Entity button) => { startNewGame = true; };
            mainMenu.MainToLoad.OnClick = (Entity button) => { SetLoadMenuVisiblity(true);  SetMainMenuVisiblity(false); };
            mainMenu.MainToQuit.OnClick = (Entity button) => Game1.GameState = GameState.Exiting;

            UserInterface.Active.AddEntity(mainMenu);

            loadMenu = new LoadMenu<S, ButtonHandler<S>>()
            {
                Visible = false
            };
            loadMenu.LoadToMain.OnClick = (Entity button) => { SetLoadMenuVisiblity(false); SetMainMenuVisiblity(true); };
            UserInterface.Active.AddEntity(loadMenu);
        }

        internal void CreateLoadMenu()
        {
            loadMenu = new LoadMenu<S, ButtonHandler<S>>();
            UserInterface.Active.AddEntity(loadMenu);
        }


        /// <summary>
        /// Adds a load save option
        /// </summary>
        /// <param name="text">The text that is to be displayed</param>
        /// <param name="onLoad"></param>
        /// <param name="onDelete"></param>
        internal void AddLoadSaveGame(string text, ButtonHandler<S> onLoad, ButtonHandler<S> onDelete)
        {
            loadMenu.AddLoadSaveGame(
                text: text,
                onClick: onLoad,
                onDelete: new ButtonHandler<ButtonHandler<S>>(onDelete, CreateDeleteMenu)
                );
        }

        /// <summary>
        /// Creates a <see cref="DeleteConfirmationDialogue"/> screen
        /// </summary>
        /// <param name="onDelete"></param>
        private void CreateDeleteMenu(ButtonHandler<S> onDelete)
        {
            SetLoadMenuVisiblity(false);
            DeleteConfirmationDialogue conf = new DeleteConfirmationDialogue(onDelete.Tag.ToString(), new Vector2(600,400), Anchor.Center);

            conf.Cancel.OnClick = (Entity e) =>
            {
                SetLoadMenuVisiblity(true);
                UserInterface.Active.RemoveEntity(conf);
            };
            conf.Delete.OnClick = (Entity e) =>
            {
                SetLoadMenuVisiblity(true);
                onDelete.action(onDelete.Tag);
                UserInterface.Active.RemoveEntity(conf);
            };
            UserInterface.Active.AddEntity(conf);
        }

        /// <summary>
        /// Removes the UI for a SaveGame
        /// </summary>
        /// <param name="tag"></param>
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
            if(startNewGame)
            {
                startNewGame = false;
                newGame();
            }
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
