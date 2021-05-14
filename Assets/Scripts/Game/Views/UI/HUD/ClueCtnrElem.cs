using Game.Config;
using Game.Modules;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI.HUD {
    public class ClueCtnrElem : MonoBehaviour {
        private Button _button;
        private Text _nameTxt;

        private ConfClue _conf;
        private bool _unlocked;
        public Action<ConfClue> onClicked;

        private void Awake() {
            _button = transform.GetComponent<Button>();
            _button.onClick.AddListener(() => onClicked?.Invoke(_conf));
            _nameTxt = transform.Find("Text").GetComponent<Text>();
        }

        private void OnEnable() {
            Facade.Clue.OnClueUnlocked += RefreshInfo;
        }

        private void OnDisable() {
            Facade.Clue.OnClueUnlocked -= RefreshInfo;
        }

        public void SetInfo(ConfClue conf) {
            _conf = conf;
            _unlocked = ClueModule.Instance.GetClueUnlocked(_conf.id);
            _button.interactable = _unlocked;
            _nameTxt.text = _unlocked ? _conf.name : "线索未发现";
        }

        private void RefreshInfo(int id) {
            if (_conf.id == id) {
                SetInfo(_conf);
            }
        }
    }
}
