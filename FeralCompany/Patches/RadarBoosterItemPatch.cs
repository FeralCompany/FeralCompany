using HarmonyLib;

namespace FeralCompany.Patches;

[HarmonyPatch(typeof(RadarBoosterItem))]
internal static class RadarBoosterItemPatch
{
    [HarmonyPostfix]
    [HarmonyPatch("AddBoosterToRadar")]
    private static void Postfix_AddBoosterToRadar(RadarBoosterItem __instance) => Feral.Events.InvokeAddRadarBooster(__instance);

    [HarmonyPostfix]
    [HarmonyPatch("RemoveBoosterFromRadar")]
    private static void Postfix_RemoveBoosterFromRadar(RadarBoosterItem __instance) => Feral.Events.InvokeRemoveRadarBooster(__instance);
}
