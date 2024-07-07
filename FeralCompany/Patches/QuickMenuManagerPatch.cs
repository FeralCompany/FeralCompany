using HarmonyLib;

namespace FeralCompany.Patches;

[HarmonyPatch(typeof(QuickMenuManager))]
public static class QuickMenuManagerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(QuickMenuManager.LeaveGameConfirm))]
    private static void PostFix_LeaveGameConfirm() => Feral.Events.InvokeExitGame();
}
