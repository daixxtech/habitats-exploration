using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Modules;
using Game.Views.Scene;
using Game.Views.UI.HUD;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.HUD)]
    public class HUDUIHandler : UIHandlerBase {
        private Button _interactBtn;
        private Text _clueCountTxt;
        private UIContainer _clueCtnr;

        private int _clueCount;
        private CClue _clueConf;

        private void Awake() {
            _interactBtn = transform.Find("Root/InteractBtn").GetComponent<Button>();
            _interactBtn.onClick.AddListener(() => Facade.Player.OnInteractedClue?.Invoke(_clueConf.id));
            _clueCountTxt = transform.Find("Root/CluesPanel/Title/ClueCountTxt").GetComponent<Text>();
            _clueCtnr = transform.Find("Root/CluesPanel/Ctnr/Viewport/Content").gameObject.AddComponent<UIContainer>();

            transform.Find("Root/Joystick").gameObject.AddComponent<JoystickElem>();
            Button pauseBtn = transform.Find("Root/PauseBtn").GetComponent<Button>();
            pauseBtn.onClick.AddListener(() => UIModule.Instance.ShowUI(UIDef.PAUSE));
        }

        public void OnEnable() {
            _interactBtn.gameObject.SetActive(false);

            CClue[] confs = ClueModule.Instance.GetCurSceneClueConfs();
            _clueCount = confs.Length;
            _clueCtnr.SetCount<ClueCtnrElem>(_clueCount);
            Action<CClue> onClicked = conf => UIModule.Instance.ShowUI(UIDef.CLUE_TIPS, conf);
            for (int i = 0; i < _clueCount; i++) {
                var elem = (ClueCtnrElem) _clueCtnr.Children[i];
                elem.SetInfo(confs[i]);
                elem.onClicked = onClicked;
            }
            RefreshClueCount(0);

            Facade.Player.OnTriggeredClue += OnPlayerTriggeredClue;
            Facade.Clue.OnClueUnlocked += RefreshClueCount;
        }

        private void OnDisable() {
            Facade.Player.OnTriggeredClue -= OnPlayerTriggeredClue;
            Facade.Clue.OnClueUnlocked -= RefreshClueCount;
        }

        private void OnPlayerTriggeredClue(Collider other, bool enter) {
            _interactBtn.gameObject.SetActive(enter);
            _clueConf = enter ? other.GetComponent<ClueController>().Conf : null;
        }

        private void RefreshClueCount(int clueID) {
            _clueCountTxt.text = string.Format("{0} / {1}", _clueCount - ClueModule.Instance.LeftLockedClueCount, _clueCount);
        }
    }
}
