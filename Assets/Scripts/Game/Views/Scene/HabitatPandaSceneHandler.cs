using Frame.Runtime.Modules;
using Frame.Runtime.Modules.Scenes;
using Game.Config;
using Game.Views.UI;
using UnityEngine;

namespace Game.Views.Scene {
    [SceneBind(SceneDef.HABITAT_PANDA)]
    public class HabitatPandaSceneHandler : MonoBehaviour {
        public void Awake() {
            UIModule.Instance.HideUIAll();
            UIModule.Instance.ShowUI(UIDef.HUD);

            Facade.Player.OnInteractedClue += ShowClueTips;
        }

        private void OnDestroy() {
            Facade.Player.OnInteractedClue -= ShowClueTips;
        }

        private static void ShowClueTips(int clueID) {
            UIModule.Instance.ShowUI(UIDef.CLUE_TIPS, ConfClue.Get(clueID));
        }
    }
}
