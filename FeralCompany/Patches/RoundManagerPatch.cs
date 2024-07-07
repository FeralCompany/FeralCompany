using HarmonyLib;

namespace FeralCompany.Patches;

[HarmonyPatch(typeof(RoundManager))]
public static class RoundManagerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("RefreshEnemiesList")]
    private static void PostFix_RefreshEnemiesList() => Feral.Events.InvokeEnterMoon();
}
