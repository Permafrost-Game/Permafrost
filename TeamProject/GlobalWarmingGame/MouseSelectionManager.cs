using Engine;
using GlobalWarmingGame.Action;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    class MouseSelectionManager
    {
        MouseState lastMouseState;
        Instruction currentInstruction;

        Camera camera;

        public MouseSelectionManager(Camera camera)
        {
            this.camera = camera;
            currentInstruction = new Instruction();
        }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            mousePosition = Vector2.Transform(mousePosition, camera.InverseTransform);

            /* testing code
            Console.WriteLine(camera.Transform);
            Console.WriteLine(camera.InverseTransform);
            */

            if (mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton != mouseState.RightButton)
            {
                //Action Instruction
                GameObject selected = ObjectClicked(mousePosition.ToPoint());
                if (selected != null)
                {
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
                if (currentInstruction.ActiveMember != null)
                {
                    currentInstruction.ActiveMember.AddGoal(mousePosition);
                }
            }

            lastMouseState = mouseState;
        }


        private GameObject ObjectClicked(Point position)
        {
            foreach (GameObject clickable in GameObjectManager.Clickable)
            {
                if (new Rectangle(clickable.Position.ToPoint(), clickable.Size.ToPoint()).Contains(position))
                {
                    return clickable;
                }
            }
            return null;
        }

    }
}