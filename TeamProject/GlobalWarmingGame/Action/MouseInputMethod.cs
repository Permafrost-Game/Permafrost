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

        bool hovering;
        
        public Panel Menu { get; private set; }

        public Texture2D BuildingTexture { get; set; }
        public int BuildingType { get; set; }

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

            UserInterface.Active.WhileMouseHoverOrDown = (Entity e) => { hovering = true; };
        }

        void OnClick()
        {
            if (!hovering)
            {
                Vector2 positionClicked = Vector2.Transform(currentMouseState.Position.ToVector2(), camera.InverseTransform);
                GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());
                Tile tileClicked = tileMap.GetTileAtPosition(positionClicked);

                if (tileClicked != null)
                {
                    if (Menu != null && Menu.Visible == true)
                        Menu.Visible = false;

                    Menu = new Panel(new Vector2(150, 200), PanelSkin.Default, Anchor.TopLeft, new Vector2(currentMouseState.X, currentMouseState.Y));
                    UserInterface.Active.AddEntity(Menu);

                    Label label = new Label("Choose Action", Anchor.TopCenter, new Vector2(500, 50));
                    label.Scale = 0.7f;
                    Menu.AddChild(label);

                    Button button1 = new Button("Move Here", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25));
                    button1.ButtonParagraph.Scale = 0.5f;
                    Menu.AddChild(button1);
                    button1.OnClick = (Entity btn) =>
                    {
                        if (objectClicked != null)
                            currentInstruction.ActiveMember.AddGoal(objectClicked.Position);

                        else
                            currentInstruction.ActiveMember.AddGoal(tileClicked.Position);

                        Menu.Visible = false;
                    };

                    if (objectClicked is IInteractable)
                    {
                        foreach (InstructionType t in ((IInteractable)objectClicked).InstructionTypes)
                        {
                            Button button2 = new Button(t.Name, ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                            button2.ButtonParagraph.Scale = 0.5f;
                            Menu.AddChild(button2);
                            button2.OnClick = (Entity btn) => { UpdateInstruction(t, (IInteractable)objectClicked); Menu.Visible = false; };
                        }
                    }

                if (BuildingTexture != null)
                {
                    var buildMenuItem = new MenuItem()
                    {
                        Id = "build",
                        Text = "Build Here",
                    };
                    buildMenuItem.Selected += (s, a) =>
                    {
                        currentInstruction.ActiveMember.Build(clickPos, BuildingTexture, BuildingType);
                    };

                    verticalMenu.Items.Add(buildMenuItem);
                }

                verticalMenu.Items.Add(new MenuItem()
                {
                    Id = "no",
                    Text = "Do Nothing"
                });
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

            if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                OnClick();

            hovering = false;

            if (previousMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                Menu.Visible = false;    

            previousMouseState = currentMouseState;
        }
    }
}