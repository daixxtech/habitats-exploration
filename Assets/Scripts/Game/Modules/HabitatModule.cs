using Frame.Runtime.Modules;
using Game.Config;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Game.Modules {
    public class HabitatModule : IModule {
        private static HabitatModule _Instance;
        public static HabitatModule Instance => _Instance ??= new HabitatModule();

        private CHabitat _habitatConf;

        public bool NeedUpdate { get; }

        public void Init() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void Dispose() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void Update() { }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            CScene sceneConf = CScene.GetArray().FirstOrDefault(conf => conf.name == scene.name);
            _habitatConf = sceneConf == null ? null : CHabitat.Get(sceneConf.habitatID);
        }

        public CHabitat GetCurHabitatConf() {
            return _habitatConf;
        }
    }
}
