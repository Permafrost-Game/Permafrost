using Engine;
using Engine.TileGrid;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// The controller class controlls the logic behind the UI<br>
    /// Communicates with the <see cref="View"/> to display menus<br>
    /// eg.<br>
    /// When the user clicks, this class will tell View to create a new menu, view is in charge of the GUI specific logic.<br>
    /// This class defines what UI needs to exist.
    /// </summary>
    class Controller : IUpdatable
    {

        private View view;

        /// <summary>The currently selected colonist that instructions will be given</summary>
        public static Colonist SelectedColonist { get; set; }
        public static Interactable SelectedBuildable { get; set; }

        /// <summary>Reference to the camera for inverse transforms</summary>
        private Camera camera;

        /// <summary>Whether the mouse is hovering over a menu</summary>
        private bool hovering = false;

        private static bool constructing = false;
        private static IBuildable building;

        private MouseState currentMouseState;
        private MouseState previousMouseState;

        private static readonly InstructionType WALK_INSTRUCTION_TYPE = new InstructionType("walk", "Walk", "Walk here");
        private static readonly InstructionType COLONIST_INSTRUCTION_TYPE = new InstructionType("selectColonist", "Select", "Selects the colonist");

        public Controller(Camera camera)
        {
            this.camera = camera;
            view = new View();
            UserInterface.Active.WhileMouseHoverOrDown = (Entity e) => { hovering = true; };

            //Buildings drop down
            List<ButtonHandler<Interactable>> BuildingButtonHandlers = new List<ButtonHandler<Interactable>>();

            BuildingButtonHandlers.Add(new ButtonHandler<Interactable>(Interactable.CampFire, SelectBuildable));
            BuildingButtonHandlers.Add(new ButtonHandler<Interactable>(Interactable.Farm, SelectBuildable));
            BuildingButtonHandlers.Add(new ButtonHandler<Interactable>(Interactable.WorkBench, SelectBuildable));

            view.CreateDropDown("Building", BuildingButtonHandlers);

            //Spawnables drop down
            List<ButtonHandler<Interactable>> spawnButtonHandlers = new List<ButtonHandler<Interactable>>();
            foreach (Interactable interactable in Enum.GetValues(typeof(Interactable)))
            {
                spawnButtonHandlers.Add(new ButtonHandler<Interactable>(interactable, SpawnInteractable));
            }

            view.CreateDropDown("Spawn", spawnButtonHandlers);
        }

        /// <summary>
        /// Called on a mouse click
        /// </summary>
        private void OnClick()
        {
            if (!hovering)
            {
                Vector2 positionClicked = Vector2.Transform(currentMouseState.Position.ToVector2(), camera.InverseTransform);
                GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());

                List<ButtonHandler<Instruction>> options = GenerateInstructionOptions(objectClicked, SelectedColonist);
                if (options != null)
                {
                    view.CreateMenu(currentMouseState.Position, options);
                }
            }
        }

        /// <summary>
        /// Takes an input GameObject 
        /// </summary>
        /// <param name="objectClicked">the object that was </param>
        /// <returns></returns>
        private static List<ButtonHandler<Instruction>> GenerateInstructionOptions(GameObject objectClicked, Colonist colonist)
        {
            List<ButtonHandler<Instruction>> options = new List<ButtonHandler<Instruction>>();

            if (objectClicked != null)
            {
                options.Add(new ButtonHandler<Instruction>(new Instruction(WALK_INSTRUCTION_TYPE, colonist, objectClicked), IssueInstruction));

                if (objectClicked is IInteractable)
                {
                    foreach (InstructionType type in ((IInteractable)objectClicked).InstructionTypes)
                    {
                        options.Add(new ButtonHandler<Instruction>(new Instruction(type, colonist, objectClicked), IssueInstruction));
                    }
                }

                if (objectClicked is Colonist)
                {
                    options.Add(new ButtonHandler<Instruction>(new Instruction(COLONIST_INSTRUCTION_TYPE, colonist, objectClicked), SelectColonist));
                }

                //If the colonist is allowed to start constructing a building
                if (constructing)
                {
                    //Building gets fed a Interactable that is also a buildable
                    building = (IBuildable)InteractablesFactory.MakeInteractable(SelectedBuildable, objectClicked.Position);

                    //If Colonist has the resources build
                    if (SelectedColonist.Inventory.CheckContainsList(building.CraftingCosts))
                    {
                        InstructionType construct = new InstructionType("build", "Build", "Build the " + SelectedBuildable.ToString(), onStart: Build);
                        options.Add(new ButtonHandler<Instruction>(new Instruction(construct, colonist, (GameObject)building), IssueInstruction));
                    }
                    //Else show notification that the colonist can't craft the building
                }
            }

            return options;
        }

        #region Drop-Down Menu Delegate methods
        /// <summary>
        /// Delegate method to select the right building
        /// </summary>
        /// <param name="buildable"></param>
        private void SelectBuildable(Interactable interactable)
        {
            SelectedBuildable = interactable;
            constructing = true;
        }

        /// <summary>
        /// Delegate method to spawn the right interactable
        /// </summary>
        /// <param name="interactable"></param>
        private void SpawnInteractable(Interactable interactable)
        {
            Vector2 position = ZoneManager.CurrentZone.TileMap.Size * ZoneManager.CurrentZone.TileMap.Tiles[0, 0].size - camera.Position;
            //Map the position onto the nearest tile and then get that tiles position
            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(interactable, ZoneManager.CurrentZone.TileMap.GetTileAtPosition(position).Position));
        }

        /// <summary>
        /// Called when colonist is at the building site and then the building is made visible
        /// </summary>
        /// <param name="follower"></param>
        private static void Build(IInstructionFollower follower)
        {
            List<ResourceItem> buildingCosts = new List<ResourceItem>();

            //If Colonist has the resources build
            if (follower.Inventory.CheckContainsList(buildingCosts))
            {
                foreach (ResourceItem item in buildingCosts)
                    follower.Inventory.RemoveItem(item);

                building.Build(building);
            }
            //Else show notification that the colonist can't craft the building
            constructing = false;
        }
        #endregion

        /// <summary>
        /// Adds the instruction to the active member of the instruction.
        /// </summary>
        /// <param name="instruction">the instruction to be issued</param>
        private static void IssueInstruction(Instruction instruction)
        {
            instruction.ActiveMember.AddInstruction(instruction, 0);
        }

        /// <summary>
        /// Takes the passive member of the instruction and sets that as the active colonist<br>
        /// This is meant to be used to select a colonist
        /// </summary>
        /// <param name="instruction">The instruction that has been selected</param>
        private static void SelectColonist(Instruction instruction)
        {
            SelectedColonist = (Colonist)instruction.PassiveMember;
        }


        public void Update(GameTime gameTime)
        {
            currentMouseState = Mouse.GetState();

            if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                OnClick();

            previousMouseState = currentMouseState;
            hovering = false;
        }


        /// <summary>
        /// Checks to see if a <see cref="Interactions.IInteractable"/> <see cref="GameObject"/> was clicked
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private static GameObject ObjectClicked(Point position)
        {
            foreach (GameObject o in GameObjectManager.Interactable)
            {
                if (new Rectangle(o.Position.ToPoint(), o.Size.ToPoint()).Contains(position))
                {
                    return o;
                }
            }
            return ZoneManager.CurrentZone.TileMap.GetTileAtPosition(position.ToVector2());
        }

    }
}
