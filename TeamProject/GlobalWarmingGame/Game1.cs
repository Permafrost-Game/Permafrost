using System;
using System.Collections.Generic;

using Engine;
using Engine.Lighting;
using Engine.TileGrid;

using GlobalWarmingGame.Action;
using GlobalWarmingGame.Resources;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Menus;
using GlobalWarmingGame.Interactions.Interactables.Buildings;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is the main class for the games implemntation.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SelectionManager selectionManager;

        TileSet tileSet;
        TileMap tileMap;
        
        Camera camera;

        MainMenu MainMenu;
        PauseMenu PauseMenu;
        MainUI MainUI;

        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        enum GameState { mainmenu, playing, paused }
        GameState gameState;

        List<Light> lightObjects;

        ShadowmapResolver shadowmapResolver;
        QuadRenderComponent quadRender;
        RenderTarget2D screenShadows;
        Texture2D ambiantLight;

        Texture2D farm;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1024,
                PreferredBackBufferHeight = 768
            };
            
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            gameState = GameState.mainmenu;
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, "hd");

            selectionManager = new SelectionManager();

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
                    new Light(new Vector2(256,224), GraphicsDevice, 256f, new Color(255,0  ,0  ,255), "Light" ),
                    new Light(new Vector2(224,512), GraphicsDevice, 256f, new Color(0,0  ,255  ,255), "Light" )
                };

                screenShadows = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }


            //LOADING TILEMAP AND ZONES
            {
                //TODO textures should be loaded from a file
                var textureSet = new Dictionary<string, Texture2D>();

                Texture2D water = this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/water");
                water.Name = "Non-Walkable";

                textureSet.Add("1", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/error"));
                textureSet.Add("2", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/dirt"));
                textureSet.Add("3", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/grass"));
                textureSet.Add("4", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/snow"));
                textureSet.Add("5", this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/stone"));
                textureSet.Add("6", water);

                

                tileSet = new TileSet(textureSet, new Vector2(16));
                //                                                  map0/00.csv  //50x50 tilemap
                //                                                  map1/00.csv  //100x100 tilemap
                tileMap = TileMapParser.parseTileMap(@"Content/maps/map1/00.csv", tileSet);

                ZoneManager.CurrentZone = new Zone() { TileMap = tileMap };
                camera = new Camera(GraphicsDevice.Viewport, tileMap.Size * 16f);
            }

            //CREATING GAME OBJECTS
            {
                //All this code below is for testing and will eventually be replaced.

                Texture2D colonist = this.Content.Load<Texture2D>(@"textures/interactables/animals/colonist/sprite0");
                farm = this.Content.Load<Texture2D>(@"textures/interactables/buildings/farm/sprite0");
                Texture2D bushH = this.Content.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite0");
                Texture2D bushN = this.Content.Load<Texture2D>(@"textures/interactables/environment/berry_bush/sprite1");
                Texture2D rabbit = this.Content.Load<Texture2D>(@"textures/interactables/animals/rabbit/sprite0");
                Texture2D tree = this.Content.Load<Texture2D>(@"textures/interactables/environment/tree/sprite0");
                Texture2D treeStump = this.Content.Load<Texture2D>(@"textures/interactables/environment/tree/sprite2");
                Texture2D logo = Content.Load<Texture2D>(@"logo");

                Texture2D[] textureArray = new Texture2D[] { farm };
                string[] stringArray = new string[] { "Farm" };

                BuildingManager.AddBuilding(0, "No Building");
                for (int i = 0; i < stringArray.Length; i++) 
                   BuildingManager.AddBuilding(i+1, stringArray[i], textureArray[i]);

                MainMenu = new MainMenu(logo);
                PauseMenu = new PauseMenu();
                MainUI = new MainUI();

                selectionManager.InputMethods.Add(new MouseInputMethod(camera, tileMap, selectionManager.CurrentInstruction, MainUI));

                ProcessMenuSelection();

                var c1 = new Colonist(position: new Vector2(480, 200), texture: colonist, inventoryCapacity: 100f);
                selectionManager.CurrentInstruction.ActiveMember = c1;
                GameObjectManager.Add(c1);

                string[] spawnables = new string[6];
                spawnables[0] = "Colonist";
                spawnables[1] = "Colonist";
                spawnables[2] = "Rabbit";
                spawnables[3] = "Farm";
                spawnables[4] = "Tree";
                spawnables[5] = "Bush";

                for (int i = 0; i < spawnables.Length; i++)
                    MainUI.SpawnMenu.AddItem(spawnables[i]);

                MainUI.SpawnMenu.OnValueChange = (Entity e) =>
                {
                    switch (MainUI.SpawnMenu.SelectedIndex)
                    {
                        case 0:
                            GameObjectManager.Add(new Colonist(position: new Vector2(256, 512), texture: colonist, inventoryCapacity: 100f));
                            break;
                        case 1:
                            GameObjectManager.Add(new Colonist(position: new Vector2(450, 450), texture: colonist, inventoryCapacity: 100f));
                            break;
                        case 2:
                            GameObjectManager.Add(new Rabbit(position: new Vector2(575, 575), texture: rabbit));
                            break;
                        case 3:
                            GameObjectManager.Add(new Farm(position: new Vector2(256, 256), texture: farm));
                            break;
                        case 4:
                            GameObjectManager.Add(new Tree(position: new Vector2(312, 612), textureTree: tree, textureStump: treeStump));
                            break;
                        case 5:
                            GameObjectManager.Add(new Bush(position: new Vector2(312, 512), harvestable: bushH, harvested: bushN));
                            break;
                    }
                };
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

                tileMap.Update(gameTime);

                foreach (IUpdatable updatable in GameObjectManager.Updatable)
                    updatable.Update(gameTime);

                foreach (MouseInputMethod mouseInputMethod in selectionManager.InputMethods)
                    mouseInputMethod.Update(gameTime);

                MainUI.Update(gameTime);

                UpdateColonistTemperatures(gameTime);

                CollectiveInventory.UpdateCollectiveInventory();

                //Uncomment this line for a light around the cursor (uses the first item in lightObjects)
                //lightObjects[0].Position = Vector2.Transform(Mouse.GetState().Position.ToVector2(), camera.InverseTransform);

                base.Update(gameTime);
            }
        }

        #region Update Colonists Temperatures
        void UpdateColonistTemperatures(GameTime gameTime)
        {
            //Adjust the temperatures of the colonists
            foreach (Colonist colonist in GameObjectManager.GetObjectsByTag("Colonist"))
            {
                float tileTemp = tileMap.GetTileAtPosition(colonist.Position).temperature.Value;

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
                spriteBatch.Draw(ambiantLight, new Rectangle(0,0,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
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

                tileMap.Draw(spriteBatch);

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

                foreach (Engine.IDrawable drawable in GameObjectManager.Drawable)
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

            foreach (Engine.IDrawable drawable in GameObjectManager.Drawable)
            {
                drawable.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
        #endregion

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

            previousKeyboardState = currentKeyboardState;
        }

        bool CheckKeyPress(Keys key)
        {
            if (previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key))
                return true;

            return false;
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
    }
}
