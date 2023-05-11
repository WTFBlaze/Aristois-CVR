using Aristois.Configs;
using Aristois.Modules;
using Aristois.Modules.Core;
using Aristois.Modules.Visual;
using Aristois.Utils;
using MelonLoader;
using System;
using System.Linq;
using UnityEngine;

namespace Aristois.Core
{
    public class Main : MelonMod
    {
        #region MelonLoader Overrides
        public override void OnInitializeMelon()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptions;
            if (!CheckForBTKUILib())
                return;
            PathManager.Initialize();
            ConsoleManager.Initialize();
            Logs.Initialize();
            BundleManager.Initialize();

            #region Register Modules
            ModuleManager.RegisterModule(new Patches());
            ModuleManager.RegisterModule(new UserInterface());
            ModuleManager.RegisterModule(new CapsuleEsp());
            ModuleManager.RegisterModule(new DebugPanel());
            #endregion

            Config.LoadConfigs();

            foreach (var m in ModuleManager.Modules) 
                m.OnStart();
        }

        public override void OnDeinitializeMelon()
        {
            if (!Variables.IsValidStart)
                return;
            Config.SaveConfigs();
            foreach (var m in ModuleManager.Modules) 
                m.OnStop();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (!Variables.IsValidStart)
                return;
            ProcessContainer();
            foreach (var m in ModuleManager.Modules) 
                m.OnSceneLoaded(buildIndex, sceneName);
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            if (!Variables.IsValidStart)
                return;
            foreach (var m in ModuleManager.Modules) 
                m.OnSceneUnloaded(buildIndex, sceneName);
        }
        #endregion
        #region Helper Methods
        private void UnhandledExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (!Logs.IsSetup)
                MelonLogger.Error($"Message: {ex.Message} | {ex.StackTrace}");
            else
                Logs.Error(ex);
        }

        private bool CheckForBTKUILib()
        {
            var btkUiLib = RegisteredMelons.FirstOrDefault(x => x.Info.Name == "BTKUILib");
            if (btkUiLib == null)
            {
                Logs.Error("BTKUILib is not currently installed! Please install it from here. https://github.com/BTK-Development/BTKUILib/releases.\nAristois will NOT be initialized!");
                return false;
            }
            if (btkUiLib.Info.Version != "1.1.0")
                Logs.Error($"The currently installed version of BTKUILib does not match the one Aristois is built for!\nYou are running v{btkUiLib.Info.Version}. Aristois uses v1.1.0. Please consider using this version until Aristois upgrades versions!");
            Variables.IsValidStart = true;
            return true;
        }

        private void ProcessContainer()
        {
            if (Variables.Container is not null)
                return;
            Variables.Container = new GameObject("Aristois <3");
            UnityEngine.Object.DontDestroyOnLoad(Variables.Container);
            foreach (var m in ModuleManager.Modules) 
                m.OnContainerCreated();
        }
        #endregion

    }
}
