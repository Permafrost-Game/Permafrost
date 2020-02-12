using System;
using System.Collections.Generic;

using Engine;
using Engine.Lighting;
using Engine.TileGrid;

using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Resources;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.UI.Menus;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.Interactions.Interactables.Environment;
using GlobalWarmingGame.Interactions.Interactables.Animals;
using GlobalWarmingGame.Interactions.Enemies;
using GlobalWarmingGame.UI;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is the main class for the games implementation.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //SelectionManager selectionManager;
        Controller controller;

        TileSet tileSet;
        // TileMap tileMap;

        Camera camera;

        MainMenu MainMenu;
        PauseMenu PauseMenu;
        MainUI MainUI;
        CollectiveInventory CollectiveInventory;

        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        enum GameState { mainmenu, playing, paused }
        GameState gameState;

        List<Light> lightObjects;

        ShadowmapResolver shadowmapResolver;
        QuadRenderComponent quadRender;
        RenderTarget2D screenShadows;
        Texture2D ambiantLight;

        Dictionary<String, Texture2D> icons;

        Texture2D stone;
        Texture2D apple;
        Texture2D wood;
        Texture2D fibers;

        Texture2D axe;
        Texture2D pickaxe;
        Texture2D hoe;

        Texture2D workBench;
        Texture2D logo;
       

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080
            };

            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            gameState = GameState.mainmenu;
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, "hd");
            
            //Removes 60 FPS limit
            this.graphics.SynchronizeWithVerticalRetrace = false;
            base.IsFixedTimeStep = false;

            base.Initialize();
        }

        #region Load Content
        protected override void LoadContent()
        {
            //INITALISING GAME COMPONENTS
            {
                spriteBatch = new SpriteBatch(GraphicsDevice);
                ambiantLight = new Texture2D(GraphicsDevice, 1, 1);
                ambiantLight.SetData(new Color[] { Color.DimGray });
            }

            //LIGHTING
            {
                quadRender = new QuadRenderComponent(this);

                shadowmapResolver = new ShadowmapResolver(GraphicsDevice, quadRender, ShadowmapSize.Size256, ShadowmapSize.Size1024);
                shadowmapResolver.LoadContent(Content);

                lightObjects = new List<Light>() //This code will be replaced
                {
                    //new Light(Vector2.Zero,         GraphicsDevice, 128f, new Color(201,226,255,32), "Light" ),
                    //new Light(new Vector2(256,224), GraphicsDevice, 256f, new Color(255,0  ,0  ,255), "Light" ),
                    //new Light(new Vector2(224,512), GraphicsDevice, 256f, new Color(0,0  ,255  ,255), "Light" )
                };

                screenShadows = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }


            //LOADING TILEMAP AND ZONES
            {
                var textureSet = new Dictionary<int, Texture2D>();

                Texture2D water = this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/water");
                water.Name = "Non-Walkable";

                textureSet.Add(0, this.Content.Load<Texture2D>(@"textures/tiles/old_tileset/error"));
                textureSet.Add(1, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Snow"));
                textureSet.Add(2, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Stone"));
                textureSet.Add(3, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Tundra1"));
                textureSet.Add(4, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Grass"));
                textureSet.Add(5, water);



                tileSet = new TileSet(textureSet, new Vector2(32f));
                //                                                  map0/00.csv  //50x50 tilemap
                //                                                  map1/00.csv  //100x100 tilemap
                //tileMap = TileMapParser.parseTileMap(@"Content/maps/map1/00.csv", tileSet);
                GameObjectManager.Init(tileSet);

                ZoneManager.CurrentZone = new Zone() { TileMap = GameObjectManager.ZoneMap };
                camera = new Camera(GraphicsDevice.Viewport, GameObjectManager.ZoneMap.Size * ZoneManager.CurrentZone.TileMap.Tiles[0, 0].size);

                GameObjectManager.Camera = camera;
            }

            //CREATING GAME OBJECTS
            {
                //All this code below is for testing and will eventually be replaced.

                InteractablesFactory.LoadContent(Content); 
                
               
                axe = this.Content.Load<Texture2D>(@"textures/icons/axe");
                pickaxe = this.Content.Load<Texture2D>(@"textures/icons/pickaxe");
                hoe = this.Content.Load<Texture2D>(@"textures/icons/hoe");
                stone = this.Content.Load<Texture2D>(@"textures/icons/stone");
                wood = this.Content.Load<Texture2D>(@"textures/icons/wood");
                fibers = this.Content.Load<Texture2D>(@"textures/icons/fibers");
                apple = this.Content.Load<Texture2D>(@"textures/icons/apple");
                logo = Content.Load<Texture2D>(@"logo");

                Texture2D[] textureArray = new Texture2D[] { this.Content.Load<Texture2D>(@"textures/interactables/buildings/farm/sprite0"), workBench };
                Texture2D[] iconTextureArray = new Texture2D[] { stone, wood, fibers, apple, axe, pickaxe, hoe };
                string[] iconStringArray = new string[] { "stone", "wood", "fibers", "food", "axe", "pickaxe", "hoe" };
                string[] stringArray = new string[] { "Farm", "Workbench" };

                BuildingManager.AddBuilding(0, "No Building");
                for (int i = 0; i < stringArray.Length; i++)
                    BuildingManager.AddBuilding(i + 1, stringArray[i], textureArray[i]);

                MainMenu = new MainMenu(logo);
                PauseMenu = new PauseMenu();

                icons = new Dictionary<string, Texture2D>(6);

                for (int i = 0; i < iconStringArray.Length; i++)
                    icons.Add(iconStringArray[i], iconTextureArray[i]);

                MainUI = new MainUI(icons);

                controller = new Controller(camera);

                ProcessMenuSelection();
                var c1 = InteractablesFactory.MakeColonist(position: ZoneManager.CurrentZone.TileMap.Size * ZoneManager.CurrentZone.TileMap.Tiles[0, 0].size / 2); 
                
                Controller.SelectedColonist = c1;
                GameObjectManager.Add(c1);

                string[] spawnables = new string[11];
                spawnables[0] = "Colonist";
                spawnables[1] = "Rabbit";
                spawnables[2] = "Farm";
                spawnables[3] = "Tree";
                spawnables[4] = "Bush";
                spawnables[5] = "Workbench";
                spawnables[6] = "Stone";
                spawnables[7] = "Tall Grass";
                spawnables[8] = "Robot";
                spawnables[9] = "Polar Bear";
                spawnables[10] = "campFire";

                for (int i = 0; i < spawnables.Length; i++)
                    MainUI.SpawnMenu.AddItem(spawnables[i]);

                MainUI.SpawnMenu.OnValueChange = (Entity e) => {
                    ProcessSpawnables();
                    //Console.WriteLine(ZoneManager.CurrentZone.TileMap.Size);
                };
                CollectiveInventory = new CollectiveInventory(MainUI, icons);

                MainUI.SpawnMenu.OnValueChange = (Entity e) => { ProcessSpawnables(); };
            }
        }

        protected override void UnloadContent()
        {

        }
        #endregion

        protected override void Update(GameTime gameTime)
        {
            UserInterface.Active.Update(gameTime);

            ShowMainMenu();
            ShowPauseMenu();
            ShowMainUI();
            PauseGame();

            if (gameState == GameState.playing)
            {
                camera.Update(gameTime);

                GameObjectManager.ZoneMap.Update(gameTime);
                BuildingManager.UpdateBuildingTemperatures(gameTime, ZoneManager.CurrentZone.TileMap);
                UpdateColonistTemperatures(gameTime);

                foreach (IUpdatable updatable in GameObjectManager.Updatable)
                    updatable.Update(gameTime);

                controller.Update(gameTime);

                UpdateColonistTemperatures(gameTime);

                CollectiveInventory.UpdateCollectiveInventory(gameTime, MainUI, icons);
                MainUI.UpdateMainUI(CollectiveInventory, gameTime);

                //Uncomment this line for a light around the cursor (uses the first item in lightObjects)
                //lightObjects[0].Position = Vector2.Transform(Mouse.GetState().Position.ToVector2(), camera.InverseTransform);

                base.Update(gameTime);
            }

            if (gameState == GameState.playing)
            {
                if (CheckKeyPress(Keys.I))
                    GameObjectManager.MoveZone(new Vector2(0, -1));
                else if (CheckKeyPress(Keys.K))
                    GameObjectManager.MoveZone(new Vector2(0, 1));
                else if (CheckKeyPress(Keys.L))
                    GameObjectManager.MoveZone(new Vector2(1, 0));
                else if (CheckKeyPress(Keys.J))
                    GameObjectManager.MoveZone(new Vector2(-1, 0));
            }

            //if (gameState == GameState.playing)
            //{
            //    if (currentKeyboardState.IsKeyUp(Keys.Escape) && previousKeyboardState.IsKeyDown(Keys.Escape))
            //        ShowPauseMenu();
            //}

            //peformanceMonitor.Update(gameTime);

            previousKeyboardState = currentKeyboardState;
        }

        #region Update Colonists Temperatures
        void UpdateColonistTemperatures(GameTime gameTime)
        {
            //Adjust the temperatures of the colonists
            foreach (Colonist colonist in GameObjectManager.GetObjectsByTag("Colonist"))
            {
                float tileTemp = GameObjectManager.ZoneMap.GetTileAtPosition(colonist.Position).temperature.Value;

                colonist.UpdateTemp(tileTemp, gameTime);
                //Console.Out.WriteLine(colonist.Temperature.Value + " " + colonist.Health);
            }
        }
        #endregion

        #region Drawing and Lighting
        protected override void Draw(GameTime gameTime)
        {
            //CALCULATE SHADOWS
            foreach (Light light in lightObjects)
            {
                GraphicsDevice.SetRenderTarget(light.RenderTarget);
                GraphicsDevice.Clear(Color.Transparent);
                DrawShadowCasters(light);

                shadowmapResolver.ResolveShadows(light.RenderTarget, light.RenderTarget, light.Position);
            }

            //DRAW LIGHTS
            {
                GraphicsDevice.SetRenderTarget(screenShadows);
                GraphicsDevice.Clear(Color.Black);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, transformMatrix: camera.Transform);
                foreach (Light light in lightObjects)
                {
                    light.Draw(spriteBatch);
                }

                spriteBatch.End();


                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                spriteBatch.Draw(ambiantLight, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                spriteBatch.End();

                GraphicsDevice.SetRenderTarget(null);
                GraphicsDevice.Clear(Color.Black);
            }

            //DRAW BACKGROUND
            {
                spriteBatch.Begin(
                    sortMode: SpriteSortMode.Deferred,
                    blendState: BlendState.Opaque,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: null,
                    rasterizerState: null,
                    effect: null,
                    transformMatrix: camera.Transform
                );

                GameObjectManager.ZoneMap.Draw(spriteBatch);

                spriteBatch.End();
            }

            //DRAW SHADOWS
            {
                BlendState blendState = new BlendState()
                {
                    ColorSourceBlend = Blend.DestinationColor,
                    ColorDestinationBlend = Blend.SourceColor
                };

                spriteBatch.Begin(
                    sortMode: SpriteSortMode.Immediate,
                    blendState: blendState,
                    depthStencilState: null,
                    rasterizerState: null,
                    effect: null,
                    transformMatrix: null
                );
                spriteBatch.Draw(screenShadows, Vector2.Zero, Color.White);
                spriteBatch.End();
            }

            //DRAW FORGROUND
            {
                spriteBatch.Begin(
                    sortMode: SpriteSortMode.FrontToBack,
                    blendState: BlendState.AlphaBlend,
                    samplerState: SamplerState.PointClamp,
                    depthStencilState: null,
                    rasterizerState: null,
                    effect: null,
                    transformMatrix: camera.Transform
                );

                foreach (Engine.Drawing.IDrawable drawable in GameObjectManager.Drawable)
                    drawable.Draw(spriteBatch);

                spriteBatch.End();
            }

            UserInterface.Active.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void DrawShadowCasters(Light light)
        {
            Matrix transform = Matrix.CreateTranslation(
                -light.Position.X + light.Radius,
                -light.Position.Y + light.Radius,
               0);

            spriteBatch.Begin(
                        sortMode: SpriteSortMode.Deferred,
                        blendState: BlendState.AlphaBlend,
                        samplerState: SamplerState.PointClamp,
                        depthStencilState: DepthStencilState.Default,
                        rasterizerState: RasterizerState.CullNone,
                        effect: null,
                        transformMatrix: transform
                    );

            foreach (Engine.Drawing.IDrawable drawable in GameObjectManager.Drawable)
            {
                drawable.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
        #endregion

        #region Menus
        void PauseGame()
        {
            currentKeyboardState = Keyboard.GetState();

            if (CheckKeyPress(Keys.Escape))
            {
                if (gameState == GameState.playing)
                    gameState = GameState.paused;

                else if (gameState == GameState.paused)
                    gameState = GameState.playing;
            }

            //previousKeyboardState = currentKeyboardState;
        }

        bool CheckKeyPress(Keys key)
        {
            return previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }

        void ShowMainMenu()
        {
            if (gameState == GameState.mainmenu)
            {
                MainMenu.Menu.Visible = true;
                PauseMenu.Menu.Visible = false;
                MainUI.TopPanel.Visible = false;
                MainUI.BottomPanel.Visible = false;
            }

            else
                MainMenu.Menu.Visible = false;
        }

        void ShowPauseMenu()
        {
            if (gameState == GameState.paused)
            {
                MainMenu.Menu.Visible = false;
                PauseMenu.Menu.Visible = true;
                MainUI.TopPanel.Visible = false;
                MainUI.BottomPanel.Visible = false;
            }

            else
                PauseMenu.Menu.Visible = false;
        }

        void ShowMainUI()
        {
            if (gameState == GameState.playing)
            {
                MainMenu.Menu.Visible = false;
                PauseMenu.Menu.Visible = false;
                MainUI.TopPanel.Visible = true;
                MainUI.BottomPanel.Visible = true;
            }

            else
            {
                MainUI.TopPanel.Visible = false;
                MainUI.BottomPanel.Visible = false;
            }
        }

        void ProcessMenuSelection()
        {
            MainMenu.MainToGame.OnClick = (Entity button) => { gameState = GameState.playing; };
            MainMenu.MainToQuit.OnClick = (Entity button) => Exit();

            PauseMenu.PauseToGame.OnClick = (Entity button) => { gameState = GameState.playing; };
            PauseMenu.PauseToMain.OnClick = (Entity button) => { gameState = GameState.mainmenu; };
            PauseMenu.PauseToQuit.OnClick = (Entity button) => Exit();
        }

        void ProcessSpawnables()
        {
            Vector2 position = ZoneManager.CurrentZone.TileMap.Size * ZoneManager.CurrentZone.TileMap.Tiles[0, 0].size - camera.Position;

            switch (MainUI.SpawnMenu.SelectedIndex)
            {
                case 0:

                    GameObjectManager.Add(InteractablesFactory.MakeColonist(position));
                    break;
                case 1:
                    GameObjectManager.Add(InteractablesFactory.MakeRabbit(position));
                    break;
                case 2:
                    GameObjectManager.Add(InteractablesFactory.MakeFarm(position));
                    break;
                case 3:
                    GameObjectManager.Add(InteractablesFactory.MakeTree(position));
                    break;
                case 4:
                    GameObjectManager.Add(InteractablesFactory.MakeBush(position));
                    break;
                case 5:
                    GameObjectManager.Add(InteractablesFactory.MakeWorkBench(position));
                    break;
                case 6:
                    GameObjectManager.Add(InteractablesFactory.MakeStoneNode(position));
                    break;
                case 7:
                    GameObjectManager.Add(InteractablesFactory.MakeTallGrass(position));
                    break;
                case 8:
                    GameObjectManager.Add(InteractablesFactory.MakeRobot(position));
                    break;
                case 9:
                    GameObjectManager.Add(InteractablesFactory.MakeBear(position));
                    break;
                case 10:
                    GameObjectManager.Add(InteractablesFactory.MakeCampfire(position));
                    break;
            }

            MainUI.SpawnMenu.DontKeepSelection = true;
        }

        #endregion
    }
}