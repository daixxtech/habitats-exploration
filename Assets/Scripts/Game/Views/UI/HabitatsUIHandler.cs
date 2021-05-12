using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Modules;
using Game.Views.UI.Habitats;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.HABITATS)]
    public class HabitatsUIHandler : UIHandlerBase {
        private HabitatCtnrElem[] _habitatCtnrElems;
        private GameObject _notAvailableTipsCpnt;

        private void Awake() {
            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(() => gameObject.SetActive(false));

            Transform archiveCtnr = transform.Find("Root/Habitats/Ctnr/Viewport/Content");
            GameObject template = archiveCtnr.Find("Template").gameObject;
            ConfHabitat[] confArr = ConfHabitat.GetArray();
            _habitatCtnrElems = new HabitatCtnrElem[confArr.Length];
            Action<ConfHabitat> onClicked = LoadHabitatScene;
            for (int i = 0, count = confArr.Length; i < count; i++) {
                _habitatCtnrElems[i] = Instantiate(template, archiveCtnr).AddComponent<HabitatCtnrElem>();
                _habitatCtnrElems[i].Clicked += onClicked;
            }
            template.SetActive(false);

            _notAvailableTipsCpnt = transform.Find("Root/NotAvailableTips").gameObject;
            UnityAction closeTips = () => _notAvailableTipsCpnt.SetActive(false);
            _notAvailableTipsCpnt.GetComponent<Button>().onClick.AddListener(closeTips);
            _notAvailableTipsCpnt.transform.Find("Content/ConfirmBtn").GetComponent<Button>().onClick.AddListener(closeTips);
        }

        public void OnEnable() {
            ConfHabitat[] confArr = ConfHabitat.GetArray();
            int length = _habitatCtnrElems.Length;
            for (int i = 0; i < length; i++) {
                _habitatCtnrElems[i].SetInfo(confArr[i]);
            }
        }

        private void LoadHabitatScene(ConfHabitat conf) {
            if (conf.isAvailable) {
                SceneModule.Instance.LoadSceneAsync(conf.sceneID);
            } else {
                _notAvailableTipsCpnt.SetActive(true);
            }
        }
    }
}
