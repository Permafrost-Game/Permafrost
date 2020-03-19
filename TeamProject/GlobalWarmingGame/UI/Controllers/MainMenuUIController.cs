using GlobalWarmingGame.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.IO;

namespace GlobalWarmingGame.UI.Controllers
{
    /// <summary>
    /// This class controls the functionality of the Main Menu and Load Menu UI
    /// </summary>
    static class MainMenuUIController
    { 
        //Change genric parameter "string" to what ever you want to use to identify different save files.
        //Could be an enum, could be a class, could be a folder directory . What ever you need to load a specific save.
        //You will also need to change "string" in other places in this class only.
        private static MainMenuView<int> view;
        private static Texture2D mainMenuLogo;

        public static readonly string SavesPath = @"Content/saves";

        private static Random random = new Random();

        public static void LoadContent(ContentManager content)
        {
            mainMenuLogo = content.Load<Texture2D>(@"logo");
        }

        private static int GetCurrentSaveID(string saveDir)
        {
            return int.Parse(saveDir.Remove(0, saveDir.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1));
        }

        //private static string GetSaveFilePath(string saveDir, int saveID)
        //{
        //    return string.Format(@"{0}/{1}/save.json", saveDir, saveID);
        //}

        public static void CreateUI(float uiScale = 1f)
        {
            view = new MainMenuView<int>();
            view.SetUIScale(uiScale);
            view.CreateUI(mainMenuLogo, NewGame);
            view.SetMainMenuVisiblity(true);

            //Add your saves from a file here
            //If playtime or numberOfTowersCaptured aren't stored, we can either remove them, or just set them to default values for now and we will fix that in the future.

            //int save = 1;
            //AddSave(save, new TimeSpan(1, 20, 0), 1);

            foreach (string saveDir in Directory.GetDirectories(SavesPath))
                AddSave(GetCurrentSaveID(saveDir), new TimeSpan(1, 20, 0), 1);

        }

        /// <summary>
        /// Adds the given SaveGame as an option in the UI
        /// </summary>
        /// <param name="save">The tag for the save</param>
        /// <param name="playTime">The play time of the save</param>
        /// <param name="numberOfTowersCaptured">The number of towers captured in this given save</param>
        private static void AddSave(int save, TimeSpan playTime, int numberOfTowersCaptured)
        {
            string text = $"{save.ToString()} - Play Time: {playTime.Hours}h {playTime.Minutes % 60}m - Towers Captured: {numberOfTowersCaptured}";
            view.AddLoadSaveGame(
                text: text,
                onLoad: new ButtonHandler<int>(
                    tag: save,
                    LoadSaveGame),
                onDelete: new ButtonHandler<int>(
                    tag: save,
                    DeleteSaveGame)
                );
        }

        private static void NewGame()
        {
            //User has selected a New Game be created, You might have to move some code from Game1 that does this, or just keep that code in Game1.

            Game1.GameState = GameState.Intro;

        if (!Directory.Exists(SavesPath))
            Directory.CreateDirectory(SavesPath);

            string[] saveDirectories = Directory.GetDirectories(SavesPath);

            int highestSaveID = 0;

            foreach (string saveDir in saveDirectories)
            {
                int currentSaveID = GetCurrentSaveID(saveDir);

                if (currentSaveID > highestSaveID)
                    highestSaveID = currentSaveID;
            }

            int newSaveID = highestSaveID + 1;

            string newSaveDir = string.Format(@"{0}/{1}", SavesPath, newSaveID);
            Directory.CreateDirectory(newSaveDir);

            int seed = random.Next();
            Vector2 currentZone = Vector2.Zero;

            var saveData = JsonConvert.SerializeObject(new
            {
                seed,
                currentZone,
            }, Formatting.Indented);

            System.IO.File.WriteAllText(string.Format(@"{0}/save.json", newSaveDir), saveData);

            GameObjectManager.Init(seed, currentZone, false);
        }

        private static void LoadSaveGame(int saveGame)
        {
            //Insert code to load a save file. Setup GameObject manager etc..

            Game1.GameState = GameState.Playing;

            GameObjectManager.Init(1, Vector2.Zero, false);
        }


        private static void DeleteSaveGame(int saveGame)
        {
            //Insert code to delete the save files, Perhaps just rename the files to an invalid name eg prefix with an underscore so that the user can recover deleted files, Up to you.


            view.RemoveSaveGame(saveGame);
        }


        public static void Update(GameTime gameTime)
        {
            view.Update(gameTime);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            view.Draw(spriteBatch);
        }
    }
}
