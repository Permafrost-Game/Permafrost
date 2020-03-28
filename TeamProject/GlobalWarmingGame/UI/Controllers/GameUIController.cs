using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.ResourceItems;
using GlobalWarmingGame.Resources;
using GlobalWarmingGame.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalWarmingGame.UI.Controllers
{
    /// <summary>
    /// The controller class controlls the logic behind the UI<br/>
    /// Communicates with the <see cref="View"/> to display menus<br/>
    /// This is the only class that should interface with <see cref="View"/><br/>
    /// </summary>
    static class GameUIController
    {
        private static GameView view;

        static GameUIController()
        {
            openInventories = new List<Inventory>();
        }

        public static void Initalise(ContentManager content)
        {
            view = new GameView();
            view.Initalise(content);
        }

        public static void LoadContent(ContentManager content)
        {
            colonistInventoryIcon = content.Load<Texture2D>("textures/icons/colonist");
        }

        public static void CreateUI(float uiScale = 1f)
        {
            openInventories.Clear();
            view.Clear();
            view.SetUIScale(uiScale);

            view.CreateUI();
            AddDropDowns();
            
            GameObjectManager.ObjectAdded += ObjectAddedEventHandler;
            GameObjectManager.ObjectRemoved += ObjectRemovedEventHandler;
            InitaliseGameObjects();
        }


        #region GameObject Event handlers

        private static void InitaliseGameObjects()
        {
            foreach (GameObject o in GameObjectManager.Objects)
            {
                ObjectAddedEventHandler(null, o);
            }
        }

        internal static void ClearUI()
        {
            view.Clear();
            SelectedColonist = null;
        }

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

        internal static void ShowPauseMenu(bool show = true) => view.SetPauseMenuVisiblity(show);

        internal static void ShowSettingsMenu(bool show = true) => view.SetSettingsMenuVisiblity(show);



        

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

                if (objectClicked is IInteractable interactable)
                {
                    foreach (InstructionType type in interactable.InstructionTypes)
                    {
                        options.Add(new ButtonHandler<Instruction>(new Instruction(type, activeMember, objectClicked), IssueInstructionCallback));
                    }
                }
                else if (objectClicked is Tile tile)
                {

                    if (tile.Walkable)
                    {
                        if (tile.Position.X == 0)
                            options.Add(CreateTravelOption(objectClicked, new Vector2(-1, 0)));
                            
                        else if (tile.Position.Y == 0)
                            options.Add(CreateTravelOption(objectClicked, new Vector2(0, -1)));

                        else if (tile.Position.X >= ((GameObjectManager.ZoneMap.Size.X - 1) * GameObjectManager.ZoneMap.TileSize.X))
                            options.Add(CreateTravelOption(objectClicked, new Vector2(1, 0)));
                        
                        else if (tile.Position.Y >= ((GameObjectManager.ZoneMap.Size.Y - 1) * GameObjectManager.ZoneMap.TileSize.Y))
                            options.Add(CreateTravelOption(objectClicked, new Vector2(0, 1)));

                        else
                            options.Add(new ButtonHandler<Instruction>(new Instruction(WALK_INSTRUCTION_TYPE, activeMember, objectClicked), IssueInstructionCallback));

                        if (constructingMode)
                        {
                            building = (IBuildable)InteractablesFactory.MakeInteractable(SelectedBuildable, objectClicked.Position);

                            options.Add(new ButtonHandler<Instruction>(new Instruction(new InstructionType("build", "Build", "Build the " + SelectedBuildable.ToString(), 0, requiredResources: building.CraftingCosts, onComplete: Build),
                                                                                       activeMember,
                                                                                       (GameObject)building), IssueInstructionCallback));
                        } 
                    }
                }

                if (objectClicked is Colonist)
                {
                    options.Add(new ButtonHandler<Instruction>(new Instruction(COLONIST_INSTRUCTION_TYPE, activeMember, objectClicked), SelectColonistCallback));
                } 
                else if (objectClicked is IStorage)
                {
                    options.Add(new ButtonHandler<Instruction>(new Instruction(VIEW_INVENTORY, passiveMember: objectClicked), ViewInventoryCallback));
                }
            }

            return options;
        }

        private static ButtonHandler<Instruction> CreateTravelOption(GameObject objectClicked, Vector2 translation)
        {
            List<Colonist> colonists = GameObjectManager.Filter<Colonist>();
            HashSet<Colonist> readyToTravel = new HashSet<Colonist>();
            return new ButtonHandler<Instruction> (
                    tag: new Instruction(type: TravelInstruction(null), passiveMember: objectClicked),
                    action: (Instruction instruction) =>
                    {
                        foreach (Colonist c in colonists)
                        {
                            IssueInstructionCallback(new Instruction(
                                type: TravelInstruction((Instruction i) => {
                                    readyToTravel.Add(c);
                                    c.ClearInstructions();
                                    if (readyToTravel.Count == colonists.Count)
                                    {
                                        GameObjectManager.MoveZone(translation);
                                    }
                                }),
                                activeMember: c,
                                passiveMember: objectClicked)
                                );
                        }
                    }
                );
        }
            

        private static InstructionType TravelInstruction(InstructionEvent e) => new InstructionType("travel", "Travel", "Travel to the next zone", onComplete: e, priority: Int16.MinValue);


        /// <summary>
        /// Adds the instruction to the active member of the instruction.
        /// Checks if a instruction has any required resources and if so the instruction will
        /// display a notification to the user if they dont have enough resources.
        /// </summary>
        /// <param name="instruction">the instruction to be issued</param>
        private static void IssueInstructionCallback(Instruction instruction)
        {
            instruction.ActiveMember.AddInstruction(instruction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instruction"></param>
        private static void ViewInventoryCallback(Instruction instruction)
        {

            if (instruction.PassiveMember is IStorage storage)
            {
                ShowInventory(storage);
            }
            else
            {
                throw new InvalidInstructionMemberTypeException(instruction, instruction.PassiveMember, typeof(IStorage));
            }
        }

        private static void ShowInventory(IStorage storage)
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
            view.CreateDropDown("Building", new List<ButtonHandler<Interactable>>
            {
                new ButtonHandler<Interactable>(Interactable.CampFire,  SelectBuildableCallback),
                new ButtonHandler<Interactable>(Interactable.Farm,      SelectBuildableCallback),
                new ButtonHandler<Interactable>(Interactable.WorkBench, SelectBuildableCallback)
            });

            //Spawnables drop down
            view.CreateDropDown("Spawn", Enum.GetValues(typeof(Interactable)).Cast<Interactable>()
                .Select(i => new ButtonHandler<Interactable>(i, SpawnInteractableCallback)).ToList());
        }

        internal static void ResourceNotification(Instruction instruction)
        {
            view.Notification($"Resources Required to {instruction.Type.Name}:", instruction.Type.RequiredResources);
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
            view.Update(gameTime);
            currentMouseState = Mouse.GetState();

            switch(Game1.GameState)
            {
                case GameState.Playing:
                case GameState.Paused:
                    Vector2 screenHover = currentMouseState.Position.ToVector2();
                    Vector2 gameHover = Vector2.Transform(screenHover, Camera.InverseTransform);
                    if (previousMouseState.LeftButton == ButtonState.Released
                        && currentMouseState.LeftButton == ButtonState.Pressed
                        && Game1.GameState == GameState.Playing) 
                        OnClick(gameHover);
                    UpdateTemperature(gameHover, screenHover);
                    break;
                
            }
            

            previousMouseState = currentMouseState;

        }

        private static void UpdateTemperature(Vector2 gameHover, Vector2 screenHover)
        {
            Tile t = GameObjectManager.ZoneMap.GetTileAtPosition(gameHover);
            string temp = string.Empty;

            if (t != null && !view.Hovering)
            {
                int temperature = (int)Math.Round(t.Temperature.Value);
                if (temperature == 0)
                    temp = "±";
                 else if (temperature > 0)
                    temp = "+";

                temp += $"{temperature}°C";
            }
            view.UpdateTemp(temp, screenHover);
        }

        /// <summary>
        /// Draws UI, calls <see cref="View.Draw"/>
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            view.Draw(spriteBatch);
        }

        /// <summary>
        /// Called on a mouse click
        /// </summary>
        private static void OnClick(Vector2 positionClicked)
        {
            if (!view.Hovering)
            {
                GameObject objectClicked = ObjectClicked(positionClicked.ToPoint());

                if (objectClicked == null)
                {
                    Vector2 bounds = new Vector2(((GameObjectManager.ZoneMap.Size.X) * GameObjectManager.ZoneMap.TileSize.X) , (GameObjectManager.ZoneMap.Size.Y) * GameObjectManager.ZoneMap.TileSize.Y);
                    Vector2 tileSize = GameObjectManager.ZoneMap.Tiles[0, 0].Size;

                    Vector2 newPositionClicked = new Vector2(positionClicked.X + (tileSize.X / 2), positionClicked.Y + (tileSize.Y / 2));

                    if (newPositionClicked.Y >= 0 && newPositionClicked.Y < bounds.Y)
                    {
                        if (newPositionClicked.X < 0)
                            objectClicked = ObjectClicked(new Vector2(0, positionClicked.Y).ToPoint());

                        else if (newPositionClicked.X > bounds.X)
                            objectClicked = ObjectClicked(new Vector2(bounds.X - tileSize.X, positionClicked.Y).ToPoint());
                    }

                    else if (newPositionClicked.X >= 0 && newPositionClicked.X < bounds.X)
                    {
                        if (newPositionClicked.Y < 0)
                            objectClicked = ObjectClicked(new Vector2(positionClicked.X, 0).ToPoint());

                        else if (newPositionClicked.Y > bounds.Y)
                            objectClicked = ObjectClicked(new Vector2(positionClicked.X, bounds.Y - tileSize.Y).ToPoint());
                    }
                }

                List<ButtonHandler<Instruction>> options = GenerateInstructionOptions(objectClicked, SelectedColonist);
                if (options != null && (options.Count > 0 || objectClicked is IInteractable))
                {
                    view.CreateMenu("Choose Action", currentMouseState.Position, options);
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
        private static void UpdateInventoryMenu(IStorage storage)
        {
            Inventory inventory = storage.Inventory;
            IEnumerable<ItemElement> ItemElements = inventory.Resources.Values.Select(i => new ItemElement(i.ResourceType.Texture, i.Weight.ToString()));
            view.UpdateInventoryMenu(inventory.GetHashCode(), ItemElements);
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
                view.AddInventory(new ButtonHandler<Inventory>(storage.Inventory, SelectInventory), icon: icon);
                openInventories.Add(storage.Inventory);
                storage.InventoryChange += InventoryChangeCallBack;
                UpdateInventoryMenu(storage);
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
                storage.InventoryChange -= InventoryChangeCallBack;
                view.RemoveInventory(storage.Inventory.GetHashCode());
            }
            
        }

        /// <summary>
        /// Callback for <see cref="Inventory.InventoryChange"/> Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void InventoryChangeCallBack(object sender, ResourceItem item)
        {
            string op = item.Weight >= 0 ? "+" : "";
            GameObjectManager.Add(new InventoryTransactionMessage((GameObject) sender, Camera, $"{op} {item.Weight} {item.ResourceType.displayName}"));
            UpdateInventoryMenu((IStorage)sender);
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
            view.SetInventoryVisiblity(inventory.GetHashCode());
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
