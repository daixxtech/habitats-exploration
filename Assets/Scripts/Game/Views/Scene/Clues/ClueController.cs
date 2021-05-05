using Game.Config;
using UnityEngine;

namespace Game.Views.Scene {
    public class ClueController : MonoBehaviour {
        public ConfClue Conf { get; set; }

        private void Awake() {
            Conf = ConfClue.Get(1);
        }
    }
}
