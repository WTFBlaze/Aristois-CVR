using Aristois.Core;
using Aristois.Utils;
using System;
using System.Collections.Generic;

namespace Aristois.Modules
{
    internal static class ModuleManager
    {
        public static List<ModuleBase> Modules { get; }

        static ModuleManager() => Modules = new List<ModuleBase>();

        public static void RegisterModule<T>(T source) where T : ModuleBase
        {
            if (Modules.Exists(x => string.Equals(x.GetModuleName(), source.GetModuleName(), StringComparison.CurrentCultureIgnoreCase)))
            {
                Logs.Error($"Failed to register module {source.GetModuleName()} as one already exists with that name!");
                return;
            }
            Modules.Add(source);
            if (!source.GetHideFromList())
                Logs.Log($"Successfully registered module: {ColorManager.Console.Cyan}{source.GetModuleName()}");
        }
    }
}
