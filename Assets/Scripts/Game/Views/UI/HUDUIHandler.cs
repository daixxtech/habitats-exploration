using Framework.Modules.UI;
using Framework.Views.UI;
using UnityEngine;

namespace Game.Views.UI {
    [UIBind(UIDef.HUD)]
    public class HUDUIHandler : AUIHandler {
        private GameObject _interactionCpnt;

        protected override void Awake() {
            base.Awake();

            _interactionCpnt = transform.Find("Root/Interaction").gameObject;
        }

        protected override void OnEnable() {
            base.OnEnable();

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
