using FrameworkRuntime.Modules.UI;
using FrameworkRuntime.Views.UI;
using Game.Views.UI.HUD;
using UnityEngine;

namespace Game.Views.UI {
    [UIBind(UIDef.HUD)]
    public class HUDUIHandler : AUIHandler {
        private GameObject _interactionCpnt;

        private void Awake() {
            transform.Find("Root/Joystick").gameObject.AddComponent<JoystickElem>();

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
