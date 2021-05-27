using Frame.Runtime.Modules;
using Frame.Runtime.Modules.Assets;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Frame.Editor.Utils {
    public static class AssetUtil {
        [MenuItem("Tools/Assets/Build AssetBundle")]
        public static void BuildAssetBundle() {
            if (!Directory.Exists(Application.streamingAssetsPath)) {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            var manifest = BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.None, BuildTarget.Android);
            if (manifest == null) {
                Debug.LogError("[AssetUtil] BuildAssetBundle: No AssetBundle found");
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
                Debug.Log($"[AssetUtil] BuildAssetBundle: Build {bundleName}");
            }
            string infoPath = Path.Combine(Application.streamingAssetsPath, AssetModule.BUNDLE_INFO_FILE_NAME);
            File.WriteAllText(infoPath, JsonConvert.SerializeObject(bundleInfos));
            AssetDatabase.Refresh();
        }
    }
}
