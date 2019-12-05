using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;

using GeonBit.UI;
using GeonBit.UI.Entities;


namespace GlobalWarmingGame.Action
{

    /// <summary>
    /// This class manages the selection and instruction of interactable <see cref="GameObject"/>s using mouse input
    /// </summary>
    class MouseInputMethod : SelectionInputMethod
    {
        readonly Camera camera;
        readonly TileMap tileMap;
        Instruction currentInstruction;

        MouseState currentMouseState;
        MouseState previousMouseState;

        bool emptyTile;

        public Panel Screen { get; private set; }
        public Panel Menu { get; private set; }

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="camera">The current camera view, required for translating MouseState point into game world Vector2s</param>
        /// <param name="currentInstruction">The current instruction</param>
        public MouseInputMethod(Camera camera, TileMap tileMap, Instruction currentInstruction)
        {
            this.camera = camera;
            this.tileMap = tileMap;
            this.currentInstruction = currentInstruction;

            Screen = new Panel(new Vector2(0,0), PanelSkin.None, Anchor.Center);

            UserInterface.Active.AddEntity(Screen);

            Screen.OnClick = (Entity btn) => { OnClick(); };
        }

        void OnClick()
        {
            Screen.ClearChildren();

            Vector2 positionClicked = Vector2.Transform(currentMouseState.Position.ToVector2(), camera.InverseTransform);

            if (tileMap.GetTileAtPosition(positionClicked) == null)
                emptyTile = true;
            else
                emptyTile = false;

            if (!emptyTile)
            {
                GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());

                Menu = new Panel(new Vector2(150, 200), PanelSkin.Default, Anchor.TopLeft, new Vector2(currentMouseState.X, currentMouseState.Y));
                Screen.AddChild(Menu);

                Label label = new Label("Choose Action", Anchor.TopCenter, new Vector2(500, 50));
                label.Scale = 0.7f;
                Menu.AddChild(label);

                Button button1 = new Button("Move Here", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25));
                button1.ButtonParagraph.Scale = 0.5f;
                Menu.AddChild(button1);
                button1.OnClick = (Entity btn) => { currentInstruction.ActiveMember.AddGoal(positionClicked); Screen.ClearChildren(); };

                if (objectClicked is IInteractable)
                {
                    foreach (InstructionType t in ((IInteractable)objectClicked).InstructionTypes)
                    {
                        Button button2 = new Button(t.Name, ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                        button2.ButtonParagraph.Scale = 0.5f;
                        Menu.AddChild(button2);
                        button2.OnClick = (Entity btn) => { UpdateInstruction(t, (IInteractable)objectClicked); Screen.ClearChildren(); };
                    }
                }

                Button button3 = new Button("Do Nothing", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 60));
                button3.ButtonParagraph.Scale = 0.5f;
                Menu.AddChild(button3);
                button3.OnClick = (Entity btn) => { Screen.ClearChildren(); };
            }
        }

        void UpdateInstruction(InstructionType type, IInteractable interactable)
        {
            currentInstruction.Type = type;

            if (interactable is Colonist && type.ID == "select")
                currentInstruction.ActiveMember = (Colonist)interactable;

            else
            {
                currentInstruction.PassiveMember = interactable;
                currentInstruction.ActiveMember.AddInstruction(currentInstruction);
                currentInstruction = new Instruction(currentInstruction.ActiveMember);
            }
        }

        public void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();

            if (CheckMouseClick())
                Screen.ClearChildren();

            previousMouseState = currentMouseState;
        }

        bool CheckMouseClick()
        {
            if (previousMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                return true;

            return false;
        }
    }
}