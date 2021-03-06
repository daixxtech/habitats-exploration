using Game.Config;
using Game.Modules;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI.HUD {
    public class ClueCtnrElem : MonoBehaviour {
        private Button _button;
        private GameObject _lockedCpnt;
        private Text _nameTxt;

        private CClue _conf;
        private bool _unlocked;
        public Action<CClue> onClicked;

        private void Awake() {
            _button = transform.GetComponent<Button>();
            _button.onClick.AddListener(() => onClicked?.Invoke(_conf));
            _nameTxt = transform.Find("Text").GetComponent<Text>();
            _lockedCpnt = transform.Find("Locked").gameObject;
        }

        private void OnEnable() {
            Facade.Clue.OnClueUnlocked += RefreshInfo;
        }

        private void OnDisable() {
            Facade.Clue.OnClueUnlocked -= RefreshInfo;
        }

        public void SetInfo(CClue conf) {
            _conf = conf;
            _unlocked = ClueModule.Instance.GetClueUnlocked(_conf.id);
            _button.interactable = _unlocked;
            _lockedCpnt.SetActive(!_unlocked);
            _nameTxt.text = _unlocked ? _conf.name : "线索未发现";
        }

        private void RefreshInfo(int id) {
            if (_conf.id == id) {
                SetInfo(_conf);
            }
        }
    }
}
