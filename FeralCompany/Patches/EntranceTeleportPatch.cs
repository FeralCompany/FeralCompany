using FeralCompany.Modules;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Text;

namespace FeralCompany.Patches
{
    [HarmonyPatch(typeof(EntranceTeleport))]
    public class EntranceTeleportPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Awake")]
        public static void Awake(EntranceTeleport __instance)
        {
            CurrentRoundData.CreatePointer(__instance.entranceId, __instance.entrancePoint.position, __instance.isEntranceToBuilding);
            Feral.Events.InvokePointersCreated(CurrentRoundData._pointers.ToArray());
        }
    }
}
