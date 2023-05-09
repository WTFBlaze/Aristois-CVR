using Aristois.Modules;

namespace Aristois.Configs
{
    internal class Config
    {
        public static MainConfig Main => MainConfig.Instance;

        public static void LoadConfigs()
        {
            MainConfig.Load();
            foreach (var m in ModuleManager.Modules) 
                m.OnConfigsLoaded();
        }

        public static void SaveConfigs()
        {
            Main.Save();
        }
    }
}
