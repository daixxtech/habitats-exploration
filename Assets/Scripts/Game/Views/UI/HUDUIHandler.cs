using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Views.Scene;
using Game.Views.UI.HUD;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.HUD)]
    public class HUDUIHandler : UIHandlerBase {
        private Button _interactBtn;
        private ClueCtnrElem[] _clueCtnrElems;

        private ConfClue _clueConf;

        private void Awake() {
            transform.Find("Root/Joystick").gameObject.AddComponent<JoystickElem>();
            Button pauseBtn = transform.Find("Root/PauseBtn").GetComponent<Button>();
            pauseBtn.onClick.AddListener(() => UIModule.Instance.ShowUI(UIDef.PAUSE));

            _interactBtn = transform.Find("Root/InteractBtn").GetComponent<Button>();
            _interactBtn.onClick.AddListener(() => UIModule.Instance.ShowUI(UIDef.CLUE_TIPS, _clueConf));

            Transform ctnr = transform.Find("Root/CluesPanel/Ctnr/Viewport/Content");
            GameObject template = ctnr.Find("Template").gameObject;
            _clueCtnrElems = new ClueCtnrElem[4];
            Action<ConfClue> onClicked = conf => UIModule.Instance.ShowUI(UIDef.CLUE_TIPS, conf);
            for (int i = 0; i < 4; i++) {
                _clueCtnrElems[i] = Instantiate(template, ctnr).AddComponent<ClueCtnrElem>();
                _clueCtnrElems[i].Clicked += onClicked;
            }
            template.SetActive(false);
        }

        public void OnEnable() {
            _interactBtn.gameObject.SetActive(false);

            string sceneName = SceneManager.GetActiveScene().name;
            ConfClue[] confArr = ConfClue.GetArray().Where(clue => ConfScene.Get(ConfHabitat.Get(clue.habitatID).sceneID).name == sceneName).ToArray();
            int count = confArr.Length;
            for (int i = 0; i < count; i++) {
                _clueCtnrElems[i].gameObject.SetActive(true);
                _clueCtnrElems[i].SetInfo(confArr[i]);
            }
            for (int i = count; i < 4; i++) {
                _clueCtnrElems[i].gameObject.SetActive(false);
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
