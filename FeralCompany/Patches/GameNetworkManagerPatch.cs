using HarmonyLib;
using Unity.Netcode;

namespace FeralCompany.Patches;

[HarmonyPatch(typeof(GameNetworkManager))]
internal static class GameNetworkManagerPatch
{
    private static bool _subscribed;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameNetworkManager.SubscribeToConnectionCallbacks))]
    private static void PostFix_SubscribeToConnectionCallbacks()
    {
        if (!_subscribed) return;

        NetworkManager.Singleton.OnClientConnectedCallback += FeralCompany.Events.InvokeClientConnect;
        NetworkManager.Singleton.OnClientDisconnectCallback += FeralCompany.Events.InvokeClientDisconnect;
        _subscribed = true;
    }
}
