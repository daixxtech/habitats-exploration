using Game.Config;
using Game.Modules;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Views.UI.Habitats {
    public class HabitatsUIHandler : MonoBehaviour {
        public HabitatCtnrElem[] habitatCtnrElems;
        public GameObject _notAvailableTipsCpnt;

        private void Awake() {
            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(() => gameObject.SetActive(false));

            Transform archiveCtnr = transform.Find("Root/Habitats/Ctnr/Viewport/Content");
            GameObject template = archiveCtnr.Find("Template").gameObject;
            ConfHabitat[] confArr = ConfHabitat.GetArray();
            habitatCtnrElems = new HabitatCtnrElem[confArr.Length];
            Action<ConfHabitat> onClicked = LoadHabitatScene;
            for (int i = 0, count = confArr.Length; i < count; i++) {
                habitatCtnrElems[i] = Instantiate(template, archiveCtnr).AddComponent<HabitatCtnrElem>();
                habitatCtnrElems[i].clicked += onClicked;
            }
            template.SetActive(false);

            _notAvailableTipsCpnt = transform.Find("Root/NotAvailableTips").gameObject;
            UnityAction closeTips = () => _notAvailableTipsCpnt.SetActive(false);
            _notAvailableTipsCpnt.GetComponent<Button>().onClick.AddListener(closeTips);
            _notAvailableTipsCpnt.transform.Find("Content/ConfirmBtn").GetComponent<Button>().onClick.AddListener(closeTips);
        }

        private void OnEnable() {
            ConfHabitat[] confArr = ConfHabitat.GetArray();
            int length = habitatCtnrElems.Length;
            for (int i = 0; i < length; i++) {
                habitatCtnrElems[i].SetInfo(confArr[i]);
            }
        }

        private void LoadHabitatScene(ConfHabitat conf) {
            if (conf.isAvailable) {
                SceneModule.Instance.LoadSceneAsync(conf.sceneName);
            } else {
                _notAvailableTipsCpnt.SetActive(true);
            }
        }
    }
}
