using Frame.Runtime.Modules;
using Frame.Runtime.Modules.Scenes;
using Game.Views.UI;
using UnityEngine;

namespace Game.Views.Scene {
    [SceneBind(SceneDef.HABITAT_PANDA)]
    public class HabitatPandaSceneHandler : MonoBehaviour {
        public void Awake() {
            UIModule.Instance.HideUIAll();
            UIModule.Instance.ShowUI(UIDef.HUD);
        }
    }
}
