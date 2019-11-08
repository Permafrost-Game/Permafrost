using Engine;
using Engine.TileGrid;
using GlobalWarmingGame.Action;
using GlobalWarmingGame.Interactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,  // set this value to the desired width of your window
                PreferredBackBufferHeight = 720   // set this value to the desired height of your window
            };
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

                Texture2D colonist = this.Content.Load<Texture2D>(@"colonist");
                Texture2D farm = this.Content.Load<Texture2D>(@"farm");
                Texture2D bush = this.Content.Load<Texture2D>(@"berrybush");
                Texture2D rabbit = this.Content.Load<Texture2D>(@"rabbit");


                tileSet = new TileSet(textureSet, new Vector2(16));
                tileMap = TileMapParser.parseTileMap(@"Content/testmap.csv", tileSet);


                ZoneManager.CurrentZone = new Zone() { TileMap = tileMap };

                //ALL the Below code is testing
                var f1 = new InteractableGameObject(
                    position: new Vector2(128, 128),
                    texture: farm,
                    new List<InstructionType>() { new InstructionType("harvest", "Harvest", "Harvests food from the farm", 1) }
                    );

                var c1 = new Colonist(
                    position:   new Vector2(0,0),
                    texture: colonist);

                var c2 = new Colonist(
                    position: new Vector2(0,0),
                    texture: colonist);

                var c3 = new Colonist(
                    position: new Vector2(75,50),
                    texture: colonist);

                var b1 = new InteractableGameObject(
                     position: new Vector2(256, 256),
                     texture: bush,
                     new List<InstructionType>() { new InstructionType("pick", "Pick Berries", "Pick Berries from the bush", 1) }
                     );
                var p1 = new PassiveMovingGameObject(
                     position: new Vector2(500, 500),
                     texture: rabbit,
                     new List<InstructionType>() { new InstructionType("hunt", "Hunt Rabbit", "Pick Flesh from rabbit", 1) }
                     );

                GameObjectManager.Add(c1);
                //GameObjectManager.Add(c2);
                GameObjectManager.Add(c3);
                GameObjectManager.Add(f1);
                GameObjectManager.Add(b1);
                GameObjectManager.Add(p1);
                selectionManager.CurrentInstruction.ActiveMember = (c1);

                GameObjectManager.Add(new DisplayLabel(0, "Food", _desktop, "lblFood"));

                
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            camera.UpdateCamera();

            foreach (IUpdatable updatable in GameObjectManager.Updatable)
                updatable.Update(gameTime);
             
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

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

            _desktop.Render();

            base.Draw(gameTime);
        }
    }
}