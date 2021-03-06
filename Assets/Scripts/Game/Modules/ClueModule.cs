using Frame.Runtime.Modules;
using Game.Config;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Game.Modules {
    public class ClueModule : IModule {
        private static ClueModule _Instance;
        public static ClueModule Instance => _Instance ??= new ClueModule();

        private CClue[] _clueConfs;
        private Dictionary<int, bool> _clueStateDict;

        public bool NeedUpdate { get; } = false;
        public int LeftLockedClueCount { get; private set; }

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
            CHabitat habitatConf = CHabitat.Get(GameSceneModule.Instance.GetCurSceneConf().habitatID);
            if (habitatConf == null) {
                _clueConfs = null;
                LeftLockedClueCount = 0;
                _clueStateDict.Clear();
            } else {
                _clueConfs = CClue.GetArray().Where(clueConf => clueConf.habitatID == habitatConf.id).ToArray();
                LeftLockedClueCount = _clueConfs.Length;
                _clueStateDict.Clear();
                foreach (var conf in _clueConfs) {
                    _clueStateDict.Add(conf.id, false);
                }
            }
        }

        private void UnlockClue(int id) {
            if (_clueStateDict.TryGetValue(id, out var unlocked)) {
                if (!unlocked) {
                    _clueStateDict[id] = true;
                    --LeftLockedClueCount;
                    Facade.Clue.OnClueUnlocked?.Invoke(id);
                }
            }
        }

        public CClue[] GetCurHabitatClueConfs() {
            return _clueConfs;
        }

        public bool GetClueUnlocked(int id) {
            return _clueStateDict.TryGetValue(id, out var unlocked) && unlocked;
        }
    }
}
