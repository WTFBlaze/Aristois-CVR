using Aristois.Core;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Aristois.Utils
{
    internal class BundleManager
    {
        private static AssetBundle Bundle { get; set; }
        public static Shader EspShader { get; private set; }
        public static GameObject PanelPrefab { get; private set; }

        internal static void Initialize()
        {
            Logs.Log($"{ColorManager.Console.Yellow}Attempting to load Asset Bundle...");
            try
            {
                #region Load Bundle
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Aristois.Resources.aristois");
                using var tempStream = new MemoryStream((int)stream.Length);
                stream.CopyTo(tempStream);
                Bundle = AssetBundle.LoadFromMemory(tempStream.ToArray(), 0);
                Bundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                #endregion

                #region Load Assets from Bundle
                EspShader = LoadAsset<Shader>("Assets/Aristois/Esp.shader");
                PanelPrefab = LoadAsset<GameObject>("Assets/Aristois/Canvas.prefab");
                #endregion

                Logs.Log($"{ColorManager.Console.Green}Successfully loaded Asset Bundle!");
            }
            catch(System.Exception ex) { Logs.Error(ex); }
        }

        private static T LoadAsset<T>(string path) where T : Object
        {
            T asset = Bundle.LoadAsset<T>(path);
            asset.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return asset;
        }
    }
}
