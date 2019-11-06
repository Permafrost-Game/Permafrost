using Engine;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    class MouseSelectionManager
    {

        MouseState lastMouseState;
        Instruction currentInstruction;

        public MouseSelectionManager()
        {
            currentInstruction = new Instruction();
        }
        public void Update()
        {
            MouseState mouseState = Mouse.GetState();

            if (mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton != mouseState.RightButton)
            {
                //Action Instruction
                GameObject selected = ObjectClicked(mouseState.Position);
                if (selected != null) {
                    if (selected is Colonist)
                    {
                        currentInstruction.ActiveMember = (Colonist)selected;
                    }
                    else if (selected is Building)
                    {
                        currentInstruction.PassiveMember = (Building)selected;
                    }
                }
            }


            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton != mouseState.LeftButton)
            {
                //Move Instruction
                if (currentInstruction.ActiveMember != null) {
                    currentInstruction.ActiveMember.AddGoal(mouseState.Position.ToVector2());
                }
            }
            lastMouseState = mouseState;


        }


        private GameObject ObjectClicked(Point position)
        {
            foreach(GameObject clickable in GameObjectManager.Clickable)
            {
                if(new Rectangle(clickable.Position.ToPoint(), clickable.Size.ToPoint()).Contains(position))
                {
                    return clickable;
                }
            }
            return null;
        }

    }
}
