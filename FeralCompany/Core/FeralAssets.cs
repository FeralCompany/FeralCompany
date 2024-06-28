using System.IO;
using UnityEngine;

namespace FeralCompany.Core;

public class FeralAssets
{
    private const string AssetBundleName = "feralcompany_assets";
    private static string AssetPath => Path.Combine(Path.GetDirectoryName(typeof(FeralAssets).Assembly.Location)!, AssetBundleName);

    // Prefabs
    internal GameObject PrefabFeralMap = null!;
    internal GameObject PrefabSettingsMenu = null!;

    internal bool Load()
    {
        var assets = AssetBundle.LoadFromFile(AssetPath);
        if (!assets)
        {
            Feral.IO.Error($"Failed to load assets @ {AssetPath}!");
            Feral.IO.Error("FeralCompany will shut down, now.");
            return false;
        }

        if (LoadAllAssets(assets))
            return true;

        Feral.IO.Error("One or more assets failed to load.");
        Feral.IO.Error("FeralCompany will shut down, now.");
        return false;
    }

    private bool LoadAllAssets(AssetBundle assets)
    {
        return
            // Prefabs
            LoadAsset(assets, "Assets/Prefabs/FeralMap.prefab", out PrefabFeralMap)
            && LoadAsset(assets, "Assets/Prefabs/SettingsMenu.prefab", out PrefabSettingsMenu);
    }

    private static bool LoadAsset<T>(AssetBundle assets, string path, out T asset) where T : Object
    {
        asset = assets.LoadAsset<T>(path);
        if (asset)
            return true;

        Feral.IO.Error($"Failed to load asset @ {AssetPath} => {path}");
        return false;
    }
}
