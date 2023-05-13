using ABI_RC.Core.InteractionSystem;
using Aristois.Core;
using BTKUILib;
using BTKUILib.UIObjects;
using System;
using System.IO;
using System.Reflection;

namespace Aristois.Modules.Core
{
    internal class UserInterface : ModuleBase
    {
        protected override bool HideFromList => true;
        protected override string ModuleName => "User Interface";

        public static Page MainPage { get; private set; }
        public static Category Category_Esp { get; private set; }
        public static Category Category_Esp_Settings { get; private set; }

        public override void OnUILoaded(ref CVR_MenuManager menuManager)
        {
            Logs.Log("UserInterface.OnUILoaded called!");

            #region Register Icons
            try
            {
                QuickMenuAPI.PrepareIcon(ModInfo.Name, "Logo", GetIcon("Logo.png"));
            }
            catch (Exception ex) { Logs.Error(ex); }
            #endregion

            #region Main Page
            try
            {
                MainPage = new Page(ModInfo.Name, "Main", true, "Logo")
                {
                    MenuTitle = $"{ModInfo.Name} v{ModInfo.Version}",
                    MenuSubtitle = $"The User Interface for {ModInfo.Name}"
                };
            }
            catch (Exception ex) { Logs.Error(ex); }
            #endregion

            #region Categories
            try
            {
                Category_Esp = MainPage.AddCategory("Esp");
                Category_Esp_Settings = MainPage.AddCategory("Esp Settings");
            }
            catch (Exception ex) { Logs.Error(ex); }
            #endregion
        }

        private Stream GetIcon(string fileName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream($"Aristois.Resources.Icons.{fileName}");
        }
    }
}
