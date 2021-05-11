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
            /* 加载模块 */
            _moduleLs = new List<IModule>();
            // 通用模块
            _moduleLs.Add(UIModule.Instance);
            _moduleLs.Add(SceneModule.Instance);
            _moduleLs.Add(ResourceModule.Instance);
            _moduleLs.Add(InputModule.Instance);
            // 功能模块
            _moduleLs.Add(ArchiveModule.Instance);
            // 模块初始化
            foreach (var module in _moduleLs) {
                module.Init();
            }
            /* 进入 Start 场景 */
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
