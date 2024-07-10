using System.IO;
using Newtonsoft.Json;

namespace Caspian_Injector
{
    public class SettingsManager
    {
        private static readonly string settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Caspian") + "\\settings.json";

        public Settings Settings { get; private set; }


        public void SaveSettings()
        {
            var jsonString = JsonConvert.SerializeObject(Settings, Formatting.Indented);
            File.WriteAllText(settingsFilePath, jsonString);
        }

        public void LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                var jsonString = File.ReadAllText(settingsFilePath);
                Settings = JsonConvert.DeserializeObject<Settings>(jsonString);
            }
            else
            {
                Settings = new Settings();
            }
        }
    }

    public class Settings
    {
        public bool UseCustomDll { get; set; }
        public string DllPath { get; set; }
    }
}
