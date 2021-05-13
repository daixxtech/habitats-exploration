﻿using Frame.Runtime.Modules.Assets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Frame.Runtime.Modules {
    public class AssetModule : IModule {
        private static AssetModule _Instance;
        public static AssetModule Instance => _Instance ??= new AssetModule();

        private Dictionary<string, string[]> _dependenciesDict;
        private Dictionary<string, string> _assetDict;
        private Dictionary<string, AssetBundle> _bundleDict;

        public bool NeedUpdate { get; } = false;

        public void Init() {
            _dependenciesDict = new Dictionary<string, string[]>();
            _assetDict = new Dictionary<string, string>();
            _bundleDict = new Dictionary<string, AssetBundle>();

            string infoPath = Path.Combine(Application.streamingAssetsPath, "BundleInfos.json");
            UnityWebRequest request = new UnityWebRequest(infoPath) {downloadHandler = new DownloadHandlerBuffer()};
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            while (!operation.isDone) { }

            string content = request.downloadHandler.text;
            BundleInfo[] bundleInfos = JsonConvert.DeserializeObject<BundleInfo[]>(content);
            _assetDict = new Dictionary<string, string>(3);
            for (int i = 0, count = bundleInfos.Length; i < count; i++) {
                BundleInfo bundleInfo = bundleInfos[i];
                _dependenciesDict.Add(bundleInfo.name, bundleInfo.dependencies);
                int assetLength = bundleInfo.assets.Length;
                for (int j = 0; j < assetLength; j++) {
                    _assetDict.Add(bundleInfo.assets[j], bundleInfo.name);
                }
            }
        }

        public void Dispose() {
            foreach (var pair in _bundleDict) {
                pair.Value.Unload(true);
            }
        }

        public void Update() { }

        public T LoadAsset<T>(string name) where T : Object {
            name = name.ToLower();
            if (!_assetDict.TryGetValue(name, out string bundleName)) {
                return null;
            }
            if (!_bundleDict.TryGetValue(bundleName, out var bundle)) {
                bundle = LoadBundle(bundleName);
            }
            return bundle ? bundle.LoadAsset<T>(name) : null;
        }

        private AssetBundle LoadBundle(string name) {
            string path = Path.Combine(Application.streamingAssetsPath, name);
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            while (!operation.isDone) { }

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
            if (bundle) {
                string[] dependencies = _dependenciesDict[name];
                if (dependencies != null) {
                    int length = dependencies.Length;
                    for (int i = 0; i < length; i++) {
                        LoadBundle(dependencies[i]);
                    }
                }
                _bundleDict.Add(name, bundle);
            }
            return bundle;
        }
    }
}
