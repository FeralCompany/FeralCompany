using HarmonyLib;

using UnityEngine;

namespace FeralCompany.Patches;

[HarmonyPatch(typeof(RoundManager))]
public static class RoundManagerPatch
{
    public static int lastSeed { get; set; }

    [HarmonyPostfix]
    [HarmonyPatch("RefreshEnemiesList")]
    private static void PostFix_RefreshEnemiesList()
    {
        if (lastSeed == StartOfRound.Instance.randomMapSeed)
        {
            return;
        }
        lastSeed = StartOfRound.Instance.randomMapSeed;
        Feral.Events.InvokeEnterMoon();
    } 
}
