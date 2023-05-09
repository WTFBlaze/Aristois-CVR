using System.IO;
using UnityEngine;

namespace Aristois.Utils
{
    internal static class PathManager
    {
        public static readonly string ModDir = Directory.GetParent(Application.dataPath) + "\\Aristois";
        public static readonly string LogsDir = ModDir + "\\Logs";
        private static readonly string ConfigsDir = ModDir + "\\Configs";
        public static readonly string MiscDir = ModDir + "\\Misc";

        public static readonly string ConfigFile = ConfigsDir + "\\Config.json";
        public static readonly string ColorsFile = ConfigsDir + "\\Colors.json";
        public static readonly string KeybindsFile = ConfigsDir + "\\Keybinds.json";

        public static void Initialize()
        {
            Directory.CreateDirectory(ModDir);
            Directory.CreateDirectory(LogsDir);
            Directory.CreateDirectory(ConfigsDir);
            Directory.CreateDirectory(MiscDir);
        }
    }
}
