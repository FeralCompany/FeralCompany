using HarmonyLib;

namespace FeralCompany.Patches;

[HarmonyPatch(typeof(StartOfRound))]
public static class StartOfRoundPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("EndOfGame")]
    private static void PostFix_EndOfGame() => FeralCompany.Events.InvokeExitMoon();
}
