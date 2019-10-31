using Engine;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    class MouseSelectionManager
    {

        MouseState lastMouseState;


        public void Update(IEnumerable<GameObject> gameObjects)
        {
            MouseState mouseState = Mouse.GetState();

            
            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton != mouseState.LeftButton)
            {
                //foreach (IClickable o in gameObjects)
                {
                    //o.OnClick();
                }
            }
        }


    }
}
