using System;
using System.Diagnostics.CodeAnalysis;
using BepInEx;
using FeralCompany.Core;
using FeralCompany.Core.Locale;
using FeralCompany.Modules;
using FeralCompany.Modules.Map;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace FeralCompany;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Feral : BaseUnityPlugin
{
    // Core
    internal static FeralSettings Settings { get; private set; } = null!;
    internal static FeralOutput IO { get; private set; } = null!;
    internal static FeralAssets Assets { get; private set; } = null!;
    internal static FeralLocales Locales { get; private set; } = null!;
    internal static FeralEvents Events { get; private set; } = null!;
    internal static FeralBindings Bindings { get; private set; } = null!;

    // Game
    internal static HUDManager HUD => HUDManager.Instance;
    internal static StartOfRound StartOfRound => StartOfRound.Instance;
    internal static PlayerControllerB Player { get; private set; } = null!;

    // Modules
    internal static CurrentRoundData CurrentRound { get; private set; } = null!;
    internal static GlobalData Globals { get; private set; } = null!;
    internal static FeralMap Map { get; private set; } = null!;
    internal static FeralNightVision NightVision { get; private set; } = null!;

    // Internal
    private static Feral _instance = null!;
    private Harmony _harmony = null!;
    private GameObject _binder = null!;

    private void Awake()
    {
        _instance = this;

        IO = new FeralOutput(Logger);
        Settings = new FeralSettings(Config);

        Assets = new FeralAssets();
        if (!Assets.Load())
            return;

        Locales = new FeralLocales();
        if (!Locales.Init())
            return;

        Events = new FeralEvents();

        Bindings = new FeralBindings();
        Bindings.Disable();

        _harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        _harmony.PatchAll(typeof(AwaitGameStart));

        Events.OnExitGame += Unload;
    }

    private void Unload()
    {
        _harmony.UnpatchSelf();
        Destroy(_binder);
        Bindings.Disable();
        _harmony.PatchAll(typeof(AwaitGameStart));
    }

    private void TriggerGame(PlayerControllerB player)
    {
        Bindings.Enable();
        Player = player;

        _binder = new GameObject("FeralGame");

        CurrentRound = _binder.AddComponent<CurrentRoundData>();

        Globals = _binder.AddComponent<GlobalData>();
        Map = _binder.AddComponent<FeralMap>();
        NightVision = _binder.AddComponent<FeralNightVision>();

        _harmony.PatchAll();
    }

    private void OnDestroy()
    {
        if (_binder)
            Destroy(_binder);
    }

    [HarmonyPatch(typeof(PlayerControllerB))]
    private static class AwaitGameStart
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PlayerControllerB.ConnectClientToPlayerObject))]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static void Postfix_ConnectClientToPlayerObject(PlayerControllerB __instance)
        {
            if (GameNetworkManager.Instance.localPlayerController == __instance)
                _instance.TriggerGame(GameNetworkManager.Instance.localPlayerController);
        }
    }
}
