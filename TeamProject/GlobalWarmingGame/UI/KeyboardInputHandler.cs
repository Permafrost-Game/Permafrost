using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// This class is for handling keyboard inputs.<br/>
    /// This is the only class that should be checking for keyboard input.<br/>
    /// </summary>
    class KeyboardInputHandler : IUpdatable
    {

        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;


        public void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            CheckInput();

        }

        private void CheckInput()
        {
            for(int keyCode = (int)Keys.D1; keyCode <= (int)Keys.D9; keyCode++)
            {
                if(CheckKeyPress((Keys) keyCode))
                {
                    Controller.SelectInventory(keyCode - (int)Keys.D1);
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
        }

        private bool CheckKeyPress(Keys key)
        {
            return previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }
    }
}
