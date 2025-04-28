using System;
using System.Linq;
using BepInEx.Configuration;
using FeralCompany.Core.Setting;
using FeralCompany.Utils;

namespace FeralCompany.Core;

public class FeralSettings(ConfigFile config)
{
    internal readonly GeneralSettings General = new(config);
    internal readonly MapSettings Map = new(config);

    internal class GeneralSettings
    {
        internal readonly Config<int> InternalNightVisionIntensity;
        internal readonly Config<int> ExternalNightVisionIntensity;

        internal GeneralSettings(ConfigFile config)
        {
            InternalNightVisionIntensity = new Config<int>(config, "NightVision", "Facility Intensity", 0,
                "The intensity of the night vision while inside the facility.",
                new AcceptableValueRange<int>(0, 100)
            );

            ExternalNightVisionIntensity = new Config<int>(config, "NightVision", "Outside Intensity", 0,
                "The intensity of the night vision while outside the facility.",
                new AcceptableValueRange<int>(0, 100)
            );
        }
    }

    internal class MapSettings
    {
        internal readonly Config<bool> Enable;
        internal readonly Config<float> Scale;
        internal readonly Config<float> Zoom;
        internal readonly SettingBase<int> CullingMask;
        internal readonly Config<float> InternalLight;
        internal readonly Config<float> ExternalLight;

        internal readonly Config<bool> PointerShip;
        internal readonly Config<bool> PointerExternalEntrance;
        internal readonly Config<bool> PointerInternalEntrance;
        internal readonly Config<bool> PointerExternalFire;
        internal readonly Config<bool> PointerInternalFire;
        internal readonly Config<bool> PointerShipRadar;
        internal readonly Config<bool> PointerExternalEntranceRadar;
        internal readonly Config<bool> PointerInternalEntranceRadar;
        internal readonly Config<bool> PointerExternalFireRadar;
        internal readonly Config<bool> PointerInternalFireRadar;

        internal MapSettings(ConfigFile config)
        {
            Enable = new Config<bool>(config, "Map", "Enable", true, "Whether or not to enable the map.");
            Scale = new Config<float>(config, "Map", "Scale", 1f, "The size of the map.", new AcceptableValueRange<float>(0.1f, 5f));
            Zoom = new Config<float>(config, "Map", "Zoom", 19.7f, "How close the camera should follow the target.");

            InternalLight = new Config<float>(config, "Map", "Facility Brightness", 15f,
                """
                The intensity of the backlight for the map while inside the facility.
                Vanilla: 0; No lighting other than internal lights and sunlight.
                """,
                new AcceptableValueRange<float>(0f, 100f)
            );

            ExternalLight = new Config<float>(config, "Map", "Outside Brightness", 15f,
                """
                The intensity of the backlight for the map while outside the facility.
                Vanilla: 0; No lighting other than internal lights and sunlight.
                """,
                new AcceptableValueRange<float>(0f, 100f)
            );

            PointerShip = new Config<bool>(config, "Map.Pointers", "Ship", true, "Whether or not to show pointers for the ship.");
            PointerExternalEntrance = new Config<bool>(config, "Map.Pointers", "External Entrance", true, "Whether or not to show pointers for the external entrance.");
            PointerInternalEntrance = new Config<bool>(config, "Map.Pointers", "Internal Entrance", true, "Whether or not to show pointers for the internal entrance.");
            PointerExternalFire = new Config<bool>(config, "Map.Pointers", "External Fire", true, "Whether or not to show pointers for external fire exits.");
            PointerInternalFire = new Config<bool>(config, "Map.Pointers", "Internal Fire", true, "Whether or not to show pointers for internal fire exits.");

            PointerShipRadar = new Config<bool>(config, "Map.Pointers", "Ship (Radar)", true, "Whether or not to show pointers for the ship when viewing a radar.");
            PointerExternalEntranceRadar = new Config<bool>(config, "Map.Pointers", "External Entrance (Radar)", true, "Whether or not to show pointers for the external entrance when viewing a radar.");
            PointerInternalEntranceRadar = new Config<bool>(config, "Map.Pointers", "Internal Entrance (Radar)", true, "Whether or not to show pointers for the internal entrance when viewing a radar.");
            PointerExternalFireRadar = new Config<bool>(config, "Map.Pointers", "External Fire (Radar)", true, "Whether or not to show pointers for external fire exits when viewing a radar.");
            PointerInternalFireRadar = new Config<bool>(config, "Map.Pointers", "Internal Fire (Radar)", true, "Whether or not to show pointers for internal fire exits when viewing a radar.");

            var cullingMask = new Config<string>(config, "Map", "Visible Layers", "Room,MapRadar",
                $"""
                The layers of the game visible to the minimap camera. Comma-separated list.
                Valid layers:
                ${string.Join(',', Mask.Names)}
                """
            );

            CullingMask = new FuncSetting<string, int>(cullingMask,
                input => input
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(0, (current, str) => current.AddMask(Mask.Named(str))),
                input => string.Join(',', Mask.Decompress(input)));
        }
    }
}
