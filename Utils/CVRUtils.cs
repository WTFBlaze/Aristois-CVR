using ABI_RC.Core.Player;
using System;
using System.Collections.Generic;

namespace Aristois.Utils
{
    internal static class CVRUtils
    {
        public static readonly Random RANDOM = new();

        internal static List<CVRPlayerEntity> GetAllPlayers()
            => CVRPlayerManager.Instance.NetworkPlayers;
    }
}
