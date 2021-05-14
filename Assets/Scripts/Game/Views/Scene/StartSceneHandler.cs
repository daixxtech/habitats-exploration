using Frame.Runtime.Modules;
using Frame.Runtime.Modules.Scenes;
using Game.Views.UI;
using UnityEngine;

namespace Game.Views.Scene {
    [SceneBind(SceneDef.START)]
    public class StartSceneHandler : MonoBehaviour {
        private void Start() {
            UIModule.Instance.HideUIAll();
            UIModule.Instance.ShowUI(UIDef.START);
        }
    }
}
