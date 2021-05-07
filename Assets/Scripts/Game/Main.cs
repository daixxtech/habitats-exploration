using Game.Config;
using Game.Modules;
using Game.Modules.Base;
using Game.Views.UI;
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
            SceneModule.Instance.LoadScene((int) ESceneDef.Start);
        }

        private void Update() {
            foreach (var module in _moduleLs) {
                if (module.NeedUpdate) {
                    module.Update();
                }
            }
            if (Input.GetButtonDown("Cancel") && SceneModule.Instance.CurScene.canPause) {
                UIModule.Instance.ShowUI(UIDef.PAUSE);
            }
        }

        private void OnDestroy() {
            foreach (var module in _moduleLs) {
                module.Dispose();
            }
        }
    }
}
