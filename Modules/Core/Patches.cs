using ABI_RC.Core.InteractionSystem;
using ABI_RC.Core.IO;
using ABI_RC.Core.Player;
using Aristois.Core;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Aristois.Modules.Core
{
    internal class Patches : ModuleBase
    {
        protected override string ModuleName => "Patches";
        protected override bool HideFromList => true;

        public override void OnStart()
        {
            ApplyPatches(typeof(CVRPlayerManagerJoin));
            ApplyPatches(typeof(CVRPlayerEntityLeave));
            ApplyPatches(typeof(PuppetMasterPatch));
        }

        private static void ApplyPatches(Type type)
        {
            try {
                HarmonyLib.Harmony.CreateAndPatchAll(type, "Aristois_Patches");
            } catch (Exception ex) {
                Logs.Error(ex);
            }
        }

        #region Our Methods
        private static void PlayerJoined(CVRPlayerEntity player)
        {
            foreach (var m in ModuleManager.Modules)
                m.OnPlayerJoined(player);
        }

        private static void PlayerLeft(CVRPlayerEntity player)
        {
            Logs.Log("LEFT: " + player.Username);
        }

        private static void WorldLoaded()
        {
            foreach (var m in ModuleManager.Modules)
                m.OnWorldLoaded();
        }
        #endregion

        [HarmonyPatch]
        class CVRPlayerManagerJoin
        {
            private static readonly MethodInfo _targetMethod = typeof(List<CVRPlayerEntity>).GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
            private static readonly MethodInfo _userJoinMethod = typeof(Patches).GetMethod("PlayerJoined", BindingFlags.Static | BindingFlags.NonPublic);
            private static readonly FieldInfo _playerEntity = typeof(CVRPlayerManager).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).Single(t => t.GetField("p") != null).GetField("p");

            static MethodInfo TargetMethod()
            {
                return typeof(CVRPlayerManager).GetMethod(nameof(CVRPlayerManager.TryCreatePlayer), BindingFlags.Instance | BindingFlags.Public);
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var code = new CodeMatcher(instructions)
                    .MatchForward(true, new CodeMatch(OpCodes.Callvirt, _targetMethod))
                    .Insert(
                        new CodeInstruction(OpCodes.Ldloc_0),
                        new CodeInstruction(OpCodes.Ldfld, _playerEntity),
                        new CodeInstruction(OpCodes.Call, _userJoinMethod)
                    )
                    .InstructionEnumeration();

                return code;
            }
        }

        [HarmonyPatch]
        class CVRPlayerEntityLeave
        {
            private static readonly MethodInfo _userLeaveMethod = typeof(Patches).GetMethod("PlayerLeft", BindingFlags.Static | BindingFlags.NonPublic);

            static MethodInfo TargetMethod()
            {
                return typeof(CVRPlayerEntity).GetMethod(nameof(CVRPlayerEntity.Recycle), BindingFlags.Instance | BindingFlags.Public);
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var code = new CodeMatcher(instructions)
                    .Advance(1)
                    .Insert(
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Call, _userLeaveMethod)
                    )
                    .InstructionEnumeration();

                return code;
            }
        }

        [HarmonyPatch(typeof(CVRObjectLoader))]
        class CVRObjectLoaderPatch
        {
            private static readonly MethodInfo _worldLoadedMethod = typeof(Patches).GetMethod("WorldLoaded", BindingFlags.Static | BindingFlags.NonPublic);
            private static readonly MethodInfo _targetMethod = typeof(AssetBundle).GetMethod(nameof(AssetBundle.Unload), BindingFlags.Public | BindingFlags.Instance);

            [HarmonyPatch(nameof(CVRObjectLoader.LoadIntoWorld), MethodType.Enumerator)]
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var code = new CodeMatcher(instructions)
                    .MatchForward(true, new CodeMatch(OpCodes.Ldc_I4_0), new CodeMatch(OpCodes.Callvirt, _targetMethod))
                    .Insert(
                        new CodeInstruction(OpCodes.Call, _worldLoadedMethod)
                    )
                    .InstructionEnumeration();
                return code;
            }
        }

        [HarmonyPatch(typeof(PuppetMaster))]
        class PuppetMasterPatch
        {
            [HarmonyPatch(nameof(PuppetMaster.AvatarInstantiated))]
            [HarmonyPostfix]
            static void AvatarInstantiated(PuppetMaster __instance)
            {
                foreach (var m in ModuleManager.Modules)
                    m.OnAvatarInstantiated(__instance);
            }
        }

        [HarmonyPatch(typeof(CVR_MenuManager))]
        class CVR_MenuManagerPatch
        {
            [HarmonyPatch("Start")]
            [HarmonyPostfix]
            static void Start(ref CVR_MenuManager __instance)
            {
                foreach (var m in ModuleManager.Modules)
                    m.OnUILoaded(ref __instance);
            }

            [HarmonyPatch(nameof(CVR_MenuManager.ToggleQuickMenu))]
            [HarmonyPostfix]
            static void ToggleQuickMenu(bool __0)
            {
                foreach (var m in ModuleManager.Modules)
                    m.OnUIToggled(__0);
            }
        }

        #region Old Patch Code
        /*        public override void OnStart()
                {
                    Logs.Log($"{ColorManager.Console.Blue}[PATCHES] {ColorManager.Console.Gray}Creating Patches...");
                    new Patch(typeof(PuppetMaster).GetMethod(nameof(PuppetMaster.AvatarInstantiated)), null, Patch.GetPatch(nameof(Patch_AvatarInstantiated)));
                    new Patch(typeof(CVR_MenuManager).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance), null, Patch.GetPatch(nameof(Patch_MenuCreated)));
                    new Patch(typeof(CVR_MenuManager).GetMethod(nameof(CVR_MenuManager.ToggleQuickMenu)), null, Patch.GetPatch(nameof(Patch_MenuToggled)));
                    new Patch(AccessTools.Constructor(typeof(PlayerDescriptor)), null, Patch.GetPatch(nameof(Patch_PlayerJoined)));
                    new Patch(typeof(CVRObjectLoader).GetMethod(nameof(CVRObjectLoader.LoadIntoWorld)), null, null, Patch.GetPatch(nameof(Transpiler_LoadIntoWorld)));
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
                    private HarmonyMethod TranspilerMethod { get; set; }
                    private HarmonyLib.Harmony Instance { get; set; }
                    private bool ShowLog { get; set; }

                    public Patch(MethodBase targetMethod, HarmonyMethod before = null, HarmonyMethod after = null, HarmonyMethod transpiler = null, bool showLog = true)
                    {
                        if (targetMethod is null || before is null && after is null && transpiler is null)
                        {
                            _failed++;
                            return;
                        }
                        Instance = new HarmonyLib.Harmony($"Aristois:{targetMethod.Name}:{CVRUtils.RANDOM.Next(100000, 999999)}");
                        TargetMethod = targetMethod;
                        PrefixMethod = before;
                        PostfixMethod = after;
                        TranspilerMethod = transpiler;
                        ShowLog = showLog;
                        Patches.Add(this);
                    }

                    public static void CreatePatches()
                    {
                        foreach (var patch in Patches)
                        {
                            try
                            {
                                patch.Instance.Patch(patch.TargetMethod, patch.PrefixMethod, patch.PostfixMethod, patch.TranspilerMethod);
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

                private static void Patch_MenuCreated(CVR_MenuManager __instance)
                {
                    foreach (var m in ModuleManager.Modules)
                        m.OnUILoaded(ref __instance);
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

                // Credits to the folks over at TotallyWholesome for this harmony patch! <3
                private static readonly MethodInfo _worldLoadedMethod = typeof(Patches).GetMethod(nameof(Patch_WorldLoaded), BindingFlags.Static | BindingFlags.NonPublic);
                private static readonly MethodInfo _targetMethod = typeof(AssetBundle).GetMethod(nameof(AssetBundle.Unload), BindingFlags.Public | BindingFlags.Instance);
                private static IEnumerable<CodeInstruction> Transpiler_LoadIntoWorld(IEnumerable<CodeInstruction> instructions)
                {
                    var code = new CodeMatcher(instructions)
                        .MatchForward(true, new CodeMatch(OpCodes.Ldc_I4_0), new CodeMatch(OpCodes.Callvirt, _targetMethod))
                        .Insert(
                            new CodeInstruction(OpCodes.Call, _worldLoadedMethod)
                        )
                        .InstructionEnumeration();

                    return code;
                }

                private static void Patch_WorldLoaded()
                {
                    Logs.Log("WorldLoaded Called!");
                }*/
        #endregion
    }
}
