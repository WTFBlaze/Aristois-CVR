using ABI_RC.Core.Savior;
using Aristois.Core;
using Aristois.Utils;
using MelonLoader;
using System.Collections;
using UnityEngine.Networking;

namespace Aristois.Modules.Core
{
    internal class BlacklistManager : ModuleBase
    {
        protected override bool HideFromList => false;
        protected override string ModuleName => "Blacklist Manager";

        public static bool CanUseRiskyFunctions { get; private set; } = false;

        public override void OnWorldLoaded()
            => MelonCoroutines.Start(FetchWorldFunctionState());

        private IEnumerator FetchWorldFunctionState()
        {
            while (MetaPort.Instance?.CurrentWorldId is null)
                yield return null;
            var request = UnityWebRequest.Get("https://wtfblaze.com/aristois/worldblacklist?id=" + CVRUtils.GetWorldID());
            yield return request.SendWebRequest();
            while (!request.isDone) { }
            switch (request.responseCode)
            {
                case 200:
                    if (request.downloadHandler.text == "True")
                    {
                        CanUseRiskyFunctions = false;
                        Logs.Log("This world doesn't support Risky Functions! All risky functions have been disabled!");
                    }
                    else
                    {
                        CanUseRiskyFunctions = true;
                        Logs.Log("This world supports Risky Functions!");
                    }
                    break;

                default:
                    Logs.Error(string.Format("There was an issue determining if this world can allow risky functions! Risky Functions have been disabled for this world as a pre-ca| {0}", request.error));
                    CanUseRiskyFunctions = false;
                    break;
            }
            request.Dispose();
            foreach (var m in ModuleManager.Modules)
                m.OnRiskyDetermined(CanUseRiskyFunctions);
        }
    }
}