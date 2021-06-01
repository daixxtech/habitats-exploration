using Frame.Runtime.Modules;
using System;
using System.Collections.Generic;

namespace Game.Modules {
    public enum ENoticePopup {
        ExplorationCompleted = 1
    }

    public class NoticeModule : IModule {
        private static NoticeModule _Instance;
        public static NoticeModule Instance => _Instance ??= new NoticeModule();

        private Dictionary<int, bool> _popupStateDict;

        public bool NeedUpdate { get; } = false;

        public void Init() {
            _popupStateDict = new Dictionary<int, bool>();
            foreach (int popup in Enum.GetValues(typeof(ENoticePopup))) {
                _popupStateDict.Add(popup, true);
            }
        }

        public void Dispose() { }

        public void Update() { }

        public bool GetPopupState(ENoticePopup popup) {
            _popupStateDict.TryGetValue((int) popup, out var state);
            return state;
        }

        public void SetPopupState(ENoticePopup popup, bool state) {
            if (_popupStateDict.ContainsKey((int) popup)) {
                _popupStateDict[(int) popup] = state;
            }
        }
    }
}
