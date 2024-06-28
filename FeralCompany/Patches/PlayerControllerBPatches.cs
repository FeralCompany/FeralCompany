using System.Diagnostics.CodeAnalysis;
using GameNetcodeStuff;
using HarmonyLib;

namespace FeralCompany.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal static class PlayerControllerBPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControllerB.ConnectClientToPlayerObject))]
    private static void PostFix_ConnectClientToPlayerObject(
        [SuppressMessage("ReSharper", "InconsistentNaming")] PlayerControllerB __instance)
    {
        if (GameNetworkManager.Instance.localPlayerController != __instance) return;
        Feral.Player = GameNetworkManager.Instance.localPlayerController;
    }
}
