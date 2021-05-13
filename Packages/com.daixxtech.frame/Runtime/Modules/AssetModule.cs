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

        private Dictionary<string, BundleInfo> _infoDict;
        private Dictionary<string, string> _resDict;
        private Dictionary<string, AssetBundle> _bundleDict;

        public bool NeedUpdate { get; } = false;

        public void Init() {
            _infoDict = new Dictionary<string, BundleInfo>();
            _resDict = new Dictionary<string, string>();
            _bundleDict = new Dictionary<string, AssetBundle>();

            string infoPath = Path.Combine(Application.streamingAssetsPath, "BundleInfos.json");
            UnityWebRequest request = new UnityWebRequest(infoPath) {downloadHandler = new DownloadHandlerBuffer()};
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            while (!operation.isDone) { }

            string content = request.downloadHandler.text;
            BundleInfo[] bundleInfos = JsonConvert.DeserializeObject<BundleInfo[]>(content);
            _resDict = new Dictionary<string, string>(3);
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

        public T LoadAsset<T>(string assetName) where T : Object {
            assetName = assetName.ToLower();
            if (!_resDict.TryGetValue(assetName, out string bundleName)) {
                return null;
            }
            if (!_bundleDict.TryGetValue(bundleName, out var bundle)) {
                bundle = LoadBundle(bundleName);
            }
            return bundle ? bundle.LoadAsset<T>(assetName) : null;
        }

        private AssetBundle LoadBundle(string bundleName) {
            string path = Path.Combine(Application.streamingAssetsPath, bundleName);
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path);
            UnityWebRequestAsyncOperation operation = request.SendWebRequest();
            while (!operation.isDone) { }

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
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