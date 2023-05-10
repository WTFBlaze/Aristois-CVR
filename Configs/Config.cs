using Aristois.Modules;

namespace Aristois.Configs
{
    internal class Config
    {
        public static MainConfig Main => MainConfig.Instance;
        public static ColorConfig Colors => ColorConfig.Instance;

        public static void LoadConfigs()
        {
            MainConfig.Load();
            ColorConfig.Load();
            foreach (var m in ModuleManager.Modules) 
                m.OnConfigsLoaded();
        }

        public static void SaveConfigs()
        {
            Main.Save();
            Colors.Save();
        }
    }
}
