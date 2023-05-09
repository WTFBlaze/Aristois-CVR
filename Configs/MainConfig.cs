using Aristois.Modules;
using Aristois.Utils;
using System.IO;

namespace Aristois.Configs
{
    internal class MainConfig
    {
        public static MainConfig Instance;

        public static void Load()
        {
            if (!File.Exists(PathManager.ConfigFile))
                JsonManager.WriteToJsonFile(PathManager.ConfigFile, new MainConfig());
            Instance = JsonManager.ReadFromJsonFile<MainConfig>(PathManager.ConfigFile);
        }

        public void Save()
        {
            JsonManager.WriteToJsonFile(PathManager.ConfigFile, Instance);
        }
    }
}
