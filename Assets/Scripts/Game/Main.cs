using Game.Modules;
using Game.Modules.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Main : MonoBehaviour {
        private List<IModule> _moduleLs;

        private void Awake() {
            _moduleLs = new List<IModule>();
            // General modules
            _moduleLs.Add(UIModule.Instance);
            _moduleLs.Add(SceneModule.Instance);
            _moduleLs.Add(ResourceModule.Instance);
            // Functional modules
            _moduleLs.Add(ArchiveModule.Instance);
            // Module initialization
            foreach (var module in _moduleLs) {
                module.Init();
            }
            // Go to Start scene
            SceneModule.Instance.LoadScene("Start");
        }

        private void Update() {
            foreach (var module in _moduleLs) {
                if (module.NeedUpdate) {
                    module.Update();
                }
            }
        }

        private void OnDestroy() {
            foreach (var module in _moduleLs) {
                module.Dispose();
            }
        }
    }
}
