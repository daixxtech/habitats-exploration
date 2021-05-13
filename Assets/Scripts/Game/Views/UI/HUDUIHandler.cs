using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Views.UI.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.HUD)]
    public class HUDUIHandler : UIHandlerBase {
        private GameObject _interactionCpnt;

        private void Awake() {
            transform.Find("Root/Joystick").gameObject.AddComponent<JoystickElem>();
            Button pauseBtn = transform.Find("Root/PauseBtn").GetComponent<Button>();
            pauseBtn.onClick.AddListener(() => UIModule.Instance.ShowUI(UIDef.PAUSE));

            _interactionCpnt = transform.Find("Root/Interaction").gameObject;
        }

        public void OnEnable() {
            Facade.Player.TriggeredClue += OnPlayerTriggeredClue;
        }

        private void OnDisable() {
            Facade.Player.TriggeredClue -= OnPlayerTriggeredClue;
        }

        private void OnPlayerTriggeredClue(Collider other, bool enter) {
            _interactionCpnt.SetActive(enter);
        }
    }
}
