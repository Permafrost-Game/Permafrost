using Newtonsoft.Json;
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
                    _fullScreen = bool.Parse(settings[key]);


                key = "resolutionScale";
                if (settings.ContainsKey(key))
                    _resolutionScale = float.Parse(settings[key]);

                key = "devMode";
                if (settings.ContainsKey(key))
                    _devMode = bool.Parse(settings[key]);

            }
            InvokeSettingsChange();
        }

        #region events
        public delegate void SettingsChange();
        public static List<SettingsChange> OnSettingsChange = new List<SettingsChange>();
        private static void InvokeSettingsChange()
        {
            OnSettingsChange.ForEach(a => a.Invoke());
            WriteSettings();
        }
        #endregion

        #region settings properties
        private static float _resolutionScale = 1f;
        public static float ResolutionScale
        {
            get
            {
                return _resolutionScale;
            }
            set
            {
                _resolutionScale = value;
                InvokeSettingsChange();
            }
        }

        private static bool _fullScreen = true;
        public static bool Fullscreen
        {
            get
            {
                return _fullScreen;
            }
            set
            {
                _fullScreen = value;
                InvokeSettingsChange();
            }
        }

        private static bool _devMode = false;
        public static bool DevMode
        {
            get
            {
                return _devMode;
            }
            set
            {
                _devMode = value;
                InvokeSettingsChange();
            }
        }

        private static void WriteSettings()
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
