using Game.Modules.Base;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Modules {
    public class BundleInfo {
        public string Name { get; }
        public string[] Assets { get; }
        public string[] Dependencies { get; }

        public BundleInfo(string name, string[] assets, string[] dependencies) {
            Name = name;
            Assets = assets;
            Dependencies = dependencies;
        }
    }

    public class ResourceModule : IModule {
        private static ResourceModule _Instance;
        public static ResourceModule Instance => _Instance ?? (_Instance = new ResourceModule());

        private Dictionary<string, BundleInfo> _infoDict;
        private Dictionary<string, string> _resDict;
        private Dictionary<string, AssetBundle> _bundleDict;

        public bool NeedUpdate { get; } = false;

        public void Init() {
            _infoDict = new Dictionary<string, BundleInfo>();
            _resDict = new Dictionary<string, string>();
            _bundleDict = new Dictionary<string, AssetBundle>();

            string infoPath = Path.Combine(Application.streamingAssetsPath, "BundleInfos.json");
            if (!File.Exists(infoPath)) {
                return;
            }
            BundleInfo[] bundleInfos = JsonConvert.DeserializeObject<BundleInfo[]>(File.ReadAllText(infoPath));
            for (int i = 0, count = bundleInfos.Length; i < count; i++) {
                BundleInfo bundleInfo = bundleInfos[i];
                _infoDict.Add(bundleInfo.Name, bundleInfo);
                int assetLength = bundleInfo.Assets.Length;
                for (int j = 0; j < assetLength; j++) {
                    _resDict.Add(bundleInfo.Assets[j], bundleInfo.Name);
                }
            }
        }

        public void Dispose() {
            foreach (var pair in _bundleDict) {
                pair.Value.Unload(true);
            }
        }

        public void Update() { }

        public T LoadRes<T>(string resName) where T : Object {
            resName = resName.ToLower();
            if (!_resDict.TryGetValue(resName, out string bundleName)) {
                return null;
            }
            if (!_bundleDict.TryGetValue(bundleName, out var bundle)) {
                bundle = LoadBundle(bundleName);
            }
            return bundle ? bundle.LoadAsset<T>(resName) : null;
        }

        private AssetBundle LoadBundle(string bundleName) {
            var bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
            if (bundle) {
                BundleInfo bundleInfo = _infoDict[bundleName];
                string[] dependencies = bundleInfo.Dependencies;
                if (dependencies != null) {
                    int length = dependencies.Length;
                    for (int i = 0; i < length; i++) {
                        LoadBundle(dependencies[i]);
                    }
                }
                _bundleDict.Add(bundleName, bundle);
            }
            return bundle;
        }
    }
}
