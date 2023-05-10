using UnityEngine;

namespace Aristois.Serialization.External
{
    internal class JsonGradient
    {
        public JsonColor Start { get; set; }
        public JsonColor End { get; set; }

        public JsonGradient(Color start, Color end)
        {
            Start = new JsonColor(start);
            End = new JsonColor(end);
        }

        public JsonGradient(string startHex, string endHex)
        {
            Start = new JsonColor(startHex);
            End = new JsonColor(endHex);
        }
    }
}
