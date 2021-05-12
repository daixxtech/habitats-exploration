using Frame.Runtime.Modules;
using Game.Config;
using Game.Views.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Modules {
    public class SceneModule : IModule {
        private static SceneModule _Instance;
        public static SceneModule Instance => _Instance ??= new SceneModule();

        public bool NeedUpdate { get; } = false;
        public ConfScene CurScene { get; private set; }

        public void Init() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public void Dispose() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void Update() { }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            UIModule.Instance.HideUIAll();
            switch (scene.name) {
            case "Start":
                UIModule.Instance.ShowUI(UIDef.START);
                break;
            case "Habitat_Panda":
                UIModule.Instance.ShowUI(UIDef.HUD);
                break;
            }
        }

        private void OnSceneUnloaded(Scene scene) { }

        public void LoadScene(int sceneID) {
            CurScene = ConfScene.Get(sceneID);
            SceneManager.LoadScene(CurScene.name);
        }

        public void LoadSceneAsync(int sceneID) {
            CurScene = ConfScene.Get(sceneID);
            AsyncOperation operation = SceneManager.LoadSceneAsync(CurScene.name);
            operation.allowSceneActivation = false;
            UIModule.Instance.ShowUI(UIDef.LOADING, operation);
        }
    }
}
