using Aristois.Serialization.External;
using Aristois.Utils;
using System.IO;

namespace Aristois.Configs
{
    internal class ColorConfig
    {
        public static ColorConfig Instance { get; private set; }

        // Esp Colors & Toggles
        public bool RainbowEsp { get; set; } = false;
        public bool GradientEsp { get; set; } = false;
        public bool ScanLinesEsp { get; set; } = false;
        public JsonColor EspColor { get; set; } = new JsonColor(ColorManager.Hex.Magenta);
        public JsonGradient EspGradient { get; set; } = new JsonGradient(ColorManager.Hex.Aqua, ColorManager.Hex.Magenta);

        // Rank Colors
        public JsonColor UserColor { get; set; } = new JsonColor(ColorManager.Hex.Green);
        public JsonColor LegendColor { get; set; } = new JsonColor(ColorManager.Hex.Yellow);
        public JsonColor GuideColor { get; set; } = new JsonColor(ColorManager.Hex.Aqua);
        public JsonColor ModeratorColor { get; set; } = new JsonColor(ColorManager.Hex.Red);
        public JsonColor DeveloperColor { get; set; } = new JsonColor(ColorManager.Hex.DarkRed);

        public static void Load()
        {
            if (!File.Exists(PathManager.ColorsFile))
                JsonManager.WriteToJsonFile(PathManager.ColorsFile, new ColorConfig());
            Instance = JsonManager.ReadFromJsonFile<ColorConfig>(PathManager.ColorsFile);
        }

        public void Save()
        {
            JsonManager.WriteToJsonFile(PathManager.ColorsFile, Instance);
        }
    }
}
