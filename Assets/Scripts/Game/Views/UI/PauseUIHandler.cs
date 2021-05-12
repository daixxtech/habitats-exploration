using Framework.Modules;
using Framework.Modules.UI;
using Framework.Views.UI;
using Game.Config;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.PAUSE)]
    public class PauseUIHandler : AUIHandler {
        private void Awake() {
            Button continueBtn = transform.Find("Root/Menu/ContinueBtn").GetComponent<Button>();
            continueBtn.onClick.AddListener(() => UIModule.Instance.HideUI(UIDef.PAUSE));
            Button backBtn = transform.Find("Root/Menu/BackBtn").GetComponent<Button>();
            backBtn.onClick.AddListener(() => {
                UIModule.Instance.HideUI(UIDef.PAUSE);
                SceneModule.Instance.LoadSceneAsync((int) ESceneDef.Start);
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
