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
using GlobalWarmingGame.UI.Menus;

namespace GlobalWarmingGame.Action
{

    /// <summary>
    /// This class manages the selection and instruction of interactable <see cref="GameObject"/>s using mouse input
    /// </summary>
    [Obsolete]
    class MouseInputMethod : SelectionInputMethod, IUpdatable
    {
        readonly Camera camera;
        readonly TileMap tileMap;
        Instruction currentInstruction;

        MainUI mainUI;
        GameTime time;

        MouseState currentMouseState;
        MouseState previousMouseState;

        public bool buildingSelected;
        int buildingId;

        bool hovering;

        public Panel Menu { get; private set; }
        public Panel CraftingMenu { get; private set; }
        public Panel ResourceNotification { get; private set; }

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
                    button1.OnClick = (Entity btn) =>
                    {
                        if (objectClicked != null)
                            ((Colonist)currentInstruction.ActiveMember).Goals.Enqueue(objectClicked.Position);
                        else
                            ((Colonist)currentInstruction.ActiveMember).Goals.Enqueue(tileClicked.Position);
                    };

                    if (objectClicked is IInteractable)
                    {
                        if (((IInteractable)objectClicked).InstructionTypes.Count != 0)
                        {
                            if (((IInteractable)objectClicked).InstructionTypes.Count == 1)
                            {
                                InstructionType instructionType = ((IInteractable)objectClicked).InstructionTypes.ToArray()[0];

                                button2.OnClick = (Entity btn) =>
                                {
                                    currentInstruction.PassiveMember = objectClicked;
                                    UpdateInstruction(instructionType, (IInteractable)objectClicked);
                                };
                            }

                            else
                            {
                                button2.OnClick = (Entity btn) =>
                                {
                                    int counter = 0;
                                    foreach (InstructionType instruction in ((IInteractable)objectClicked).InstructionTypes)
                                    {
                                        instructionButton.OnClick = (Entity e) =>
                                        {
                                            CraftingMenu.Visible = false;
                                            Menu.Visible = false;

                                            UpdateInstruction(instruction, (IInteractable)objectClicked);
                                        };

                                        counter++;
                                    }
                                };
                            }
                        }
                    }

                    if (buildingSelected)
                    {
                        button4.OnClick = (Entity btn) =>
                        {
                            if (objectClicked == null && tileClicked.Walkable)
                            {
                                PlaceBuildingHelper(tileClicked);
                            }
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

            mainUI.BuildMenu.SelectedIndex = 0;
        }

        bool CanColonistBuild(List<ResourceItem> buildingCosts)
        {
            IInstructionFollower colonist = currentInstruction.ActiveMember;
            bool build = false;

            if (colonist.Inventory.CheckContainsList(buildingCosts))
            {
                foreach (ResourceItem item in buildingCosts)
                    colonist.Inventory.RemoveItem(item);

                build = true;
                ResourceNotification.Visible = false;
            }

            else
            {
                Label label = new Label("Not Enough Resources", Anchor.Center);
                ResourceNotification.AddChild(label);

                ResourceNotification.Visible = true;
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

        [Obsolete]
        void UpdateInstruction(InstructionType type, IInteractable interactable)
        {
            currentInstruction.Type = type;

            if (interactable is Colonist && type.ID == "select")
                currentInstruction.ActiveMember = (Colonist)interactable;

            else
            {
                currentInstruction.PassiveMember = (GameObject) interactable;
                ((Colonist)currentInstruction.ActiveMember).AddInstruction(currentInstruction);
                currentInstruction = new Instruction(currentInstruction.ActiveMember);
            }
        }

        public void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();

            if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                OnClick();

            hovering = false;

            previousMouseState = currentMouseState;
        }
    }
}