using System.IO;
using UnityEngine;

namespace FeralCompany.Core;

public class FeralAssets
{
    private const string AssetBundleName = "feralcompany_assets";
    private static string AssetPath => Path.Combine(Path.GetDirectoryName(typeof(FeralAssets).Assembly.Location)!, AssetBundleName);

    // Prefabs
    internal GameObject PrefabFeralMap = null!;
    internal GameObject PrefabMapPointer = null!;
    internal GameObject PrefabTestPrefab = null!;

    // Sprites
    internal Sprite SpriteHome = null!;
    internal Sprite SpriteExtinguisher = null!;
    internal Sprite SpriteEntrance = null!;

    internal bool Load()
    {
        var assets = AssetBundle.LoadFromFile(AssetPath);
        if (!assets)
        {
            FeralCompany.IO.Error($"Failed to load assets @ {AssetPath}!");
            FeralCompany.IO.Error("FeralCompany will shut down, now.");
            return false;
        }

        if (LoadAllAssets(assets))
            return true;

        FeralCompany.IO.Error("One or more assets failed to load.");
        FeralCompany.IO.Error("FeralCompany will shut down, now.");
        return false;
    }

    private bool LoadAllAssets(AssetBundle assets)
    {
        return
            // Prefabs
            LoadAsset(assets, "Assets/Prefabs/FeralMap.prefab", out PrefabFeralMap)
            && LoadAsset(assets, "Assets/Prefabs/MapComp/MapPointer.prefab", out PrefabMapPointer)
            && LoadAsset(assets, "Assets/Prefabs/TestPrefab.prefab", out PrefabTestPrefab)

            // Sprites
            && LoadAsset(assets, "Assets/Sprites/Home.png", out SpriteHome)
            && LoadAsset(assets, "Assets/Sprites/Entrance.png", out SpriteEntrance)
            && LoadAsset(assets, "Assets/Sprites/Extinguisher.png", out SpriteExtinguisher);
    }

    private static bool LoadAsset<T>(AssetBundle assets, string path, out T asset) where T : Object
    {
        asset = assets.LoadAsset<T>(path);
        if (asset)
            return true;

        FeralCompany.IO.Error($"Failed to load asset @ {AssetPath} => {path}");
        return false;
    }
}
