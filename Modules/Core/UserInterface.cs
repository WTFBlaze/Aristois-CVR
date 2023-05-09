using Aristois.Core;
using BTKUILib;
using BTKUILib.UIObjects;
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

        public override void OnUILoaded()
        {
            #region Register Icons
            QuickMenuAPI.PrepareIcon(ModInfo.Name, "Logo", GetIcon("Logo.png"));
            #endregion

            #region Main Page
            MainPage = new Page(ModInfo.Name, "Main", true, "Logo")
            {
                MenuTitle = $"{ModInfo.Name} v{ModInfo.Version}",
                MenuSubtitle = $"The User Interface for {ModInfo.Name}"
            };
            #endregion

            #region Categories
            Category_Esp = MainPage.AddCategory("Esp");
            #endregion
        }

        private Stream GetIcon(string fileName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream($"Aristois.Resources.Icons.{fileName}");
        }
    }
}
