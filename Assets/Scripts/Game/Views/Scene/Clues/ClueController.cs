using Game.Config;
using UnityEngine;

namespace Game.Views.Scene {
    public class ClueController : MonoBehaviour {
        public CClue Conf { get; private set; }

        public void SetInfo(CClue conf) {
            Conf = conf;
        }
    }
}
