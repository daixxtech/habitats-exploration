using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Views.UI.Habitats;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.HABITATS)]
    public class HabitatsUIHandler : UIHandlerBase {
        private UIContainer _habitatCtnr;
        private GameObject _notAvailableTipsCpnt;

        private void Awake() {
            _notAvailableTipsCpnt = transform.Find("Root/NotAvailableTips").gameObject;
            _habitatCtnr = transform.Find("Root/Habitats/Ctnr/Viewport/Content").gameObject.AddComponent<UIContainer>();

            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(() => UIModule.Instance.HideUI(UIDef.HABITATS));
            UnityAction closeTips = () => _notAvailableTipsCpnt.SetActive(false);
            Button background = _notAvailableTipsCpnt.GetComponent<Button>();
            background.onClick.AddListener(closeTips);
            Button confirmBtn = _notAvailableTipsCpnt.transform.Find("Content/ConfirmBtn").GetComponent<Button>();
            confirmBtn.onClick.AddListener(closeTips);
        }

        public void OnEnable() {
            CHabitat[] confs = CHabitat.GetArray();
            int count = confs.Length;
            _habitatCtnr.SetCount<HabitatCtnrElem>(count);
            Action<CHabitat> onClicked = LoadHabitatScene;
            for (int i = 0; i < count; i++) {
                var elem = (HabitatCtnrElem) _habitatCtnr.Children[i];
                elem.SetInfo(confs[i]);
                elem.onClicked = onClicked;
            }
        }

        private void LoadHabitatScene(CHabitat conf) {
            if (conf.isAvailable) {
                string sceneName = CScene.Get(conf.sceneID).name;
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
                operation.allowSceneActivation = false;
                UIModule.Instance.HideUIAll();
                UIModule.Instance.ShowUI(UIDef.LOADING, operation);
            } else {
                _notAvailableTipsCpnt.SetActive(true);
            }
        }
    }
}
