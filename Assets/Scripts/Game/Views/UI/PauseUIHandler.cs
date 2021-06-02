using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Views.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.PAUSE)]
    public class PauseUIHandler : UIHandlerBase {
        private void Awake() {
            Button continueBtn = transform.Find("Root/Content/ContinueBtn").GetComponent<Button>();
            continueBtn.onClick.AddListener(() => UIModule.Instance.HideUI(UIDef.PAUSE));
            Button backBtn = transform.Find("Root/Content/BackBtn").GetComponent<Button>();
            backBtn.onClick.AddListener(() => {
                UIModule.Instance.HideUIAll();
                UIModule.Instance.ShowUI(UIDef.LOADING, SceneDef.START);
            });
        }

        public void OnEnable() {
            Time.timeScale = 0;
        }

        private void OnDisable() {
            Time.timeScale = 1;
        }
    }
}
