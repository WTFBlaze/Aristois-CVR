using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.Player;
using Aristois.Components;
using Aristois.Core;
using Aristois.Utils;
using HarmonyLib;
using MelonLoader;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Aristois.Modules.Core
{
    internal class Patches : ModuleBase
    {
        protected override string ModuleName => "Patches";
        protected override bool HideFromList => true;

        public override void OnStart()
        {
            Logs.Log($"{ColorManager.Console.Blue}[PATCHES] {ColorManager.Console.Gray}Creating Patches...");
            new Patch(typeof(PuppetMaster).GetMethod(nameof(PuppetMaster.AvatarInstantiated)), null, Patch.GetPatch(nameof(Patch_AvatarInstantiated)));
            new Patch(typeof(CVR_MenuManager).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance), null, Patch.GetPatch(nameof(Patch_MenuCreated)));
            new Patch(typeof(CVR_MenuManager).GetMethod(nameof(CVR_MenuManager.ToggleQuickMenu)), null, Patch.GetPatch(nameof(Patch_MenuToggled)));
            new Patch(AccessTools.Constructor(typeof(PlayerDescriptor)), null, Patch.GetPatch(nameof(Patch_PlayerJoined)));
            Patch.CreatePatches();
        }

        #region Patching Utils
        public class Patch
        {
            private static readonly List<Patch> Patches = new();
            private static int _failed;
            private MethodBase TargetMethod { get; set; }   
            private HarmonyMethod PrefixMethod { get; set; }
            private HarmonyMethod PostfixMethod { get; set; }
            private HarmonyLib.Harmony Instance { get; set; }
            private bool ShowLog { get; set; }

            public Patch(MethodBase targetMethod, HarmonyMethod before = null, HarmonyMethod after = null, bool showLog = true)
            {
                if (targetMethod is null || before is null && after is null)
                {
                    _failed++;
                    return;
                }
                Instance = new HarmonyLib.Harmony($"Aristois:{targetMethod.Name}:{CVRUtils.RANDOM.Next(100000, 999999)}");
                TargetMethod = targetMethod;
                PrefixMethod = before;
                PostfixMethod = after;
                ShowLog = showLog;
                Patches.Add(this);
            }

            public static void CreatePatches()
            {
                foreach (var patch in Patches)
                {
                    try
                    {
                        patch.Instance.Patch(patch.TargetMethod, patch.PrefixMethod, patch.PostfixMethod);
                    }
                    catch
                    {
                        _failed++;
                        Logs.Log($"{ColorManager.Console.Blue}[PATCHES] {ColorManager.Console.Gray}Failed to patch {patch.TargetMethod.Name}!");
                    }
                }
                Logs.Log($"{ColorManager.Console.Blue}[PATCHES] {ColorManager.Console.Gray}Finished Creating Patches with {ColorManager.Console.Green}{Patches.Count - _failed} Successful {ColorManager.Console.Gray}and {ColorManager.Console.Red}{_failed} Failed{ColorManager.Console.Gray}!");
            }

            public static void RemovePatches()
                => HarmonyLib.Harmony.UnpatchAll();

            public static HarmonyMethod GetPatch(string name)
                => new(typeof(Patches).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }
        #endregion

        private static void Patch_AvatarInstantiated(PuppetMaster __instance)
        {
            foreach (var m in ModuleManager.Modules) 
                m.OnAvatarInstantiated(__instance);
        }

        private static void Patch_MenuCreated()
        {
            foreach (var m in ModuleManager.Modules)
                m.OnUILoaded();
        }

        private static void Patch_MenuToggled(bool __0)
        {
            foreach (var m in ModuleManager.Modules)
                m.OnUIToggled(__0);
        }

        private static void Patch_PlayerJoined(PlayerDescriptor __instance)
        {
            MelonCoroutines.Start(RunMe());
            IEnumerator RunMe()
            {
                yield return new WaitForSeconds(1f);
                bool isLocal = string.IsNullOrEmpty(__instance.userName);

                __instance.gameObject.AddComponent<ObjectHandler>().OnDestroy_E += () =>
                {
                    foreach (var m in ModuleManager.Modules)
                        m.OnPlayerLeft(__instance, isLocal);
                };

                foreach (var m in ModuleManager.Modules)
                    m.OnPlayerJoined(__instance, isLocal);
            }
        }
    }
}
