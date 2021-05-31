using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Views.UI.Archives;
using System;
using UnityEngine.UI;

namespace Game.Views.UI {
    public enum EArchiveTab {
        Animal = 1, Clue = 2
    }

    [UIBind(UIDef.ARCHIVE)]
    public class ArchivesUIHandler : UIHandlerBase {
        private Button _animalTabBtn;
        private Button _clueTabBtn;

        private AnimalArchivePanel _animalArchivePanel;
        private ClueArchivePanel _clueArchivePanel;

        private EArchiveTab _currentTab = 0;

        private void Awake() {
            _animalTabBtn = transform.Find("Root/Content/Tabs/AnimalTabBtn").GetComponent<Button>();
            _animalTabBtn.onClick.AddListener(() => CheckTab(EArchiveTab.Animal));
            _clueTabBtn = transform.Find("Root/Content/Tabs/ClueTabBtn").GetComponent<Button>();
            _clueTabBtn.onClick.AddListener(() => CheckTab(EArchiveTab.Clue));

            _animalArchivePanel = transform.Find("Root/Content/AnimalArchive").gameObject.AddComponent<AnimalArchivePanel>();
            _clueArchivePanel = transform.Find("Root/Content/ClueArchive").gameObject.AddComponent<ClueArchivePanel>();

            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(() => UIModule.Instance.HideUI(UIDef.ARCHIVE));
        }

        public void OnEnable() {
            CheckTab(EArchiveTab.Animal);
        }

        private void CheckTab(EArchiveTab targetTab) {
            if (targetTab == _currentTab) {
                return;
            }
            switch (targetTab) {
            case EArchiveTab.Animal:
                _clueArchivePanel.gameObject.SetActive(false);
                _animalArchivePanel.Show();
                break;
            case EArchiveTab.Clue:
                _animalArchivePanel.gameObject.SetActive(false);
                _clueArchivePanel.Show();
                break;
            default: throw new ArgumentOutOfRangeException(nameof(targetTab), targetTab, null);
            }
            _currentTab = targetTab;
        }
    }
}
