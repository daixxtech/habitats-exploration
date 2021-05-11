using Framework.Modules;
using Game.Config;
using System.Collections.Generic;

namespace Game.Modules {
    public class ArchiveModule : IModule {
        private static ArchiveModule _Instance;
        public static ArchiveModule Instance => _Instance ?? (_Instance = new ArchiveModule());

        private Dictionary<int, bool> _archive;

        public bool NeedUpdate { get; } = false;

        public void Init() {
            var confArr = ConfAnimal.GetArray();
            _archive = new Dictionary<int, bool>(confArr.Length);
            bool flag = false;
            foreach (var conf in confArr) {
                _archive.Add(conf.id, flag = !flag);
            }
        }

        public void Dispose() { }

        public void Update() { }

        public bool GetArchiveState(int animalID) {
            _archive.TryGetValue(animalID, out bool unlocked);
            return unlocked;
        }

        private void OnArchiveUnlocked(int animalID) {
            if (_archive.ContainsKey(animalID)) {
                _archive[animalID] = true;
            }
        }
    }
}
