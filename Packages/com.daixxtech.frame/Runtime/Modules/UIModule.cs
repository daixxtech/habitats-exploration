using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Frame.Runtime.Modules {
    public class UIModule : IModule {
        private const int DESTROY_TIME = 10;

        private static UIModule _Instance;
        public static UIModule Instance => _Instance ??= new UIModule();

        private Transform _root;
        private Dictionary<string, Type> _typeDict;
        private Dictionary<string, UIHandlerBase> _handlerDict;
        private List<string> _destroyList;

        public bool NeedUpdate { get; } = true;
        public Camera UICamera { get; private set; }
        public object Parameter { get; private set; }

        public void Init() {
            _root = GameObject.Find("UIRoot").transform;
            UICamera = _root.Find("UICamera").GetComponent<Camera>();
            UnityEngine.Object.DontDestroyOnLoad(_root);

            _typeDict = new Dictionary<string, Type>();
            _handlerDict = new Dictionary<string, UIHandlerBase>();
            _destroyList = new List<string>();

            Type[] types = Assembly.GetCallingAssembly().GetExportedTypes();
            Type baseType = typeof(UIHandlerBase);
            for (int i = 0, typeCount = types.Length; i < typeCount; i++) {
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
                UIHandlerBase handler = pair.Value;
                if (!handler.gameObject.activeSelf && (handler.DestroyTimer -= Time.deltaTime) <= 0) {
                    _destroyList.Add(pair.Key);
                }
            }
            foreach (var name in _destroyList) {
                UnityEngine.Object.Destroy(_handlerDict[name].gameObject);
                _handlerDict.Remove(name);
            }
        }

        public void ShowUI(string name, object param = null) {
            Parameter = param;
            if (_handlerDict.TryGetValue(name, out var handler)) {
                handler.gameObject.SetActive(true);
            } else {
                if (_typeDict.TryGetValue(name, out var type)) {
                    GameObject prefab = AssetModule.Instance.LoadAsset<GameObject>(name);
                    GameObject instance = UnityEngine.Object.Instantiate(prefab, _root);
                    handler = (UIHandlerBase) instance.AddComponent(type);
                    _handlerDict.Add(name, handler);
                    handler.GetComponent<Canvas>().worldCamera = UICamera;
                } else {
                    throw new Exception($"[UIModule] ShowUI: Cannot find script bound with {name}");
                }
            }
            handler.DestroyTimer = DESTROY_TIME;
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
