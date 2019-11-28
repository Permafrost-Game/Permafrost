using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;

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

        private Desktop _desktop;

        Camera camera;
        Effect effect;
        MainMenu mainMenu;
        PauseMenu pauseMenu;

        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        bool isPaused;
        bool isPlaying;


        //testcode 
        Vector2 lightPosition = new Vector2(256, 256);
        Vector2 lightPosition2 = new Vector2(512, 512);
        LightArea lightArea1;
        LightArea lightArea2;
        ShadowmapResolver shadowmapResolver;
        QuadRenderComponent quadRender;
        RenderTarget2D screenShadows;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1024,  // set this value to the desired width of your window
                PreferredBackBufferHeight = 768   // set this value to the desired height of your window
            };
            
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            camera = new Camera(GraphicsDevice.Viewport);
            selectionManager = new SelectionManager();

            this.IsMouseVisible = true;
            base.Initialize();     
        }

        protected override void LoadContent()
        {
            { //TEST CODE
                quadRender = new QuadRenderComponent(this);

                shadowmapResolver = new ShadowmapResolver(GraphicsDevice, quadRender, ShadowmapSize.Size256, ShadowmapSize.Size1024);
                shadowmapResolver.LoadContent(Content);
                
                lightArea1 = new LightArea(GraphicsDevice, ShadowmapSize.Size512);
                lightArea2 = new LightArea(GraphicsDevice, ShadowmapSize.Size512);
                screenShadows = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }


            spriteBatch = new SpriteBatch(GraphicsDevice);
            {
                _desktop = new Desktop();
                MyraEnvironment.Game = this;
                selectionManager.InputMethods.Add(new MouseInputMethod(camera, _desktop, selectionManager.CurrentInstruction));

                effect = Content.Load<Effect>(@"shaders/Lights");
                //effect = null; //Line is for testing reasons
                var t = effect.Parameters[0];

                mainMenu = new MainMenu();
                pauseMenu = new PauseMenu();

                //TODO this code should be loaded from a file
                var textureSet = new Dictionary<string, Texture2D>();

                Texture2D water = this.Content.Load<Texture2D>(@"tileset/test_tileset-1/water");
                water.Name = "Non-Walkable";

                textureSet.Add("0", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/error"));
                textureSet.Add("1", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/dirt"));
                textureSet.Add("2", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/grass"));
                textureSet.Add("3", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/snow"));
                textureSet.Add("4", this.Content.Load<Texture2D>(@"tileset/test_tileset-1/stone"));
                textureSet.Add("5", water);


                Texture2D colonist = this.Content.Load<Texture2D>(@"interactables/colonist");
                Texture2D farm = this.Content.Load<Texture2D>(@"interactables/farm");
                Texture2D bushH = this.Content.Load<Texture2D>(@"interactables/berrybush-harvestable");
                Texture2D bushN = this.Content.Load<Texture2D>(@"interactables/berrybush-nonharvestable");
                Texture2D rabbit = this.Content.Load<Texture2D>(@"interactables/rabbit");

                tileSet = new TileSet(textureSet, new Vector2(16));
                tileMap = TileMapParser.parseTileMap(@"Content/testmap.csv", tileSet);

                ZoneManager.CurrentZone = new Zone() { TileMap = tileMap };


                //ALL the Below code is testing
                
                var c1 = new Colonist(
                    position:   new Vector2(25, 25),
                    texture: colonist,
                    inventoryCapacity: 100f);
                    
                selectionManager.CurrentInstruction.ActiveMember = (c1);
                
                GameObjectManager.Add(c1);
                
                GameObjectManager.Add(new Colonist(
                    position: new Vector2(75, 75),
                    texture: colonist,
                    inventoryCapacity: 100f));

                GameObjectManager.Add(new Colonist(
                    position: new Vector2(450, 450),
                    texture: colonist,
                    inventoryCapacity: 100f));

                GameObjectManager.Add(new Farm(
                    position: new Vector2(128, 128),
                    texture: farm));
                    
                GameObjectManager.Add(new Bush(
                    position: new Vector2(256, 256),
                    harvestable: bushH,
                    harvested: bushN));
                    
                GameObjectManager.Add(new Rabbit(
                    position: new Vector2(575, 575),
                    texture: rabbit));
                    
                //GameObjectManager.Add( new InteractableGameObject(
                //    position: new Vector2(256, 256),
                //     texture: bush,
                //     new List<InstructionType>() { new InstructionType("pick", "Pick Berries", "Pick Berries from the bush", 1) }
                //     );
                //GameObjectManager.Add( new PassiveMovingGameObject(
                //     position: new Vector2(575, 575),
                //     texture: rabbit,
                //     new List<InstructionType>() { new InstructionType("hunt", "Hunt Rabbit", "Pick Flesh from rabbit", 1) }
                //     );
                
                GameObjectManager.Add(new DisplayLabel(0, "Food", _desktop, "lblFood"));
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            ShowMainMenu();
            ProcessMenuSelection();
            SuspendContextMenuClick();

            if (!isPaused && isPlaying)
            {
                camera.UpdateCamera();
                
                //TileMap.update is used to update the temperature of the tiles
                tileMap.Update(gameTime);
                
                foreach (IUpdatable updatable in GameObjectManager.Updatable)
                    updatable.Update(gameTime);

                CollectiveInventory.UpdateCollectiveInventory();

                base.Update(gameTime);
            }
            
            if (isPlaying)
            {
                currentKeyboardState = Keyboard.GetState();

                if (CheckKeypress(Keys.Escape))
                    ShowPauseMenu();

                previousKeyboardState = currentKeyboardState;
            }
        }

        private void DrawCasters()
        {
            spriteBatch.Begin(
                        sortMode: SpriteSortMode.Deferred,
                        blendState: BlendState.AlphaBlend,
                        samplerState: SamplerState.PointClamp,
                        depthStencilState: DepthStencilState.Default,
                        rasterizerState: RasterizerState.CullNone,
                        effect: null,
                        transformMatrix: camera.Transform
                    );

            foreach (Engine.IDrawable drawable in GameObjectManager.Drawable)
                drawable.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void DrawScene()
        {
            spriteBatch.Begin(
                        sortMode: SpriteSortMode.Deferred,
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
        private void DrawGround()
        {
            //draw the tile texture tiles across the screen
            Rectangle source = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Begin(
                        sortMode: SpriteSortMode.Deferred,
                        blendState: BlendState.Opaque,
                        samplerState: SamplerState.PointClamp,
                        depthStencilState: DepthStencilState.Default,
                        rasterizerState: RasterizerState.CullNone,
                        effect: null,
                        transformMatrix: camera.Transform
                    );

            tileMap.Draw(spriteBatch);

            

            spriteBatch.End();

        }

        private void _Draw(GameTime gameTime)
        {

            //first light area
            lightArea1.LightPosition = lightPosition;
            lightArea1.BeginDrawingShadowCasters();
            DrawCasters();
            lightArea1.EndDrawingShadowCasters();
            shadowmapResolver.ResolveShadows(lightArea1.RenderTarget, lightArea1.RenderTarget, Vector2.Transform(lightPosition, camera.Transform));

            //second light area
            lightArea2.LightPosition = lightPosition2;
            lightArea2.BeginDrawingShadowCasters();
            DrawCasters();
            lightArea2.EndDrawingShadowCasters();
            shadowmapResolver.ResolveShadows(lightArea2.RenderTarget, lightArea2.RenderTarget, Vector2.Transform(lightPosition2, camera.Transform));


            GraphicsDevice.SetRenderTarget(screenShadows);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            spriteBatch.Draw(lightArea1.RenderTarget, lightArea1.LightPosition - (lightArea1.LightAreaSize * 0.5f), Color.Red);
            spriteBatch.Draw(lightArea2.RenderTarget, lightArea2.LightPosition - (lightArea2.LightAreaSize * 0.5f), Color.Blue);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);


            GraphicsDevice.Clear(Color.Black);

            DrawGround();

            BlendState blendState = new BlendState();
            blendState.ColorSourceBlend = Blend.DestinationColor;
            blendState.ColorDestinationBlend = Blend.SourceColor;

            spriteBatch.Begin(SpriteSortMode.Immediate, blendState);
            spriteBatch.Draw(screenShadows, Vector2.Zero, Color.White);
            spriteBatch.End();

            DrawScene();

            base.Draw(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            

            GraphicsDevice.Clear(Color.Black);

            if (isPlaying)
            {
                //TEST CODE!!!
                _Draw(gameTime);
                if (false)
                {

                    spriteBatch.Begin(
                        sortMode: SpriteSortMode.FrontToBack,
                        blendState: BlendState.AlphaBlend,
                        samplerState: SamplerState.PointClamp,
                        depthStencilState: null,
                        rasterizerState: null,
                        effect: effect,
                        transformMatrix: camera.Transform
                    );

                    tileMap.Draw(spriteBatch);

                    foreach (Engine.IDrawable drawable in GameObjectManager.Drawable)
                        drawable.Draw(spriteBatch);

                    spriteBatch.End();

                    base.Draw(gameTime);
                }
            }

            _desktop.Render();
        }

        void ShowMainMenu()
        {
            if (!isPlaying)
            {
                Point position = new Vector2(graphics.PreferredBackBufferWidth / 2 - 75f, graphics.PreferredBackBufferHeight / 2 - 50f).ToPoint();
                mainMenu.DrawMainMenu(_desktop, position);
            }
        }

        void ShowPauseMenu()
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Point position = new Vector2(graphics.PreferredBackBufferWidth / 2 - 75f, graphics.PreferredBackBufferHeight / 2 - 50f).ToPoint();
                pauseMenu.DrawPauseMenu(_desktop, position);
            }

            else
                _desktop.HideContextMenu();
        }

        /// <summary>
        /// Checks if the currently released key had been pressed the previous frame
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool CheckKeypress(Keys key)
        {
            if (previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key))
                return true;

            return false;
        }

        /// <summary>
        /// Suspends mouse input during Main and Pause Menus, otherwise resumes it.
        /// </summary>
        void SuspendContextMenuClick()
        {
            _desktop.ContextMenuClosing += (s, a) =>
            {
                if (!_desktop.ContextMenu.Bounds.Contains(_desktop.TouchPosition))
                {
                    if (!isPaused || isPlaying)
                        a.Cancel = false;
                    else
                        a.Cancel = true;
                }
            };
        }

        /// <summary>
        /// Executes selected commands on the Main and Pause Menus
        /// </summary>
        void ProcessMenuSelection()
        {
            mainMenu.MainToGame.Selected += (s, a) => { isPaused = false; isPlaying = true; };
            mainMenu.MainToQuit.Selected += (s, a) => Exit();

            pauseMenu.PauseToGame.Selected += (s, a) => { isPaused = false; };
            pauseMenu.PauseToMain.Selected += (s, a) => { isPlaying = false; ShowMainMenu(); };
            pauseMenu.PauseToQuit.Selected += (s, a) => Exit();
        }
    }
}