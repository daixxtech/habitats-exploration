using Frame.Runtime.Modules;
using Game.Config;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Game.Modules {
    public class ClueModule : IModule {
        private static ClueModule _Instance;
        public static ClueModule Instance => _Instance ??= new ClueModule();

        public bool NeedUpdate { get; } = false;

        private ConfClue[] _clueConfs;
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
            _clueConfs = ConfClue.GetArray().Where(clue => ConfScene.Get(ConfHabitat.Get(clue.habitatID).sceneID).name == scene.name).ToArray();
            foreach (var conf in _clueConfs) {
                _clueStateDict.Add(conf.id, false);
            }
        }

        private void UnlockClue(int id) {
            if (_clueStateDict.TryGetValue(id, out var unlocked)) {
                if (!unlocked) {
                    _clueStateDict[id] = true;
                    Facade.Clue.OnClueUnlocked?.Invoke(id);
                }
            }
        }

        public ConfClue[] GetCurSceneClueConfs() {
            return _clueConfs;
        }

        public bool GetClueUnlocked(int id) {
            return _clueStateDict.TryGetValue(id, out var unlocked) && unlocked;
        }
    }
}
