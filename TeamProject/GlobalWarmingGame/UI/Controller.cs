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
    /// This is the only class that should interface with <see cref="View"/><br/>
    /// </summary>
    static class Controller
    {

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

        #region Event handlers

        /// <summary>
        /// Handles <see cref="GameObjectManager.ObjectAdded"/><br/>
        /// Checks if a new colonist has been added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="GameObject"></param>
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

        /// <summary>
        /// Handles <see cref="GameObjectManager.ObjectRemoved"/><br/>
        /// Checks if a colonist has been removed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="GameObject"></param>
        public static void ObjectRemovedEventHandler(object sender, GameObject GameObject)
        {
            if (GameObject is Colonist colonist)
            {
                RemoveInventoryMenu(colonist);
            }
        }
        #endregion

        #region Instruction Menu


        private static readonly InstructionType WALK_INSTRUCTION_TYPE = new InstructionType("walk", "Walk", "Walk here");
        private static readonly InstructionType COLONIST_INSTRUCTION_TYPE = new InstructionType("selectColonist", "Select", "Selects the colonist");
        private static readonly InstructionType VIEW_INVENTORY = new InstructionType("viewInventory", "View Inventory", "Opens the inventory");

        /// <summary>The currently selected colonist that instructions will be given</summary>
        public static Colonist SelectedColonist { get; set; }

        /// <summary>
        /// Generates a list of button handlers.<br/>
        /// Will add walk if the object clicked isnt null<br/>
        /// 
        /// </summary>
        /// <param name="objectClicked">the object that was clicked</param>
        /// <param name="activeMember">the colonist and active member of the instructions</param>
        /// <returns></returns>
        private static List<ButtonHandler<Instruction>> GenerateInstructionOptions(GameObject objectClicked, Colonist activeMember)
        {
            List<ButtonHandler<Instruction>> options = new List<ButtonHandler<Instruction>>();

            if (objectClicked != null)
            {
                options.Add(new ButtonHandler<Instruction>(new Instruction(WALK_INSTRUCTION_TYPE, activeMember, objectClicked), IssueInstructionCallback));

                if (objectClicked is IInteractable interactable)
                {
                    foreach (InstructionType type in interactable.InstructionTypes)
                    {
                        options.Add(new ButtonHandler<Instruction>(new Instruction(type, activeMember, objectClicked), IssueInstructionCallback));
                    }
                }
                else if (constructingMode)
                {
                    building = (IBuildable)InteractablesFactory.MakeInteractable(SelectedBuildable, objectClicked.Position);

                    options.Add(new ButtonHandler<Instruction>(new Instruction(new InstructionType("build", "Build", "Build the " + SelectedBuildable.ToString(), 0, building.CraftingCosts, onComplete: Build),
                                                                               activeMember,
                                                                               (GameObject)building), IssueInstructionCallback));
                }

                if (objectClicked is Colonist)
                {
                    options.Add(new ButtonHandler<Instruction>(new Instruction(COLONIST_INSTRUCTION_TYPE, activeMember, objectClicked), SelectColonistCallback));
                } 
                else if (objectClicked is IStorage)
                {
                    options.Add(new ButtonHandler<Instruction>(new Instruction(VIEW_INVENTORY, null, objectClicked), ViewInventoryCallback));
                }
            }

            return options;
        }

        /// <summary>
        /// Adds the instruction to the active member of the instruction.
        /// Checks if a instruction has any required resources and if so the instruction will
        /// display a notification to the user if they dont have enough resources.
        /// </summary>
        /// <param name="instruction">the instruction to be issued</param>
        private static void IssueInstructionCallback(Instruction instruction)
        {
            //Check if the instruction requires resources or craftables
            if (instruction.Type.RequiredResources != null)
            {
                //Check if the colonist has the required resources in their inventory
                if (instruction.ActiveMember.Inventory.ContainsAll(instruction.Type.RequiredResources))
                {
                    instruction.ActiveMember.AddInstruction(instruction, 0);
                }
                else 
                {
                    View.Notification("Missing items:", instruction.Type.RequiredResources);
                }
            }
            else
            {
                instruction.ActiveMember.AddInstruction(instruction, 0);
            }
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
                    SelectInventory(storage.Inventory);
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

        #region Drop-Down Menu and Build logic

        private static bool constructingMode = false;
        private static IBuildable building;
        private static Interactable SelectedBuildable { get; set; }
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
            constructingMode = true;
        }

        /// <summary>
        /// Creates a <see cref="IInteractable"/> from the given <paramref name="interactable"/>, with the position at the center of the screen<br/>
        /// adding it to the game <see cref="GameObjectManager"/>.<br/>
        /// </summary>
        /// <param name="interactable">the <see cref="Interactable"/> that maps to the <see cref="IInteractable"/> to be added</param>
        private static void SpawnInteractableCallback(Interactable interactable)
        {
            Vector2 position = GameObjectManager.ZoneMap.Size * GameObjectManager.ZoneMap.Tiles[0, 0].Size - Camera.Position;
            GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(interactable, GameObjectManager.ZoneMap.GetTileAtPosition(position).Position));
        }

        /// <summary>
        /// Called when colonist is at the building site and then the building is made visible
        /// </summary>
        /// <param name="follower"></param>
        private static void Build(Instruction instruction)
        {
            Colonist colonist = (Colonist) instruction.ActiveMember;
            List<ResourceItem> buildingCosts = instruction.Type.RequiredResources;

            //If Colonist has the resources build
            if (colonist.Inventory.ContainsAll(buildingCosts))
            {
                foreach (ResourceItem item in buildingCosts)
                    colonist.Inventory.RemoveItem(item);

                building.Build();
            }
            constructingMode = false;
        }
        #endregion

        #region Update loop

        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        public static void Update(GameTime gameTime)
        {
            View.Update(gameTime);
            currentMouseState = Mouse.GetState();

            if (previousMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
                OnClick();

            previousMouseState = currentMouseState;

        }
        /// <summary>
        /// Draws UI, calls <see cref="View.Draw"/>
        /// </summary>
        /// <param name="spriteBatch"></param>
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
                if (options != null && (options.Count > 0 || objectClicked is IInteractable))
                {
                    View.CreateMenu("Choose Action", currentMouseState.Position, options);
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
            foreach (GameObject o in GameObjectManager.Interactables)
            {
                if (new Rectangle((o.Position - o.Size / 2).ToPoint(), o.Size.ToPoint()).Contains(position))
                {
                    return o;
                }
            }
            return GameObjectManager.ZoneMap.GetTileAtPosition(position.ToVector2());
        }
        #endregion

        #region Inventory Menu

        /// <summary>The Icon that is to be used when a colonist's inventory is added</summary>
        private static Texture2D colonistInventoryIcon;

        /// <summary>A list of all inventories that have a UI menu</summary>
        private static readonly List<Inventory> openInventories;

        /// <summary>
        /// Converts <paramref name="inventory"/> into a <c>IEnumberable{ItemElemnt}</c><br/>
        /// and calls <see cref="View.UpdateInventoryMenu"/>
        /// </summary>
        /// <param name="inventory">The <see cref="Inventory"/> to be updated</param>
        private static void UpdateInventoryMenu(Inventory inventory)
        {
            IEnumerable<ItemElement> ItemElements = inventory.Resources.Values.Select(i => new ItemElement(i.ResourceType.Texture, i.Weight.ToString()));
            View.UpdateInventoryMenu(inventory.GetHashCode(), ItemElements);
        }

        /// <summary>
        /// Adds the <see cref="IStorage.Inventory"/> belonging to <paramref name="storage"/> to the UI system
        /// </summary>
        /// <param name="storage">The <see cref="IStorage"/> whoes <see cref="Inventory"/> is to be used to create an inventory menu</param>
        private static void AddInventoryMenu(IStorage storage)
        {
            if (!openInventories.Contains(storage.Inventory))
            {
                Texture2D icon = storage is Colonist ? colonistInventoryIcon : null;
                View.AddInventory(new ButtonHandler<Inventory>(storage.Inventory, SelectInventory), icon: icon);
                openInventories.Add(storage.Inventory);
                storage.Inventory.InventoryChange += InventoryChangeCallBack;
                UpdateInventoryMenu(storage.Inventory);
            }
        }

        /// <summary>
        /// Removes the <see cref="IStorage.Inventory"/> belonging to <paramref name="storage"/> from the UI system
        /// </summary>
        /// <param name="storage">The <see cref="IStorage"/> whoes <see cref="Inventory"/> was previously added and now should be removed</param>
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

        /// <summary>
        /// Selects an inventory menu by its <paramref name="index"/>
        /// </summary>
        /// <param name="index"></param>
        public static void SelectInventory(int index)
        {
            if(openInventories.Count - 1 >= index)
            {
                SelectInventory(openInventories[index]);
            }
        }


        /// <summary>
        /// Selects an inventory menu by its associated <paramref name="inventory"/>
        /// </summary>
        /// <param name="inventory"></param>
        private static void SelectInventory(Inventory inventory)
        {
            View.SetInventoryVisiblity(inventory.GetHashCode());
            foreach (Colonist colonist in GameObjectManager.Filter<Colonist>() )
            {
                if(colonist.Inventory == inventory)
                {
                    SelectedColonist = colonist;
                }
            }
        }
        #endregion


    }
}
