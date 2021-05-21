using Frame.Runtime.Modules.Scenes;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Frame.Runtime.Modules {
    public class SceneModule : IModule {
        private static SceneModule _Instance;
        public static SceneModule Instance => _Instance ??= new SceneModule();

        private Dictionary<string, Type> _typeDict;

        public bool NeedUpdate { get; } = false;

        public void Init() {
            _typeDict = new Dictionary<string, Type>();
            Type[] types = Assembly.GetCallingAssembly().GetExportedTypes();
            Type baseType = typeof(MonoBehaviour);
            for (int i = 0, typeCount = types.Length; i < typeCount; i++) {
                if (types[i].IsClass && types[i].BaseType == baseType) {
                    var bind = types[i].GetCustomAttribute<SceneBindAttribute>();
                    if (bind != null) {
                        _typeDict.Add(bind.name, types[i]);
                    }
                }
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void Dispose() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (_typeDict.TryGetValue(scene.name, out var type)) {
                GameObject.Find("Scene").gameObject.AddComponent(type);
            }
        }

        public void Update() { }
    }
}
