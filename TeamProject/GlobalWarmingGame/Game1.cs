using Engine;
using Engine.Lighting;
using Engine.TileGrid;
using GeonBit.UI;
using GeonBit.UI.Entities;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Interactions.Interactables.Buildings;
using GlobalWarmingGame.Resources;
using GlobalWarmingGame.UI;
using GlobalWarmingGame.UI.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is the main class for the games implementation.
    /// </summary>
    public class Game1 : Game
    {
        private readonly bool isFullScreen = false;
        private readonly float resolutionScale = 0.75f;




        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TileSet tileSet;
 
        Camera camera;
        KeyboardInputHandler keyboardInputHandler;

        MainMenu MainMenu;
        PauseMenu PauseMenu;

        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        enum GameState { mainmenu, playing, paused }
        GameState gameState;

        List<Light> lightObjects;

        ShadowmapResolver shadowmapResolver;
        QuadRenderComponent quadRender;
        RenderTarget2D screenShadows;
        Texture2D ambiantLight;
        Texture2D logo;
 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = isFullScreen
            };
            
            
            Content.RootDirectory = "Content";
            gameState = GameState.mainmenu;
            SoundFactory.Loadsounds(Content);
            SoundFactory.PlaySong(Songs.Menu);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth  = (int) (GraphicsDevice.DisplayMode.Width * resolutionScale);
            graphics.PreferredBackBufferHeight = (int) (GraphicsDevice.DisplayMode.Height * resolutionScale);
            graphics.ApplyChanges();

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
                //water.Name = "Non-Walkable";

                textureSet.Add(0, this.Content.Load<Texture2D>(@"textures/tiles/old_tileset/error"));
                textureSet.Add(1, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Snow"));
                textureSet.Add(2, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Stone"));
                textureSet.Add(3, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Tundra1"));
                textureSet.Add(4, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Grass"));
                textureSet.Add(5, water);

                InteractablesFactory.LoadContent(Content);

                ResourceTypeFactory.LoadContent(Content);

                tileSet = new TileSet(textureSet, new Vector2(32f));
                GameObjectManager.Init(tileSet);


                //GameObjectManager.CurrentZone = new Zone() { TileMap = GameObjectManager.ZoneMap };
                camera = new Camera(GraphicsDevice.Viewport, GameObjectManager.ZoneMap.Size * GameObjectManager.ZoneMap.Tiles[0, 0].Size);

                GameObjectManager.Camera = camera;
                this.keyboardInputHandler = new KeyboardInputHandler();
            }

            //CREATING GAME OBJECTS
            {
                //All this code below is for testing and will eventually be replaced.
                Controller.LoadContent(Content);

                
               
                logo = Content.Load<Texture2D>(@"logo");

                MainMenu = new MainMenu(logo);
                PauseMenu = new PauseMenu();

                Colonist c1 = (Colonist)InteractablesFactory.MakeInteractable(Interactable.Colonist, position: GameObjectManager.ZoneMap.Size * GameObjectManager.ZoneMap.Tiles[0, 0].Size / 2);

                GameObjectManager.Add(c1);

                
                
                
                ProcessMenuSelection();

                /*
                MainUI = new MainUI();

                ProcessMenuSelection();
                

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

                 MainUI.SpawnMenu.OnValueChange = (Entity e) => { ProcessSpawnables(); };
                
                CollectiveInventory = new CollectiveInventory(MainUI);
                */

            }
            
        }

        protected override void UnloadContent()
        {

        }
        #endregion

        protected override void Update(GameTime gameTime)
        {
            Controller.Update(gameTime);
            ShowMainMenu();
            ShowPauseMenu();
            ShowMainUI();
            PauseGame();

            if (gameState == GameState.playing)
            {
                
                camera.Update(gameTime);
                keyboardInputHandler.Update(gameTime);

                GameObjectManager.ZoneMap.Update(gameTime);
                BuildingManager.UpdateBuildingTemperatures(gameTime, GameObjectManager.ZoneMap);
                UpdateColonistTemperatures(gameTime);

                //TODO the .ToArray() here is so that the foreach itterates over a copy of the list, Not ideal as it adds time complexity
                foreach (IUpdatable updatable in GameObjectManager.Updatables.ToArray())
                    updatable.Update(gameTime);


                UpdateColonistTemperatures(gameTime);

                base.Update(gameTime);
            }

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

                foreach (Engine.Drawing.IDrawable drawable in GameObjectManager.Drawables)
                    drawable.Draw(spriteBatch);

                spriteBatch.End();
            }

            Controller.Draw(spriteBatch);
            

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

            foreach (Engine.Drawing.IDrawable drawable in GameObjectManager.Drawables)
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
                //MainUI.TopPanel.Visible = false;
                //MainUI.BottomPanel.Visible = false;
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
                //MainUI.TopPanel.Visible = false;
                //MainUI.BottomPanel.Visible = false;
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
                //MainUI.TopPanel.Visible = true;
                //MainUI.BottomPanel.Visible = true;
            }

            else
            {
                //MainUI.TopPanel.Visible = false;
                //MainUI.BottomPanel.Visible = false;
            }
        }

        void ProcessMenuSelection()
        {
            MainMenu.MainToGame.OnClick = (Entity button) => { gameState = GameState.playing;  SoundFactory.PlaySong(Songs.Main); };
            MainMenu.MainToQuit.OnClick = (Entity button) => Exit();

            PauseMenu.PauseToGame.OnClick = (Entity button) => { gameState = GameState.playing; };
            PauseMenu.PauseToMain.OnClick = (Entity button) => { gameState = GameState.mainmenu; };
            PauseMenu.PauseToQuit.OnClick = (Entity button) => Exit();
        }

        /*
        void ProcessSpawnables()
        {
            Vector2 position = GameObjectManager.ZoneMap.Size * GameObjectManager.ZoneMap.Tiles[0, 0].size - camera.Position;

            switch (MainUI.SpawnMenu.SelectedIndex)
            {
                case 0:

                    GameObjectManager.Add((GameObject) InteractablesFactory.MakeInteractable(Interactable.Colonist, position));
                    break;
                case 1:
                    GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Rabbit, position));
                    break;
                case 2:
                    GameObjectManager.Add((GameObject) InteractablesFactory.MakeInteractable(Interactable.Farm,position));
                    break;
                case 3:
                    GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Tree, position));
                    break;
                case 4:
                    GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bush, position));
                    break;
                case 5:
                    GameObjectManager.Add((GameObject) InteractablesFactory.MakeInteractable(Interactable.WorkBench,position));
                    break;
                case 6:
                    GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.StoneNode, position));
                    break;
                case 7:
                    GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.TallGrass, position));
                    break;
                case 8:
                    GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Robot, position));
                    break;
                case 9:
                    GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.Bear, position));
                    break;
                case 10:
                    GameObjectManager.Add((GameObject)InteractablesFactory.MakeInteractable(Interactable.CampFire, position));
                    break;
            }

            //MainUI.SpawnMenu.DontKeepSelection = true;
        }*/

        #endregion
    }
}
 