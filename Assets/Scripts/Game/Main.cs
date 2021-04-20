using Game.Modules.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Main : MonoBehaviour {
        private List<IModule> _moduleLs;

        private void Awake() {
            _moduleLs = new List<IModule>();
            // General modules

            // Functional modules

            // Module initialization
            foreach (var module in _moduleLs) {
                module.Init();
            }
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
