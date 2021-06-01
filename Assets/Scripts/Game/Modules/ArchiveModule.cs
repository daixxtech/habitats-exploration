using Frame.Runtime.Modules;
using Game.Config;
using System.Collections.Generic;

namespace Game.Modules {
    public class ArchiveModule : IModule {
        private static ArchiveModule _Instance;
        public static ArchiveModule Instance => _Instance ??= new ArchiveModule();

        private Dictionary<int, bool> _clueArchive;

        public bool NeedUpdate { get; } = false;

        public void Init() {
            var clueConfs = CClue.GetArray();
            _clueArchive = new Dictionary<int, bool>(clueConfs.Length);
            foreach (var clueConf in clueConfs) {
                _clueArchive.Add(clueConf.id, false);
            }

            Facade.Clue.OnClueUnlocked += UnlockClueArchive;
        }

        public void Dispose() {
            Facade.Clue.OnClueUnlocked -= UnlockClueArchive;
        }

        public void Update() { }

        public bool GetClueArchiveState(int animalID) {
            _clueArchive.TryGetValue(animalID, out bool unlocked);
            return unlocked;
        }

        private void UnlockClueArchive(int clueID) {
            if (_clueArchive.ContainsKey(clueID)) {
                _clueArchive[clueID] = true;
            }
        }
    }
}
