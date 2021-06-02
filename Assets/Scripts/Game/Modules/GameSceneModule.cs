using Frame.Runtime.Modules;
using Game.Config;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Game.Modules {
    public class GameSceneModule : IModule {
        private static GameSceneModule _Instance;
        public static GameSceneModule Instance => _Instance ??= new GameSceneModule();

        private CScene _sceneConf;

        public bool NeedUpdate { get; } = false;

        public void Init() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void Dispose() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void Update() { }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            _sceneConf = CScene.GetArray().FirstOrDefault(conf => conf.name == scene.name);
        }

        public CScene GetCurSceneConf() {
            return _sceneConf;
        }
    }
}
