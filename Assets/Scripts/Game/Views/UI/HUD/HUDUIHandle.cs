using UnityEngine;

namespace Game.Views.UI {
    public class HUDUIHandle : MonoBehaviour {
        private GameObject _interactionCpnt;

        private void Awake() {
            _interactionCpnt = transform.Find("Root/Interaction").gameObject;
        }

        private void OnEnable() {
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
