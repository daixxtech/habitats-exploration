using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Views.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.COMPLETION)]
    public class CompletionUIHandler : UIHandlerBase {
        private void Awake() {
            Button continueBtn = transform.Find("Root/Content/ContinueBtn").GetComponent<Button>();
            continueBtn.onClick.AddListener(() => UIModule.Instance.HideUI(UIDef.COMPLETION));
            Button backBtn = transform.Find("Root/Content/BackBtn").GetComponent<Button>();
            backBtn.onClick.AddListener(() => {
                AsyncOperation operation = SceneManager.LoadSceneAsync(SceneDef.START);
                operation.allowSceneActivation = false;
                UIModule.Instance.HideUIAll();
                UIModule.Instance.ShowUI(UIDef.LOADING, operation);
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
