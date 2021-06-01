using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Modules;
using Game.Views.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.COMPLETION)]
    public class CompletionUIHandler : UIHandlerBase {
        private Toggle _doNotPopupAgain;

        private void Awake() {
            _doNotPopupAgain = transform.Find("Root/Content/DoNotPopupAgain").GetComponent<Toggle>();

            Button continueBtn = transform.Find("Root/Content/ContinueBtn").GetComponent<Button>();
            continueBtn.onClick.AddListener(() => {
                NoticeModule.Instance.SetPopupState(ENoticePopup.ExplorationCompleted, !_doNotPopupAgain.isOn);
                UIModule.Instance.HideUI(UIDef.COMPLETION);
            });
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
