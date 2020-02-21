using Engine;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.UI
{
    /// <summary>
    /// The controller class controlls the logic behind the UI<br/>
    /// Communicates with the <see cref="View"/> to display menus<br/>
    /// eg.<br/>
    /// When the user clicks, this class will tell View to create a new menu, view is in charge of the GUI specific logic.<br/>
    /// This class defines what UI needs to exist.<br/>
    /// </summary>
    static class Controller
    {

        private static Texture2D colonistInventoryIcon;
        

        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        private static readonly List<Inventory> openInventories;

        static Controller()
        {
            openInventories = new List<Inventory>();
            
            GameObjectManager.ObjectAdded += ObjectAddedEventHandler;
            GameObjectManager.ObjectRemoved += ObjectRemovedEventHandler;
        }

        public static void LoadContent(ContentManager content)
        {
            View.Initialize(content);
            colonistInventoryIcon = content.Load<Texture2D>("textures/icons/colonist");

            AddDropDowns();
        }



        public static void UpdateColonist()
        {
            openInventories.Clear();
            foreach (Colonist colonist in GameObjectManager.Filter<Colonist>())
            {
                if (SelectedColonist == null)
                {
                    SelectedColonist = colonist;
                }

                AddInventoryMenu(colonist);
            }
        }
        public static void ObjectAddedEventHandler(object sender, GameObject GameObject)
        {
            if(GameObject is Colonist colonist)
            {
                if (SelectedColonist == null)
                {
                    SelectedColonist = colonist;
                }
                AddInventoryMenu(colonist);
            }
        }

        public static void ObjectRemovedEventHandler(object sender, GameObject GameObject)
        {
            if (GameObject is Colonist colonist)
            {
                RemoveInventoryMenu(colonist);
            }
        }

        #region Instruction Menu


        private static readonly InstructionType WALK_INSTRUCTION_TYPE = new InstructionType("walk", "Walk", "Walk here");
        private static readonly InstructionType COLONIST_INSTRUCTION_TYPE = new InstructionType("selectColonist", "Select", "Selects the colonist");
        private static readonly InstructionType VIEW_INVENTORY = new InstructionType("viewInventory", "View Inventory", "Opens the inventory");

        /// <summary>The currently selected colonist that instructions will be given</summary>
        public static Colonist SelectedColonist { get; set; }

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
                options.Add(new ButtonHandler<Instruction>(new Instruction(WALK_INSTRUCTION_TYPE, colonist, objectClicked), IssueInstructionCallback));

                if (objectClicked is IInteractable interactable)
                {
                    foreach (InstructionType type in interactable.InstructionTypes)
                    {
                        options.Add(new ButtonHandler<Instruction>(new Instruction(type, colonist, objectClicked), IssueInstructionCallback));
                    }
                }

                if (objectClicked is Colonist)
                {
                    options.Add(new ButtonHandler<Instruction>(new Instruction(COLONIST_INSTRUCTION_TYPE, colonist, objectClicked), SelectColonistCallback));
                } else if (objectClicked is IStorage)
                {
                    options.Add(new ButtonHandler<Instruction>(new Instruction(VIEW_INVENTORY, null, objectClicked), ViewInventoryCallback));
                }


                //If the colonist is allowed to start constructing a building
                if (constructing)
                {
                    //Building gets fed a Interactable that is also a buildable
                    building = (IBuildable)InteractablesFactory.MakeInteractable(SelectedBuildable, objectClicked.Position);

                    //If Colonist has the resources build
                    if (colonist.Inventory.ContainsAll(building.CraftingCosts))
                    {
                        InstructionType construct = new InstructionType("build", "Build", "Build the " + SelectedBuildable.ToString(), onStart: Build);
                        options.Add(new ButtonHandler<Instruction>(new Instruction(construct, colonist, (GameObject)building), IssueInstructionCallback));
                    }
                    //TODO Else show notification that the colonist can't craft the building
                }
            }

            return options;
        }

        /// <summary>
        /// Adds the instruction to the active member of the instruction.
        /// </summary>
        /// <param name="instruction">the instruction to be issued</param>
        private static void IssueInstructionCallback(Instruction instruction)
        {
            instruction.ActiveMember.AddInstruction(instruction, 0);
        }

        /// <summary>
        /// Adds the instruction to the active member of the instruction.
        /// </summary>
        /// <param name="instruction">the instruction to be issued</param>
        private static void ViewInventoryCallback(Instruction instruction)
        {

            if (instruction.PassiveMember is IStorage storage)
            {
                if (!openInventories.Contains(storage.Inventory))
                {
                    AddInventoryMenu(storage);
                }
                else
                {
                    ToggleInventoryVisibility(storage.Inventory);
                }
            }
            else
            {
                throw new InvalidInstructionMemberTypeException(instruction, instruction.PassiveMember, typeof(IStorage));
            }
        }

        /// <summary>
        /// Takes the passive member of the instruction and sets that as the active colonist<br>
        /// This is meant to be used to select a colonist
        /// </summary>
        /// <param name="instruction">The instruction that has been selected</param>
        private static void SelectColonistCallback(Instruction instruction)
        {
            SelectedColonist = (Colonist)instruction.PassiveMember;
            ViewInventoryCallback(instruction);
        }


        #endregion

        #region Drop-Down Menu

        private static bool constructing = false;
        private static IBuildable building;
        public static Interactable SelectedBuildable { get; set; }
        public static Camera Camera { get => GameObjectManager.Camera; }

        /// <summary>
        /// Adds the Building and Spawn dropdown menus to the view
        /// </summary>
        private static void AddDropDowns()
        {
            //Buildings drop down
            View.CreateDropDown("Building", new List<ButtonHandler<Interactable>>
            {
                new ButtonHandler<Interactable>(Interactable.CampFire,  SelectBuildableCallback),
                new ButtonHandler<Interactable>(Interactable.Farm,      SelectBuildableCallback),
                new ButtonHandler<Interactable>(Interactable.WorkBench, SelectBuildableCallback)
            });

            //Spawnables drop down
            View.CreateDropDown("Spawn", Enum.GetValues(typeof(Interactable)).Cast<Interactable>()
                .Select(i => new ButtonHandler<Interactable>(i, SpawnInteractableCallback)).ToList());
        }


        /// <summary>
        /// Selects an Interactable for construction
        /// </summary>
        /// <param name="interactable"></param>
        private static void SelectBuildableCallback(Interactable interactable)
        {
            SelectedBuildable = interactable;
            constructing = true;
        }

        /// <summary>
        /// Creates the interactable
        /// </summary>
        /// <param name="interactable"></param>
        private static void SpawnInteractableCallback(Interactable interactable)
        {
            Vector2 position = ZoneManager.CurrentZone.TileMap.Size * ZoneManager.CurrentZone.TileMap.Tiles[0, 0].size - Camera.Position;
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
            if (follower.Inventory.ContainsAll(buildingCosts))
            {
                foreach (ResourceItem item in buildingCosts)
                    follower.Inventory.RemoveItem(item);

                building.Build();
            }
            //Else show notification that the colonist can't craft the building
            constructing = false;
        }
        #endregion

        #region Update loop

        public static void Update(GameTime gameTime)
        {
            View.Update(gameTime);
            currentMouseState = Mouse.GetState();

            if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                OnClick();

            previousMouseState = currentMouseState;

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            View.Draw(spriteBatch);
        }

        /// <summary>
        /// Called on a mouse click
        /// </summary>
        private static void OnClick()
        {
            if (!View.Hovering)
            {
                Vector2 positionClicked = Vector2.Transform(currentMouseState.Position.ToVector2(), Camera.InverseTransform);
                GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());

                List<ButtonHandler<Instruction>> options = GenerateInstructionOptions(objectClicked, SelectedColonist);
                if (options != null)
                {
                    View.CreateMenu(currentMouseState.Position, options);
                }
            }
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
        #endregion


        private static void UpdateInventoryMenu(Inventory inventory)
        {
            IEnumerable<ItemElement> ItemElements = inventory.Resources.Values.Select(i => new ItemElement(i.ResourceType.Texture, i.Weight.ToString()));
            View.UpdateInventoryMenu(inventory.GetHashCode(), ItemElements);
        }

        private static void AddInventoryMenu(IStorage storage)
        {
            if (!openInventories.Contains(storage.Inventory))
            {
                Texture2D icon = storage is Colonist ? colonistInventoryIcon : null;
                View.AddInventory(new ButtonHandler<Inventory>(storage.Inventory, ToggleInventoryVisibility), icon: icon);
                openInventories.Add(storage.Inventory);
                storage.Inventory.InventoryChange += InventoryChangeCallBack;
                UpdateInventoryMenu(storage.Inventory);
            }
        }

        private static void RemoveInventoryMenu(IStorage storage)
        {
            if(openInventories.Contains(storage.Inventory))
            {
                openInventories.Remove(storage.Inventory);
                storage.Inventory.InventoryChange -= InventoryChangeCallBack;
                View.RemoveInventory(storage.Inventory.GetHashCode());
            }
            
        }

        /// <summary>
        /// Callback for <see cref="Inventory.InventoryChange"/> Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void InventoryChangeCallBack(object sender, EventArgs e)
        {
            UpdateInventoryMenu((Inventory)sender);
        }

        private static void ToggleInventoryVisibility(Inventory inventory)
        {
            View.ToggleInventoryMenuVisibility(inventory.GetHashCode());
        }

    }
}
