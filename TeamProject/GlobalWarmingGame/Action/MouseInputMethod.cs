using Engine;
using GlobalWarmingGame;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.Action
{

    /// <summary>
    /// This class manages the selection and instruction of interactable <see cref="GameObject"/>s using mouse input
    /// </summary>
    class MouseInputMethod : SelectionInputMethod
    {
        private readonly Camera camera;
        private readonly Desktop desktop;
        private Instruction currentInstruction;

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="camera">The current camera view, required for translating MouseState point into game world Vector2s</param>
        /// <param name="desktop">Myra UI desktop, required for mouse event handling</param>
        /// <param name="currentInstruction">The current instruction</param>
        public MouseInputMethod(Camera camera, Desktop desktop, Instruction currentInstruction)
        {
            this.camera = camera;
            this.desktop = desktop;
            this.currentInstruction = currentInstruction;
            desktop.TouchDown += (s, a) => OnClick();
        }

        private void OnClick()
        {
            Vector2 clickPos = Vector2.Transform(desktop.TouchPosition.ToVector2(), camera.InverseTransform);
            GameObject objectClicked = ObjectClicked(clickPos.ToPoint());

            if (desktop.ContextMenu != null)
            {
                return;
            }

            var container = new VerticalStackPanel
            {
                Spacing = 4
            };

            var titleContainer = new Panel
            {
                Background = DefaultAssets.UISpritesheet["button"]
            };

            var titleLabel = new Label
            {
                Text = "Choose Action",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            titleContainer.Widgets.Add(titleLabel);
            container.Widgets.Add(titleContainer);

            var verticalMenu = new VerticalMenu();

            //Menu Items
            
            {
                var moveMenuItem = new MenuItem()
                {
                    Id = "move",
                    Text = "Move Here",
                };
                moveMenuItem.Selected += (s, a) =>
                {
                    currentInstruction.ActiveMember.AddGoal(clickPos);
                };

                verticalMenu.Items.Add(moveMenuItem);

                if (objectClicked is IInteractable)
                {
                    foreach (InstructionType t in ((IInteractable)objectClicked).InstructionTypes)
                    {
                        var newMenuItem = new MenuItem()
                        {
                            Id = t.ID,
                            Text = t.Name
                        };
                        newMenuItem.Selected += (s, a) =>
                        {
                            UpdateInstruction(t, (IInteractable)objectClicked);
                            
                        };

                        verticalMenu.Items.Add(newMenuItem);
                    }
                }

                verticalMenu.Items.Add(new MenuItem()
                {
                    Id = "no",
                    Text = "Do Nothing"
                });
            }

            container.Widgets.Add(verticalMenu);

            desktop.ShowContextMenu(container, desktop.TouchPosition);
        }

        private void UpdateInstruction(InstructionType type, IInteractable interactable)
        {
            currentInstruction.Type = type;


            if(interactable is Colonist && type.ID == "select")
            {
                currentInstruction.ActiveMember = (Colonist) interactable;
            } else
            {
                currentInstruction.PassiveMember = interactable;
                currentInstruction.ActiveMember.AddInstruction(currentInstruction);
                currentInstruction = new Instruction(currentInstruction.ActiveMember);
            }

             
            
        }

    }
}