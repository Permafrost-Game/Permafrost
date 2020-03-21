using GlobalWarmingGame.UI.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// This class is for handling keyboard inputs.<br/>
    /// This is the only class that should be checking for keyboard input.<br/>
    /// </summary>
    class KeyboardInputHandler
    {

        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        private GraphicsDeviceManager graphics;

        public KeyboardInputHandler(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            CheckInput();
        }

        private void CheckInput()
        {
            if (CheckKeyPress(Keys.F11))
                graphics.ToggleFullScreen();

            switch (Game1.GameState)
            {
                case GameState.Playing:
                    for (int keyCode = (int)Keys.D1; keyCode <= (int)Keys.D9; keyCode++)
                    {
                        if (CheckKeyPress((Keys)keyCode))
                        {
                            GameUIController.SelectInventory(keyCode - (int)Keys.D1);
                        }
                    }


                    if (CheckKeyPress(Keys.I))
                        GameObjectManager.MoveZone(new Vector2(0, -1));
                    else if (CheckKeyPress(Keys.K))
                        GameObjectManager.MoveZone(new Vector2(0, 1));
                    else if (CheckKeyPress(Keys.L))
                        GameObjectManager.MoveZone(new Vector2(1, 0));
                    else if (CheckKeyPress(Keys.J))
                        GameObjectManager.MoveZone(new Vector2(-1, 0));

                    if (CheckKeyPress(Keys.Escape))
                        Game1.GameState = GameState.Paused;
                    break;
                case GameState.Paused:
                    if (CheckKeyPress(Keys.Escape))
                        Game1.GameState = GameState.Playing;
                    break;
                case GameState.Intro:
                    if (CheckKeyPress(Keys.Escape))
                        Game1.GameState = GameState.Playing;
                    break;
                
            }
            //else if (CheckKeyPress(Keys.F5))
            //    GameObjectManager.SaveZone();
            //else if (CheckKeyPress(Keys.F9))
            //    GameObjectManager.LoadZone();

        }

        private bool CheckKeyPress(Keys key)
        {
            return previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }
    }
}
