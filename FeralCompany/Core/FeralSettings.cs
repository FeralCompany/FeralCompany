using BepInEx.Configuration;
using FeralCompany.Core.Setting;
using FeralCompany.Modules.Map;
using FeralCompany.Utils.LayerMask;

namespace FeralCompany.Core;

public class FeralSettings(ConfigFile config)
{
    internal readonly MapSettings Map = new(config);
    internal readonly GeneralSettings General = new(config);

    internal class GeneralSettings(ConfigFile config)
    {
        internal readonly FuncSetting<string, Locale.Locale> Locale = new(
            new Config<string>(config, "General", "Locale", "en_US"),
            key => Feral.Locales.Locales[key],
            locale => locale.Key
        );
    }

    internal class MapSettings(ConfigFile config)
    {
        internal readonly Config<bool> Enable = new(config, "Map", "Enable", true);
        internal readonly Config<float> Scale = new(config, "Map", "Scale", 1f);
        internal readonly Config<Rotations> Rotation = new(config, "Map", "Rotation", Rotations.Default);
        internal readonly Config<float> Zoom = new(config, "Map", "Zoom", 19.7f);

        internal readonly FuncSetting<string, Masks[]> CullingMask = new(
            new Config<string>(config, "Map", "Visible Layers", "Room,MapRadar"),
            Mask.Extract,
            Mask.ToConfigString
        );
    }
}
