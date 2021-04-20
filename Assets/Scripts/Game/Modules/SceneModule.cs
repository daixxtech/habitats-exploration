using Game.Modules.Base;
using Game.Views.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Modules {
    public class SceneModule : IModule {
        private static SceneModule _Instance;

        public static SceneModule Instance => _Instance ?? (_Instance = new SceneModule());
        public bool NeedUpdate { get; } = false;

        public void Init() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public void Dispose() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void Update() { }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) { }

        private void OnSceneUnloaded(Scene scene) { }

        public void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadSceneAsync(string sceneName) {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            UIModule.Instance.ShowUI(UIDef.LOADING, operation);
        }
    }
}
