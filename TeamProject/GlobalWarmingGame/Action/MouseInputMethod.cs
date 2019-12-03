using Engine;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Action
{

    /// <summary>
    /// This class manages the selection and instruction of interactable <see cref="GameObject"/>s using mouse input
    /// </summary>
    class MouseInputMethod : SelectionInputMethod
    {
        readonly Camera camera;
        Instruction currentInstruction;

        Panel screen;
        Panel menu;
        MouseState mouseState;

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="camera">The current camera view, required for translating MouseState point into game world Vector2s</param>
        /// <param name="currentInstruction">The current instruction</param>
        public MouseInputMethod(Camera camera, Instruction currentInstruction)
        {
            this.camera = camera;
            this.currentInstruction = currentInstruction;

            screen = new Panel(new Vector2(camera.Viewport.Width, camera.Viewport.Height), PanelSkin.None, Anchor.Center);
            UserInterface.Active.AddEntity(screen);
            screen.OnClick = (Entity btn) => { OnClick(); };
        }

        void OnClick()
        {
            if (menu != null)
                menu.Visible = false;

            Vector2 positionClicked = Vector2.Transform(mouseState.Position.ToVector2(), camera.InverseTransform);
            GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());

            menu = new Panel(new Vector2(150, 200), PanelSkin.Default, Anchor.TopLeft, new Vector2(mouseState.X, mouseState.Y));
            screen.AddChild(menu);

            Label label = new Label("Choose Action", Anchor.TopCenter, new Vector2(500, 50));
            label.Scale = 0.7f;
            menu.AddChild(label);

            Button button1 = new Button("Move Here", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25));
            button1.ButtonParagraph.Scale = 0.5f;
            menu.AddChild(button1);
            button1.OnClick = (Entity btn) => { currentInstruction.ActiveMember.AddGoal(positionClicked); };

            if (objectClicked is IInteractable)
            {
                foreach (InstructionType t in ((IInteractable)objectClicked).InstructionTypes)
                {
                    Button button2 = new Button(t.Name, ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                    button2.ButtonParagraph.Scale = 0.5f;
                    menu.AddChild(button2);
                    button2.OnClick = (Entity btn) => { UpdateInstruction(t, (IInteractable)objectClicked); menu.Visible = false; };
                }
            }

            Button button3 = new Button("Do Nothing", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 60));
            button3.ButtonParagraph.Scale = 0.5f;
            menu.AddChild(button3);
            button3.OnClick = (Entity btn) => { menu.Visible = false; };

            if(!menu.Visible)
                menu.Visible = true;
        }

        void UpdateInstruction(InstructionType type, IInteractable interactable)
        {
            currentInstruction.Type = type;

            if (interactable is Colonist && type.ID == "select")
            {
                currentInstruction.ActiveMember = (Colonist)interactable;
            }

            else
            {
                currentInstruction.PassiveMember = interactable;
                currentInstruction.ActiveMember.AddInstruction(currentInstruction);
                currentInstruction = new Instruction(currentInstruction.ActiveMember);
            }
        }

        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
        }
    }
}