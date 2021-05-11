using Game.Modules.Base;
using Game.Modules.UI;
using Game.Views.UI.Base;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Game.Modules {
    public class UIModule : IModule {
        private static UIModule _Instance;
        public static UIModule Instance => _Instance ?? (_Instance = new UIModule());

        private Transform _root;
        private Dictionary<string, Type> _typeDict;
        private Dictionary<string, AUIHandler> _handlerDict;
        private List<string> _destroyList;

        public bool NeedUpdate { get; } = true;
        public Camera UICamera { get; private set; }
        public object Param { get; private set; }

        public void Init() {
            _root = GameObject.Find("UIRoot").transform;
            UnityEngine.Object.DontDestroyOnLoad(_root);
            UICamera = _root.Find("UICamera").GetComponent<Camera>();

            _typeDict = new Dictionary<string, Type>();
            _handlerDict = new Dictionary<string, AUIHandler>();
            _destroyList = new List<string>();

            Type[] types = Assembly.GetExecutingAssembly().GetExportedTypes();
            Type baseType = typeof(AUIHandler);
            for (int i = 0, count = types.Length; i < count; i++) {
                if (types[i].IsClass && types[i].BaseType == baseType) {
                    var bind = types[i].GetCustomAttribute<UIBindAttribute>();
                    if (bind != null) {
                        _typeDict.Add(bind.name, types[i]);
                    }
                }
            }
        }

        public void Dispose() {
            foreach (var pair in _handlerDict) {
                UnityEngine.Object.Destroy(pair.Value);
            }
        }

        public void Update() {
            _destroyList.Clear();
            foreach (var pair in _handlerDict) {
                AUIHandler handler = pair.Value;
                if (!handler.gameObject.activeSelf && (handler.DestroyCountDown -= Time.deltaTime) <= 0) {
                    _destroyList.Add(pair.Key);
                }
            }
            foreach (var name in _destroyList) {
                UnityEngine.Object.Destroy(_handlerDict[name].gameObject);
                _handlerDict.Remove(name);
            }
        }

        public void ShowUI(string name, object param = null) {
            Param = param;
            if (_handlerDict.TryGetValue(name, out var handler)) {
                handler.gameObject.SetActive(true);
            } else {
                if (_typeDict.TryGetValue(name, out var type)) {
                    GameObject prefab = ResourceModule.Instance.LoadRes<GameObject>(name);
                    GameObject ui = UnityEngine.Object.Instantiate(prefab, _root);
                    handler = ui.AddComponent(type) as AUIHandler;
                    _handlerDict.Add(name, handler);
                } else {
                    throw new Exception($"[{nameof(UIModule)}] ShowUI: Cannot find script bound with {name}");
                }
            }
        }

        public void HideUI(string name) {
            if (_handlerDict.TryGetValue(name, out var handler)) {
                handler.gameObject.SetActive(false);
            }
        }

        public void HideUIAll() {
            foreach (var pair in _handlerDict) {
                pair.Value.gameObject.SetActive(false);
            }
        }
    }
}
