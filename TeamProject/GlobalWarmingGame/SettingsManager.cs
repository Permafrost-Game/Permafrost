using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace GlobalWarmingGame
{

    public static class SettingsManager
    {
        private const string SETTINGS_PATH = @"Content/settings.json";

        public static void Initalise()
        {
            if (File.Exists(SETTINGS_PATH))
            {
                var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(SETTINGS_PATH));

                string key = "isFullScreen";
                if (settings.ContainsKey(key))
                    fullScreen = bool.Parse(settings[key]);


                key = "resolutionScale";
                if (settings.ContainsKey(key))
                    resolutionScale = float.Parse(settings[key]);

                key = "devMode";
                if (settings.ContainsKey(key))
                    devMode = bool.Parse(settings[key]);

            }
            InvokeSettingsChange();
        }

        #region events
        public delegate void SettingsChange();
        public static List<SettingsChange> OnSettingsChange = new List<SettingsChange>();
        private static void InvokeSettingsChange() => OnSettingsChange.ForEach(a => a.Invoke());
        #endregion

        #region settings properties
        private static float resolutionScale = 1f;
        public static float ResolutionScale
        {
            get
            {
                return resolutionScale;
            }
            set
            {
                resolutionScale = value;
                InvokeSettingsChange();
            }
        }

        private static bool fullScreen = true;
        public static bool Fullscreen
        {
            get
            {
                return fullScreen;
            }
            set
            {
                fullScreen = value;
                InvokeSettingsChange();
            }
        }

        private static bool devMode = false;
        public static bool DevMode
        {
            get
            {
                return devMode;
            }
            set
            {
                devMode = value;
                InvokeSettingsChange();
            }
        }

        public static void WriteSettings()
        {
            var settingsData = JsonConvert.SerializeObject(new
            {
                isFullScreen = Fullscreen,
                resolutionScale = ResolutionScale,
                devMode = DevMode,

            }, Formatting.Indented);

            System.IO.File.WriteAllText(SETTINGS_PATH, settingsData);
        }


        #endregion

    }
}
