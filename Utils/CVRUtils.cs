using ABI_RC.Core.Player;
using ABI_RC.Core.Savior;
using System;
using System.Collections.Generic;

namespace Aristois.Utils
{
    internal static class CVRUtils
    {
        public static readonly Random RANDOM = new();

        internal static List<CVRPlayerEntity> GetAllPlayers()
            => CVRPlayerManager.Instance.NetworkPlayers;

        internal static string GetInstanceID()
            => MetaPort.Instance.CurrentInstanceId;

        internal static string GetWorldID()
            => MetaPort.Instance.CurrentWorldId;

        internal static string GetWorldName()
            => MetaPort.Instance.CurrentInstanceName;
    }
}
