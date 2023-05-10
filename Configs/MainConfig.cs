using Aristois.Utils;
using System.IO;

namespace Aristois.Configs
{
    internal class MainConfig
    {
        public static MainConfig Instance { get; private set; }

        public readonly string Category_Visual = "========[VISUAL]========";
        public bool CapsuleEsp { get; set; } = false;

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
