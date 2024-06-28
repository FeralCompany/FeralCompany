using BepInEx;
using FeralCompany.Core;
using FeralCompany.Core.Locale;
using FeralCompany.Modules.Map;
using FeralCompany.Modules.SettingsMenu;
using GameNetcodeStuff;
using HarmonyLib;

namespace FeralCompany;

[BepInPlugin(Guid, Name, Version)]
public class Feral : BaseUnityPlugin
{
    private const string Guid = MyPluginInfo.PLUGIN_GUID;
    private const string Name = MyPluginInfo.PLUGIN_NAME;
    private const string Version = MyPluginInfo.PLUGIN_VERSION;

    internal static HUDManager HUD { get; private set; } = null!;
    internal static StartOfRound Round => StartOfRound.Instance;

    internal static PlayerControllerB Player
    {
        get => _internalPlayer;
        set
        {
            _internalPlayer = value;
            Events.InvokePlayerAssigned(value);
        }
    }

    // Core
    internal static FeralSettings Settings { get; private set; } = null!;
    internal static FeralOutput IO { get; private set; } = null!;
    internal static FeralAssets Assets { get; private set; } = null!;
    internal static FeralLocales Locales { get; private set; } = null!;
    internal static FeralEvents Events { get; private set; } = null!;

    // Modules
    internal static SettingsMenu SettingsMenu { get; private set; } = null!;
    internal static FeralMap Map { get; private set; } = null!;

    private static PlayerControllerB _internalPlayer = null!;

    private void Awake()
    {
        IO = new FeralOutput(Logger);
        Settings = new FeralSettings(Config);

        Assets = new FeralAssets();
        if (!Assets.Load())
            return;

        Locales = new FeralLocales();
        if (Locales.Init())
            return;

        Events = new FeralEvents();

        new Harmony(Guid).PatchAll();
    }

    private void Start()
    {
        Events.OnPlayerAssigned += player =>
        {
            IO.Info("Player was assigned!");
            if (Map) Destroy(Map);
            if (SettingsMenu) Destroy(SettingsMenu);

            Map = FeralMap.Create();
            SettingsMenu = SettingsMenu.Create();

            Map.MapUI.Open();
            SettingsMenu.SettingsUI.Open();
        };
    }
}
