using UnityEngine;

namespace Aristois.Utils
{
    internal static class ColorManager
    {
        public static class Hex
        {
            public const string Red = "#ff0000";
            public const string DarkRed = "#7d0000";
            public const string Blue = "#0004ff";
            public const string LightBlue = "#4C8BDE";
            public const string DarkBlue = "#00027d";
            public const string Green = "#11ff00";
            public const string DarkGreen = "#087500";
            public const string Yellow = "#fbff00";
            public const string DarkYellow = "#888a00";
            public const string Orange = "#ff7700";
            public const string Brown = "#733600";
            public const string Pink = "#e24fff";
            public const string Magenta = "#b700ff";
            public const string Purple = "#490066";
            public const string Aqua = "#00eeff";
            public const string DarkAqua = "#00757d";
            public const string Grey = "#a6a6a6";
            public const string DarkGrey = "#474747";
        }

        public static class Colors
        {
            public static Color Red { get => HexToColor(Hex.Red); }
            public static Color DarkRed { get => HexToColor(Hex.DarkRed); }
            public static Color Blue { get => HexToColor(Hex.Blue); }
            public static Color LightBlue { get => HexToColor(Hex.LightBlue); }
            public static Color DarkBlue { get => HexToColor(Hex.DarkBlue); }
            public static Color Green { get => HexToColor(Hex.Green); }
            public static Color DarkGreen { get => HexToColor(Hex.DarkGreen); }
            public static Color Yellow { get => HexToColor(Hex.Yellow); }
            public static Color DarkYellow { get => HexToColor(Hex.DarkYellow); }
            public static Color Orange { get => HexToColor(Hex.Orange); }
            public static Color Brown { get => HexToColor(Hex.Brown); }
            public static Color Pink { get => HexToColor(Hex.Pink); }
            public static Color Magenta { get => HexToColor(Hex.Magenta); }
            public static Color Purple { get => HexToColor(Hex.Purple); }
            public static Color Aqua { get => HexToColor(Hex.Aqua); }
            public static Color DarkAqua { get => HexToColor(Hex.DarkAqua); }
            public static Color Grey { get => HexToColor(Hex.Grey); }
            public static Color DarkGrey { get => HexToColor(Hex.DarkGrey); }
        }

        public static class Console
        {
            public const string Black = "\x1b[30m";
            public const string DarkGreen = "\x1b[32m";
            public const string DarkBlue = "\x1b[34m";
            public const string DarkCyan = "\x1b[36m";
            public const string DarkRed = "\x1b[31m";
            public const string DarkMagenta = "\x1b[35m";
            public const string DarkYellow = "\x1b[33m";
            public const string Gray = "\x1b[37m";
            public const string DarkGray = "\x1b[90m";
            public const string Green = "\x1b[92m";
            public const string Blue = "\x1b[94m";
            public const string Red = "\x1b[91m";
            public const string Cyan = "\x1b[96m";
            public const string Magenta = "\x1b[95m";
            public const string Yellow = "\x1b[93m";
            public const string White = "\x1b[97m";
        }

        public static Color HexToColor(string hexCode)
        {
            ColorUtility.TryParseHtmlString(hexCode, out var color);
            return color;
        }

        public static string ColorToHex(Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }

        public static string Color32ToHex(Color32 color)
        {
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }

        public static Color ShiftHueBy(Color color, float amount)
        {
            Color.RGBToHSV(color, out float hue, out float sat, out float val);
            hue += amount;
            return Color.HSVToRGB(hue, sat, val);
        }

        public static string TextGradient(string input)
        {
            string result = string.Empty;
            Color currentColor = Color.white;
            foreach (char c in input)
            {
                if (c == ' ')
                {
                    result += ' ';
                }
                else
                {
                    currentColor = ShiftHueBy(currentColor, 0.1f);
                    var hexCode = ColorToHex(currentColor);
                    result += $"<color=#{hexCode}>{c}</color>";
                }
            }
            return result;
        }

        public static string TextGradient(string input, ref Color inputColor)
        {
            string result = string.Empty;
            foreach (char c in input)
            {
                if (c == ' ')
                {
                    result += ' ';
                }
                else
                {
                    inputColor = ShiftHueBy(inputColor, 0.1f);
                    var hexCode = ColorToHex(inputColor);
                    result += $"<color=#{hexCode}>{c}</color>";
                }
            }
            return result;
        }

        public static string TextGradient(string input, Color a, Color b)
        {
            string result = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (c == ' ')
                    result += ' ';
                else
                {
                    Color newColor = Color.Lerp(a, b, Mathf.InverseLerp(0, input.Length, i));
                    var hexCode = ColorToHex(newColor);
                    result += $"<color={hexCode}>{c}</color>";
                }
            }
            return result;
        }

        public static Texture2D ColorToT2D(Color32 col)
        {
            Texture2D tex = new(1, 1);
            tex.SetPixels32(new Color32[] { col });
            tex.Apply();
            return tex;
        }
    }
}
