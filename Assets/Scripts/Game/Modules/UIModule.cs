using Game.Modules.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Modules {
    public class UIModule : IModule {
        private static UIModule _Instance;
        private Transform _uiRoot;
        private Dictionary<string, GameObject> _uiDict;

        public static UIModule Instance => _Instance ?? (_Instance = new UIModule());
        public bool NeedUpdate { get; } = false;
        public object Param { get; private set; }

        public void Init() {
            _uiRoot = GameObject.Find("UIRoot").transform;
            Object.DontDestroyOnLoad(_uiRoot);
            _uiDict = new Dictionary<string, GameObject>();
        }

        public void Dispose() {
            foreach (var pair in _uiDict) {
                Object.Destroy(pair.Value);
            }
        }

        public void Update() { }

        public void ShowUI(string uiName, object param = null) {
            if (!_uiDict.TryGetValue(uiName, out var go)) {
                go = _uiRoot.Find(uiName)?.gameObject;
                if (go == null) {
                    Debug.LogError($"[{nameof(UIModule)}] ShowUI: Cannot find UIHandler named {uiName}");
                    return;
                }
                _uiDict.Add(uiName, go);
            }
            Param = param;
            go.SetActive(true);
        }

        public void HideUI(string uiName) {
            if (_uiDict.TryGetValue(uiName, out var go)) {
                go.SetActive(false);
            } else {
                Debug.LogError($"[{nameof(UIModule)}] HideUI: Cannot find UIHandler named {uiName}");
            }
        }

        public void HideUIAll() {
            foreach (var pair in _uiDict) {
                pair.Value.SetActive(false);
            }
        }
    }
}
