using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;

using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework.Graphics;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using System.Collections.Generic;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Menus;

namespace GlobalWarmingGame.Action
{

    /// <summary>
    /// This class manages the selection and instruction of interactable <see cref="GameObject"/>s using mouse input
    /// </summary>
    class MouseInputMethod : SelectionInputMethod, IUpdatable
    {
        readonly Camera camera;
        readonly TileMap tileMap;
        Instruction currentInstruction;

        MainUI mainUI;

        MouseState currentMouseState;
        MouseState previousMouseState;

        public bool buildingSelected;
        int buildingId;

        bool hovering;

        public Panel Menu { get; private set; }

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="camera">The current camera view, required for translating MouseState point into game world Vector2s</param>
        /// <param name="currentInstruction">The current instruction</param>
        public MouseInputMethod(Camera camera, TileMap tileMap, Instruction currentInstruction, MainUI mainUI)
        {
            this.camera = camera;
            this.tileMap = tileMap;
            this.currentInstruction = currentInstruction;
            this.mainUI = mainUI;
            UserInterface.Active.WhileMouseHoverOrDown = (Entity e) => { hovering = true; };

            PopulateBuildMenu();
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
                        if (((IInteractable)objectClicked).InstructionTypes.Count == 1)
                        {
                            foreach (InstructionType t in ((IInteractable)objectClicked).InstructionTypes)
                            {
                                Button button2 = new Button(t.Name, ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                                button2.ButtonParagraph.Scale = 0.5f;
                                Menu.AddChild(button2);
                                button2.OnClick = (Entity btn) => { UpdateInstruction(t, (IInteractable)objectClicked); Menu.Visible = false; };
                            }
                        }

                        else
                        {
                            Button button2 = new Button("Craft Items", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                            button2.ButtonParagraph.Scale = 0.5f;
                            Menu.AddChild(button2);
                            Label craftingMenu = new Label("Choose Item", Anchor.TopCenter, new Vector2(500, 50));

                            Button instructionButton1 = new Button("Craft Items", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                            Button instructionButton2 = new Button("Craft Items", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                            Button instructionButton3 = new Button("Craft Items", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                            Button instructionButton4 = new Button("Craft Items", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                            Button instructionButton5 = new Button("Craft Items", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                            Button instructionButton6 = new Button("Craft Items", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                            Button instructionButton7 = new Button("Craft Items", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                            Button instructionButton8 = new Button("Craft Items", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));

                            button2.OnClick = (Entity btn) => 
                            {
                                Menu.Visible = false;
                                craftingMenu.Scale = 0.7f;
                                Menu.AddChild(craftingMenu);
                                UpdateInstruction(, (IInteractable)objectClicked);     
                            };
                        }           
                    }

                    Button button3 = new Button("Do Nothing", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 60));
                    button3.ButtonParagraph.Scale = 0.5f;
                    Menu.AddChild(button3);
                    button3.OnClick = (Entity btn) => { Menu.Visible = false; };

                    if (buildingSelected)
                    {
                        Button button4 = new Button("Build Here", ButtonSkin.Default, Anchor.Center, new Vector2(125, 25), new Vector2(0, 30));
                        button4.ButtonParagraph.Scale = 0.5f;
                        Menu.AddChild(button4);
                        button4.OnClick = (Entity btn) =>
                        {
                            if (objectClicked == null && tileClicked.Walkable)
                            {
                                PlaceBuildingHelper(tileClicked);
                            }

                            Menu.Visible = false;
                        };
                    }
                }
            }
        }

        void PlaceBuildingHelper(Tile tileClicked)
        {
            Building buildingDetails = BuildingManager.GetBuilding(buildingId);
            List<ResourceItem> buildingCosts = new List<ResourceItem>();

            //TODO make an abstract building class to reduce repeated code
            switch (buildingId)
            {
                case (1):
                    Farm farm = new Farm(tileClicked.Position, buildingDetails.Texture);
                    buildingCosts = farm.CraftingCosts;

                    if (CanColonistBuild(buildingCosts))
                    {
                        //Colonist colonist = currentInstruction.ActiveMember;
                        //colonist.AddGoal(tileClicked.Position);
                        GameObjectManager.Add(farm);
                    }
                    break;

                case (2):
                    WorkBench workBench = new WorkBench(tileClicked.Position, buildingDetails.Texture);
                    buildingCosts = workBench.CraftingCosts;

                    if (CanColonistBuild(buildingCosts))
                    {
                        GameObjectManager.Add(workBench);
                    }
                    break;
            }
        }

        bool CanColonistBuild(List<ResourceItem> buildingCosts)
        {
            Colonist colonist = currentInstruction.ActiveMember;
            bool build = false;

            if (colonist.Inventory.CheckContainsList(buildingCosts))
            {
                foreach (ResourceItem item in buildingCosts)
                {
                    colonist.Inventory.RemoveItem(item);
                    Console.WriteLine("Removed " + item.Type.DisplayName + " amount: " + item.Amount);
                }
                build = true;
            }
            return build;
        }

        void PopulateBuildMenu()
        {
            string[] buildings = BuildingManager.GetBuildingStrings();

            for (int i = 0; i < buildings.Length; i++)
                mainUI.BuildMenu.AddItem(buildings[i]);

            mainUI.BuildMenu.OnValueChange = (Entity e) =>
            {
                switch (mainUI.BuildMenu.SelectedIndex)
                {
                    case 0:
                        buildingSelected = false;
                        buildingId = 0;
                        break;
                    case 1:
                        buildingSelected = true;
                        buildingId = 1;
                        break;
                    case 2:
                        buildingSelected = true;
                        buildingId = 2;
                        break;
                }
            };
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

            if (Menu != null && Menu.Visible)
                if (previousMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
                    Menu.Visible = false;

            previousMouseState = currentMouseState;
        }
    }
}

