using Frame.Runtime.Modules;
using Game.Modules;
using Game.Views.Scene;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {
    public class Main : MonoBehaviour {
        private List<IModule> _moduleLs;

        private void Awake() {
            /* 加载模块 */
            _moduleLs = new List<IModule>();
            // 通用模块
            _moduleLs.Add(UIModule.Instance);
            _moduleLs.Add(SceneModule.Instance);
            _moduleLs.Add(AssetModule.Instance);
            _moduleLs.Add(InputModule.Instance);
            // 功能模块
            _moduleLs.Add(GameSceneModule.Instance);
            _moduleLs.Add(ArchiveModule.Instance);
            _moduleLs.Add(ClueModule.Instance);
            _moduleLs.Add(NoticeModule.Instance);
            // 模块初始化
            foreach (var module in _moduleLs) {
                module.Init();
            }
            /* 进入 Start 场景 */
            SceneManager.LoadScene(SceneDef.START);
            Application.targetFrameRate = 60;
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
