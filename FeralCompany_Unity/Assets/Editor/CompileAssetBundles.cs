using System;
using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace FeralCompany.Unity.Editor
{
    public class CompileAssetBundles : MonoBehaviour
    {
        private const string LogPrefix = "<b>AssetBundler:</b> ";
        private const string AssetBundlesDir = "AssetBundles";
        private const string UserSettingsFile = "user_settings.json";

        private const BuildAssetBundleOptions Options = BuildAssetBundleOptions.StrictMode;
        private const BuildTarget Target = BuildTarget.StandaloneWindows;

        [Serializable]
        public class UserSettings
        {
            public string assetBundlesDestination;

            public static bool TryParseUserSettings([CanBeNull] out UserSettings settings)
            {
                try
                {
                    var json = File.ReadAllText(UserSettingsFile);
                    settings = JsonUtility.FromJson<UserSettings>(json);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError($"{LogPrefix}Failed to parse user settings: {UserSettingsFile}");
                    Debug.LogError($"{e.Message}\n{e.StackTrace}");
                    settings = null;
                    return false;
                }
            }
        }

        [MenuItem("FeralCompany/Deploy AssetBundles", false, 3)]
        public static void Execute_DeployAssetBundles()
        {
            if (!UserSettings.TryParseUserSettings(out var settings))
                return;
            if (!TryCompileAssetBundles(out var assetBundles))
                return;

            try
            {
                foreach (var assetBundle in assetBundles!)
                {
                    var target = Path.Combine(AssetBundlesDir, assetBundle);
                    var dest = Path.Combine(settings!.assetBundlesDestination, assetBundle);
                    Debug.Log($"{LogPrefix}{target} => {dest}");
                    File.Delete(dest);
                    File.Copy(target, dest);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{LogPrefix}Failed to deploy AssetBundle: {e.Message}\n{e.StackTrace}");
            }
        }

        private static bool TryCompileAssetBundles([CanBeNull] out string[] files)
        {
            if (Directory.Exists(AssetBundlesDir))
                Directory.Delete(AssetBundlesDir, true);
            Directory.CreateDirectory(AssetBundlesDir);

            try
            {
                var manifest = BuildPipeline.BuildAssetBundles(AssetBundlesDir, Options, Target);
                if (manifest is null)
                {
                    Debug.LogError($"{LogPrefix}Unknown error occurred while compiling AssetBundles!");
                    files = null;
                    return false;
                }

                Debug.Log($"{LogPrefix}Successfully compiled AssetBundles: {string.Join(", ", manifest.GetAllAssetBundles())}");
                files = manifest.GetAllAssetBundles();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"{LogPrefix}Failed to compile AssetBundles: {e.Message}\n{e.StackTrace}");
                files = null;
                return false;
            }
        }
    }
}
