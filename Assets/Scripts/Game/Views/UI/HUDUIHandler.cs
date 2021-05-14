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
        private UIContainer _clueCtnr;

        private ConfClue _clueConf;

        private void Awake() {
            _interactBtn = transform.Find("Root/InteractBtn").GetComponent<Button>();
            _interactBtn.onClick.AddListener(() => Facade.Player.OnInteractedClue?.Invoke(_clueConf.id));
            _clueCtnr = transform.Find("Root/CluesPanel/Ctnr/Viewport/Content").gameObject.AddComponent<UIContainer>();

            transform.Find("Root/Joystick").gameObject.AddComponent<JoystickElem>();
            Button pauseBtn = transform.Find("Root/PauseBtn").GetComponent<Button>();
            pauseBtn.onClick.AddListener(() => UIModule.Instance.ShowUI(UIDef.PAUSE));
        }

        public void OnEnable() {
            _interactBtn.gameObject.SetActive(false);

            ConfClue[] confArr = ClueModule.Instance.GetCurSceneClueConfs();
            int count = confArr.Length;
            _clueCtnr.SetCount<ClueCtnrElem>(count);
            Action<ConfClue> onClicked = conf => UIModule.Instance.ShowUI(UIDef.CLUE_TIPS, conf);
            for (int i = 0; i < count; i++) {
                var elem = (ClueCtnrElem) _clueCtnr.Children[i];
                elem.SetInfo(confArr[i]);
                elem.onClicked = onClicked;
            }

            Facade.Player.OnTriggeredClue += OnPlayerTriggeredClue;
        }

        private void OnDisable() {
            Facade.Player.OnTriggeredClue -= OnPlayerTriggeredClue;
        }

        private void OnPlayerTriggeredClue(Collider other, bool enter) {
            _interactBtn.gameObject.SetActive(enter);
            _clueConf = enter ? other.GetComponent<ClueController>().Conf : null;
        }
    }
}
