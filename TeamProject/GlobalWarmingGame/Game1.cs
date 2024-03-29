﻿using Engine.Lighting;
using Engine.TileGrid;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Event;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Resources;
using GlobalWarmingGame.UI;
using GlobalWarmingGame.UI.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GlobalWarmingGame
{
    /// <summary>
    /// This class is the main class for the games implementation.
    /// </summary>
    public class Game1 : Game
    {

        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardInputHandler keyboardInputHandler;

        List<Light> lightObjects;
        private bool lighting = false;
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
                        MainMenuUIController.ClearUI();
                        break;
                    case GameState.Paused:
                        GameUIController.ShowPauseMenu(false);
                        goto case GameState.Playing;
                    case GameState.Settings:
                        GameUIController.ShowSettingsMenu(false);
                        goto case GameState.Playing;
                    case GameState.Playing:
                        if (previous != GameState.Playing && previous != GameState.Paused && previous != GameState.Settings)
                        {
                            SoundFactory.StopSong();
                            GameUIController.ClearUI();
                        }
                        break;
                    case GameState.CutScene:
                        CutSceneFactory.StopVideo();
                        break;
                }

                _GameState = value;
                
                switch (value)
                {
                    case GameState.MainMenu:
                        if (previous == GameState.Playing
                            || previous == GameState.Paused
                            || previous == GameState.Settings)
                            MainMenuUIController.UnloadSave();

                        GameUIController.ClearUI();
                        MainMenuUIController.CreateUI();
                        SoundFactory.PlaySong(Songs.Menu);
                        break;
                    case GameState.Paused:
                        GameUIController.ShowPauseMenu(true);
                        goto case GameState.Playing;
                    case GameState.Settings:
                        GameUIController.ShowSettingsMenu(true);
                        goto case GameState.Playing;
                    case GameState.Playing:
                        if (previous != GameState.Playing && previous != GameState.Paused && previous != GameState.Settings)
                        {
                            SoundFactory.PlaySong(Songs.Main);
                            GameUIController.CreateUI();
                        }
                        break;
                }

            }
        }

        System.TimeSpan autoSaveTimer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GameUIController.Initalise(Content);

            SettingsManager.OnSettingsChange.Add(OnSettingsChange);
            SettingsManager.Initalise();
            
            

            //Removes 60 FPS limit
            this.graphics.SynchronizeWithVerticalRetrace = false;
            base.IsFixedTimeStep = false;
            base.Initialize();

        }

        private void OnSettingsChange()
        {
            if (graphics.IsFullScreen != SettingsManager.Fullscreen)
                graphics.ToggleFullScreen();

            graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width * SettingsManager.ResolutionScale);
            graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height * SettingsManager.ResolutionScale);
            graphics.ApplyChanges();
             
            if (GameObjectManager.Camera != null)
            {
                GameObjectManager.Camera.Viewport = GraphicsDevice.Viewport;
            }

            GameUIController.DevMode = SettingsManager.DevMode;
            

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
                var textureSet = new Dictionary<int, Texture2D>
                {
                    { 0, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/error") },
                    { 1, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Tundra1") },
                    { 2, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Grass") },
                    { 3, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Snow") },
                    { 4, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/Stone") },
                    { 5, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/water") },
                    { 6, this.Content.Load<Texture2D>(@"textures/tiles/main_tileset/deepWater") }
                };

                Textures.LoadContent(Content);

                ResourceTypeFactory.Init();

                GameUIController.LoadContent(Content);
                MainMenuUIController.LoadContent(Content);

                GameObjectManager.TileSet = new TileSet(textureSet, new Vector2(32f));
                GameObjectManager.GraphicsDevice = GraphicsDevice;
                GameObjectManager.SpriteBatch = spriteBatch;


                this.keyboardInputHandler = new KeyboardInputHandler(graphics);
            }

            //Menu Setup
            {
                GameState = GameState.MainMenu;
            }

        }

        protected override void UnloadContent()
        {
            MainMenuUIController.UnloadSave();
        }
        #endregion

        protected override void Update(GameTime gameTime)
        {
            GameUIController.Update(gameTime);
            

            keyboardInputHandler.Update(gameTime);

            if (GameState == GameState.Playing)
            {
                MainMenuUIController.GameTime += gameTime.ElapsedGameTime;
                autoSaveTimer += gameTime.ElapsedGameTime;

                //System.Console.WriteLine(autoSaveTimer.TotalMinutes);

                if (autoSaveTimer.TotalMinutes >= 5)
                {
                    autoSaveTimer = new System.TimeSpan();
                    MainMenuUIController.UnloadSave();
                }

                GameObjectManager.Camera.Update(gameTime);

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
            else if (GameState == GameState.CutScene)
            {
                CutSceneFactory.Update(gameTime);
            }
            else if (GameState == GameState.MainMenu)
            {
                MainMenuUIController.Update(gameTime);
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

                            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, transformMatrix: GameObjectManager.Camera.Transform);
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
                            transformMatrix: GameObjectManager.Camera.Transform
                        );

                Vector2 zoneSize = GameObjectManager.ZoneMap.Size * GameObjectManager.ZoneMap.Tiles[0, 0].Size;
                spriteBatch.Draw(GameObjectManager.GreyTiles, new Rectangle((int)-zoneSize.X, (int)-zoneSize.Y, GameObjectManager.GreyTiles.Width, GameObjectManager.GreyTiles.Height), Color.Gray);

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
                            transformMatrix: GameObjectManager.Camera.Transform
                        );

                        foreach (Engine.Drawing.IDrawable drawable in GameObjectManager.Drawables)
                            drawable.Draw(spriteBatch);


                        spriteBatch.End();

                    }

                    GameUIController.Draw(spriteBatch);

                    break;
                case GameState.CutScene:
                    spriteBatch.Begin();
                    CutSceneFactory.Draw(spriteBatch, GraphicsDevice);
                    spriteBatch.End();

                    break;
                case GameState.MainMenu:
                    MainMenuUIController.Draw(spriteBatch);
                    break;
                default:
                    GraphicsDevice.Clear(Color.Black);

                    break;
            }


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

    public enum GameState { MainMenu, Playing, Paused, Settings, Exiting, CutScene }
}
