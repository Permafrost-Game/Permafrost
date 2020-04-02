using GlobalWarmingGame.Interactions.Interactables;
using GlobalWarmingGame.UI.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GlobalWarmingGame.UI.Controllers
{
    /// <summary>
    /// This class controls the functionality of the Main Menu and Load Menu UI
    /// </summary>
    static class MainMenuUIController
    { 
        private static MainMenuView<int> view;
        private static Texture2D mainMenuLogo;

        public static readonly string SavesPath = @"Content/saves";

        private static Random random = new Random();

        private static int currentSaveID;

        public static TimeSpan GameTime { get; set; }

        private static double savePlayTime = 0;

        private static bool hasBeenLoaded = false;

        public static void LoadContent(ContentManager content)
        {
            mainMenuLogo = content.Load<Texture2D>(@"logo");
        }

        private static int GetCurrentSaveID(string saveDir)
        {
            return int.Parse(saveDir.Remove(0, saveDir.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1));
        }

        private static string GetSavePath(int saveID)
        {
            return string.Format(@"{0}/{1}/save.json", SavesPath, saveID);
        }

        private static string GenerateSaveData()
        {
            return JsonConvert.SerializeObject(new
            {
                seed = GameObjectManager.seed,
                currentZone = GameObjectManager.ZoneFileName(),
                playTime = savePlayTime + GameTime.TotalMilliseconds
            }, Formatting.Indented);
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


            if (Directory.Exists(SavesPath))
            {
                foreach (string saveDir in Directory.GetDirectories(SavesPath))
                {
                    int saveID = GetCurrentSaveID(saveDir);
                    var save = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(GetSavePath(saveID)));

                    TimeSpan playTime = TimeSpan.FromMilliseconds(Double.Parse(save["playTime"]));
                    AddSave(saveID, playTime, 0);
                }
            }
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

        internal static void ClearUI()
        {
            view?.Clear();
        }

        private static void NewGame()
        {
            //User has selected a New Game be created, You might have to move some code from Game1 that does this, or just keep that code in Game1.

            

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

            currentSaveID = highestSaveID + 1;

            string newSaveDir = string.Format(@"{0}/{1}", SavesPath, currentSaveID);
            Directory.CreateDirectory(newSaveDir);
            Directory.CreateDirectory(string.Format(@"{0}/zones", newSaveDir));

            int seed = random.Next();
            Vector2 currentZone = Vector2.Zero;

            var saveData = GenerateSaveData();

            System.IO.File.WriteAllText(string.Format(@"{0}/save.json", newSaveDir), saveData);

            GameObjectManager.Init(currentSaveID, seed, currentZone, true);
            GlobalCombatDetector.Initalise();

            hasBeenLoaded = true;

            Game1.GameState = GameState.CutScene;
            CutSceneFactory.PlayVideo(VideoN.Intro, () => Game1.GameState = GameState.Playing);
        }

        private static void LoadSaveGame(int saveGame)
        {

            

            currentSaveID = saveGame;

            string savePath = GetSavePath(saveGame);
            var save = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(savePath));

            savePlayTime = double.Parse(save["playTime"]);

            int seed = int.Parse(save["seed"]);

            string[] zoneCoords = save["currentZone"].Split(',');
            Vector2 currentZone = new Vector2(int.Parse(zoneCoords[0]), int.Parse(zoneCoords[1]));

            GameObjectManager.Init(saveGame, seed, currentZone, true);

            hasBeenLoaded = true;

            Game1.GameState = GameState.Playing;
        }

        public static void UnloadSave()
        {
            if (hasBeenLoaded)
            {
                GameObjectManager.SaveZone();

                var saveData = GenerateSaveData();

                System.IO.File.WriteAllText(GetSavePath(currentSaveID), saveData);
            }
        }

        private static void DeleteSaveGame(int saveGame)
        {
            view.RemoveSaveGame(saveGame);
            Directory.Delete(string.Format(@"{0}/{1}", SavesPath, saveGame), true); 
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
