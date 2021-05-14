using Frame.Runtime.Modules;
using Game.Config;
using Game.Views.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Game.Modules {
    public class ClueModule : IModule {
        private static ClueModule _Instance;
        public static ClueModule Instance => _Instance ??= new ClueModule();

        public bool NeedUpdate { get; } = false;

        private Dictionary<int, bool> _clueStateDict;

        public void Init() {
            _clueStateDict = new Dictionary<int, bool>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Facade.Player.OnInteractedClue += UnlockClue;
        }

        public void Dispose() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Facade.Player.OnInteractedClue -= UnlockClue;
        }

        public void Update() { }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            _clueStateDict.Clear();
            foreach (var conf in ConfClue.GetArray().Where(clue => ConfScene.Get(ConfHabitat.Get(clue.habitatID).sceneID).name == scene.name)) {
                _clueStateDict.Add(conf.id, false);
            }
        }

        private void UnlockClue(int id) {
            if (_clueStateDict.TryGetValue(id, out var _)) {
                _clueStateDict[id] = true;
                Facade.Clue.OnClueUnlocked?.Invoke(id);
                UIModule.Instance.ShowUI(UIDef.CLUE_TIPS, ConfClue.Get(id));
            }
        }

        public bool GetClueUnlocked(int id) {
            return _clueStateDict.TryGetValue(id, out var unlocked) && unlocked;
        }
    }
}
