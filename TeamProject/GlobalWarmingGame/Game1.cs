using Engine;
using Engine.Lighting;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Event;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Resources;
using GlobalWarmingGame.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is the main class for the games implementation.
    /// </summary>
    public class Game1 : Game
    {
        const string SettingsPath = @"Content/settings.json";

        private float resolutionScale = 0.75f;
        private int seed = new System.Random().Next();
        private Vector2 currentZone = Vector2.Zero;


        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TileSet tileSet;

        Camera camera;
        KeyboardInputHandler keyboardInputHandler;

        List<Light> lightObjects;
        private bool lighting = true;
        ShadowmapResolver shadowmapResolver;
        QuadRenderComponent quadRender;
        RenderTarget2D screenShadows;
        Texture2D ambiantLight;

        private static GameState _GameState;
        public static GameState GameState
        {
            get { return _GameState; }
            set
            {
                GameState previous = _GameState;

                switch (previous)
                {
                    case GameState.MainMenu:
                        SoundFactory.StopSong();
                        Controller.ResetUI();
                        break;
                    case GameState.Paused:
                        Controller.ShowPauseMenu(false);
                        goto case GameState.Playing;
                    case GameState.Settings:
                        Controller.ShowSettingsMenu(false);
                        goto case GameState.Playing;
                    case GameState.Playing:
                        if (previous != GameState.Playing && previous != GameState.Paused && previous != GameState.Settings)
                        {
                            SoundFactory.StopSong();
                            Controller.ResetUI();
                        }
                        break;
                    case GameState.Intro:
                        CutSceneFactory.StopVideo();
                        break;
                }

                _GameState = value;

                switch (value)
                {
                    case GameState.MainMenu:
                        Controller.ResetUI();
                        Controller.CreateMainMenu();
                        SoundFactory.PlaySong(Songs.Menu);
                        break;
                    case GameState.Paused:
                        Controller.ShowPauseMenu(true);
                        goto case GameState.Playing;
                    case GameState.Settings:
                        Controller.ShowSettingsMenu(true);
                        goto case GameState.Playing;
                    case GameState.Playing:
                        if (previous != GameState.Playing && previous != GameState.Paused && previous != GameState.Settings)
                        {
                            SoundFactory.PlaySong(Songs.Main);
                            Controller.CreateGameUI();
                        }
                        break;
                    case GameState.Intro:
                        CutSceneFactory.PlayVideo(VideoN.Intro);
                        break;
                }

            }
        }

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            if (File.Exists(SettingsPath))
            {
                var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(SettingsPath));

                if (bool.Parse(settings["isFullScreen"]))
                    graphics.ToggleFullScreen();

                resolutionScale = float.Parse(settings["resolutionScale"]);

                seed = int.Parse(settings["seed"]);

                string[] zoneCoords = settings["currentZone"].Split(',');
                currentZone = new Vector2(int.Parse(zoneCoords[0]), int.Parse(zoneCoords[1]));
            }

            graphics.PreferredBackBufferWidth  = (int) (GraphicsDevice.DisplayMode.Width * resolutionScale);
            graphics.PreferredBackBufferHeight = (int) (GraphicsDevice.DisplayMode.Height * resolutionScale);
            graphics.ApplyChanges();

            Controller.Initalise(Content);

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

            CutSceneFactory.LoadContent(Content);
            SoundFactory.Loadsounds(Content);

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

                textureSet.Add(0, this.Content.Load<Texture2D>(@"textures/tiles/old_tileset/error"));
                textureSet.Add(1, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Snow"));
                textureSet.Add(2, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Stone"));
                textureSet.Add(3, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Tundra1"));
                textureSet.Add(4, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Grass"));
                textureSet.Add(5, water);

                Textures.LoadContent(Content);

                InteractablesFactory.LoadContent(Content);

                ResourceTypeFactory.Init();

                Controller.LoadContent(Content);

                tileSet = new TileSet(textureSet, new Vector2(32f));

                GameObjectManager.Init(tileSet, seed, currentZone, false);


                //GameObjectManager.CurrentZone = new Zone() { TileMap = GameObjectManager.ZoneMap };
                camera = new Camera(GraphicsDevice.Viewport, GameObjectManager.ZoneMap.Size * GameObjectManager.ZoneMap.Tiles[0, 0].Size);

                GameObjectManager.Camera = camera;
                this.keyboardInputHandler = new KeyboardInputHandler(graphics);
            }

            //Menu Setup
            {
                GameState = GameState.MainMenu;
            }

        }

        protected override void UnloadContent()
        {
            GameObjectManager.SaveZone();

            var settingsData = JsonConvert.SerializeObject(new
            {
                isFullScreen = graphics.IsFullScreen,
                resolutionScale,
                seed,
                currentZone = GameObjectManager.ZoneFileName()
            }, Formatting.Indented);

            System.IO.File.WriteAllText(SettingsPath, settingsData);
        }
        #endregion

        protected override void Update(GameTime gameTime)
        {
            Controller.Update(gameTime);
            GlobalCombatDetector.UpdateParticipants();

            keyboardInputHandler.Update(gameTime);

            if (GameState == GameState.Playing)
            {

                camera.Update(gameTime);

                TemperatureManager.UpdateTemperature(gameTime);
                EventManager.UpdateEventTime(gameTime);

                //TODO the .ToArray() here is so that the foreach itterates over a copy of the list, Not ideal as it adds time complexity
                foreach (Engine.IUpdatable updatable in GameObjectManager.Updatables.ToArray())
                    updatable.Update(gameTime);

                base.Update(gameTime);
            }
            else if (GameState == GameState.Exiting)
            {
                Exit();
            }
        }


        #region Drawing and Lighting
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (GameState)
            {
                case GameState.Paused:
                case GameState.Settings:
                case GameState.Playing:
                    //CALCULATE SHADOWS
                    if (lighting)
                    {
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
                    }

                    //DRAW BACKGROUND
                    {
                        spriteBatch.Begin(
                            sortMode: SpriteSortMode.Deferred,
                            samplerState: SamplerState.PointClamp,
                            transformMatrix: camera.Transform
                        );

                        GameObjectManager.ZoneMap.Draw(spriteBatch);

                        spriteBatch.End();
                    }

                    //DRAW SHADOWS
                    if(lighting) {
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
                            transformMatrix: camera.Transform
                        );

                        foreach (Engine.Drawing.IDrawable drawable in GameObjectManager.Drawables)
                            drawable.Draw(spriteBatch);


                        spriteBatch.End();

                    }
                   
                    break;
                case GameState.Intro:
                    spriteBatch.Begin();
                    CutSceneFactory.Draw(spriteBatch, GraphicsDevice);
                    spriteBatch.End();

                    break;
                default:
                    GraphicsDevice.Clear(Color.Black);

                    break;
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

        #endregion
    }

    public enum GameState { MainMenu, Playing, Paused, Settings, Intro, Exiting }
}
