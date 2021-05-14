using Game.Config;
using UnityEngine;

namespace Game.Views.Scene {
    public class ClueController : MonoBehaviour {
        public ConfClue Conf { get; private set; }

        public void SetInfo(ConfClue conf) {
            Conf = conf;
        }
    }
}
