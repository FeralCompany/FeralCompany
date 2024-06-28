using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FeralCompany_Unity.Editor
{
    public class CompileAssetBundles : MonoBehaviour
    {
        private const string LogPrefix = "<b>AssetBundler:</b> ";
        private const string AssetBundlesDir = "AssetBundles";

        private const BuildAssetBundleOptions Options = BuildAssetBundleOptions.StrictMode;
        private const BuildTarget Target = BuildTarget.StandaloneWindows;

        [MenuItem("FeralCompany/Compile AssetBundles")]
        public static void Execute_CompileAssetBundles()
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
                    return;
                }

                Debug.Log($"{LogPrefix}Successfully compiled AssetBundles: {string.Join(", ", manifest.GetAllAssetBundles())}");
            }
            catch (Exception e)
            {
                Debug.LogError($"{LogPrefix}Failed to compile AssetBundles: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}
