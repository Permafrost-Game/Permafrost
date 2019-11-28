using System;
using System.Collections.Generic;

using Engine;
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
        Texture2D logo;

        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;

        bool isPaused;
        bool isPlaying;

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
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, "hd");

            camera = new Camera(GraphicsDevice.Viewport);
            selectionManager = new SelectionManager();

            base.Initialize();     
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            {
                selectionManager.InputMethods.Add(new MouseInputMethod(camera, selectionManager.CurrentInstruction));

                //TODO this code should be loaded from a file
                var textureSet = new Dictionary<string, Texture2D>();

                Texture2D water = Content.Load<Texture2D>(@"tileset/test_tileset-1/water");
                water.Name = "Non-Walkable";

                textureSet.Add("0", Content.Load<Texture2D>(@"tileset/test_tileset-1/error"));
                textureSet.Add("1", Content.Load<Texture2D>(@"tileset/test_tileset-1/dirt"));
                textureSet.Add("2", Content.Load<Texture2D>(@"tileset/test_tileset-1/grass"));
                textureSet.Add("3", Content.Load<Texture2D>(@"tileset/test_tileset-1/snow"));
                textureSet.Add("4", Content.Load<Texture2D>(@"tileset/test_tileset-1/stone"));
                textureSet.Add("5", water);

                Texture2D colonist = Content.Load<Texture2D>(@"interactables/colonist");
                Texture2D farm = Content.Load<Texture2D>(@"interactables/farm");
                Texture2D bushH = Content.Load<Texture2D>(@"interactables/berrybush-harvestable");
                Texture2D bushN = Content.Load<Texture2D>(@"interactables/berrybush-nonharvestable");
                Texture2D rabbit = Content.Load<Texture2D>(@"interactables/rabbit");

                logo = Content.Load<Texture2D>(@"logo");

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

                MainMenu = new MainMenu(logo, new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                PauseMenu = new PauseMenu();

                UserInterface.Active.AddEntity(MainMenu.Menu);
                UserInterface.Active.AddEntity(PauseMenu.Menu);

                PauseMenu.Menu.Visible = false;
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            UserInterface.Active.Update(gameTime);

            ProcessMenuSelection();

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
                {
                    isPaused = !isPaused;
                    ShowPauseMenu();
                }

                previousKeyboardState = currentKeyboardState;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

           // if (isPlaying)
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

                UserInterface.Active.Draw(spriteBatch);

                base.Draw(gameTime);
            }
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

        void ShowMainMenu()
        {
            isPlaying = false;
            isPaused = false;

            PauseMenu.Menu.Visible = false;
            MainMenu.Menu.Visible = true;
        }

        void ShowPauseMenu()
        {
            if (isPaused)
                PauseMenu.Menu.Visible = true;

            else
                PauseMenu.Menu.Visible = false;
        }

        void ProcessMenuSelection()
        {
            MainMenu.MainToGame.OnClick = (Entity button) => { MainMenu.Menu.Visible = false; isPaused = false; isPlaying = true; };
            MainMenu.MainToQuit.OnClick = (Entity button) => Exit();

            PauseMenu.PauseToGame.OnClick = (Entity button) => { isPaused = false; PauseMenu.Menu.Visible = false; };
            PauseMenu.PauseToMain.OnClick = (Entity button) => { ShowMainMenu(); };
            PauseMenu.PauseToQuit.OnClick = (Entity button) => Exit();
        }
    }
}
