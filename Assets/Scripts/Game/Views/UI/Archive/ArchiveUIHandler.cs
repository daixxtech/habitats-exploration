using Game.Config;
using Game.Modules;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    public class ArchiveUIHandler : MonoBehaviour {
        public ArchiveCtnrElem[] archiveCtnrElems;

        public GameObject detailsCpnt;
        public GameObject lockedCpnt;
        public Text nameTxt;
        public Text distributionTxt;
        public Image iconImg;
        public Text latinNameTxt;
        public Text descriptionTxt;

        private void Awake() {
            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(() => gameObject.SetActive(false));

            detailsCpnt = transform.Find("Root/Archive/Details").gameObject;
            lockedCpnt = transform.Find("Root/Archive/Locked").gameObject;
            nameTxt = transform.Find("Root/Archive/Details/Header/NameTxt").GetComponent<Text>();
            distributionTxt = transform.Find("Root/Archive/Details/Brief/DistributionText").GetComponent<Text>();
            iconImg = transform.Find("Root/Archive/Details/Brief/IconImg").GetComponent<Image>();
            latinNameTxt = transform.Find("Root/Archive/Details/Content/LatinNameTxt").GetComponent<Text>();
            descriptionTxt = transform.Find("Root/Archive/Details/Content/Description/Viewport/Content").GetComponent<Text>();

            Transform archiveCtnr = transform.Find("Root/Archive/Ctnr/Viewport/Content");
            GameObject template = archiveCtnr.Find("Template").gameObject;
            ConfAnimal[] confArr = ConfAnimal.GetArray();
            archiveCtnrElems = new ArchiveCtnrElem[confArr.Length];
            Action<ConfAnimal> onClicked = ShowDetails;
            for (int i = 0, count = confArr.Length; i < count; i++) {
                archiveCtnrElems[i] = Instantiate(template, archiveCtnr).AddComponent<ArchiveCtnrElem>();
                archiveCtnrElems[i].clicked += onClicked;
            }
            template.SetActive(false);
        }

        private void OnEnable() {
            ConfAnimal[] confArr = ConfAnimal.GetArray();
            int length = archiveCtnrElems.Length;
            int defaultCheckedIndex = -1;
            for (int i = 0; i < length; i++) {
                archiveCtnrElems[i].SetInfo(confArr[i]);
                if (ArchiveModule.Instance.GetArchiveState(confArr[i].id) && defaultCheckedIndex == -1) {
                    defaultCheckedIndex = i;
                }
            }
            if (defaultCheckedIndex == -1) {
                defaultCheckedIndex = 0;
            }
            archiveCtnrElems[defaultCheckedIndex].GetComponent<Toggle>().isOn = true;
            ShowDetails(confArr[defaultCheckedIndex]);
        }

        private void ShowDetails(ConfAnimal conf) {
            bool unlocked = ArchiveModule.Instance.GetArchiveState(conf.id);
            detailsCpnt.SetActive(unlocked);
            lockedCpnt.SetActive(!unlocked);
            if (unlocked) {
                nameTxt.text = conf.name;
                distributionTxt.text = conf.distribution;
                iconImg.sprite = ResourceModule.Instance.LoadRes<Sprite>(conf.icon);
                latinNameTxt.text = conf.latinName;
                descriptionTxt.text = conf.description;
            }
        }
    }
}
