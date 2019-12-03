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
        MainMenu mainMenu;
        PauseMenu pauseMenu;

        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        bool isPaused;
        bool isPlaying;
        
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            {
                _desktop = new Desktop();
                MyraEnvironment.Game = this;
                selectionManager.InputMethods.Add(new MouseInputMethod(camera, _desktop, selectionManager.CurrentInstruction));

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
                    position: new Vector2(25, 25),
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

                //Adjust the temperatures of the colonists
                foreach (Colonist colonist in GameObjectManager.GetObjectsByTag("Colonist"))
                {
                    float tileTemp = tileMap.GetTileAtPosition(colonist.Position).temperature.Value;
                    float colonistTemp = colonist.Temperature.Value;
                    
                    if (colonistTemp > 50 || colonistTemp < -5)
                    {
                        colonist.Temperature.Value = MathHelper.Clamp(colonistTemp,-5,50);
                        continue;
                    }

                    colonist.UpdateTemp(tileTemp,gameTime);
                    Console.Out.WriteLine(colonist.Temperature.Value + " " + colonist.Health);
                }

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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (isPlaying)
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

                tileMap.Draw(spriteBatch);

                foreach (Engine.IDrawable drawable in GameObjectManager.Drawable)
                    drawable.Draw(spriteBatch);

                spriteBatch.End();

                base.Draw(gameTime);
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