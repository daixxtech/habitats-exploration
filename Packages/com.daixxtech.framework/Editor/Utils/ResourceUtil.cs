using FrameworkRuntime.Modules.Resource;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FrameworkEditor.Utils {
    public static class ResourceUtil {
        [MenuItem("Tools/Resources/Build AssetBundle")]
        public static void BuildAssetBundle() {
            if (!Directory.Exists(Application.streamingAssetsPath)) {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            var manifest = BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.None, BuildTarget.Android);
            if (manifest == null) {
                Debug.LogError($"[{nameof(ResourceUtil)}] BuildAssetBundle: No AssetBundle found");
                return;
            }
            string[] bundleNames = manifest.GetAllAssetBundles();
            BundleInfo[] bundleInfos = new BundleInfo[bundleNames.Length];
            for (int i = 0; i < bundleNames.Length; i++) {
                string bundleName = bundleNames[i];
                var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
                string[] bundleAssets = bundle.GetAllAssetNames().Select(Path.GetFileName).ToArray();
                string[] bundleDependencies = manifest.GetAllDependencies(bundleName);
                bundleInfos[i] = new BundleInfo(bundleName, bundleAssets, bundleDependencies);
                bundle.Unload(true);
                Debug.Log($"[{nameof(ResourceUtil)}] BuildAssetBundle: Build {bundleName}");
            }
            string infoPath = Path.Combine(Application.streamingAssetsPath, "BundleInfos.json");
            File.WriteAllText(infoPath, JsonConvert.SerializeObject(bundleInfos));
            AssetDatabase.Refresh();
        }
    }
}
