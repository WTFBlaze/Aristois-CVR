using Aristois.Utils;
using UnityEngine;

namespace Aristois.Serialization.External
{
    internal class JsonColor
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public JsonColor(Color color)
        {
            R = color.r;
            G = color.g;
            B = color.b;
            A = color.a;
        }

        public JsonColor(string hex)
        {
            var color = ColorManager.HexToColor(hex);
            R = color.r;
            G = color.g;
            B = color.b;
            A = color.a;
        }

        public string GetHex()
        {
            return ColorManager.ColorToHex(GetColor());
        }

        public Color GetColor()
        {
            return new Color(R, G, B, A);
        }
    }
}
