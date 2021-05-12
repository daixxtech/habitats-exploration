using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Modules;
using Game.Views.UI.Archives;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.ARCHIVE)]
    public class ArchivesUIHandler : UIHandlerBase {
        private ArchiveCtnrElem[] _archiveCtnrElems;
        private GameObject _detailsCpnt, _lockedCpnt;
        private Text _nameTxt;
        private Text _distributionTxt;
        private Image _iconImg;
        private Text _latinNameTxt;
        private Text _descriptionTxt;

        private void Awake() {
            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(() => gameObject.SetActive(false));

            _detailsCpnt = transform.Find("Root/Archive/Details").gameObject;
            _lockedCpnt = transform.Find("Root/Archive/Locked").gameObject;
            _nameTxt = transform.Find("Root/Archive/Details/Header/NameTxt").GetComponent<Text>();
            _distributionTxt = transform.Find("Root/Archive/Details/Brief/DistributionText").GetComponent<Text>();
            _iconImg = transform.Find("Root/Archive/Details/Brief/IconImg").GetComponent<Image>();
            _latinNameTxt = transform.Find("Root/Archive/Details/Content/LatinNameTxt").GetComponent<Text>();
            _descriptionTxt = transform.Find("Root/Archive/Details/Content/Description/Viewport/Content").GetComponent<Text>();

            Transform archiveCtnr = transform.Find("Root/Archive/Ctnr/Viewport/Content");
            GameObject template = archiveCtnr.Find("Template").gameObject;
            ConfAnimal[] confArr = ConfAnimal.GetArray();
            _archiveCtnrElems = new ArchiveCtnrElem[confArr.Length];
            Action<ConfAnimal> onClicked = ShowDetails;
            for (int i = 0, count = confArr.Length; i < count; i++) {
                _archiveCtnrElems[i] = Instantiate(template, archiveCtnr).AddComponent<ArchiveCtnrElem>();
                _archiveCtnrElems[i].Clicked += onClicked;
            }
            template.SetActive(false);
        }

        public void OnEnable() {
            ConfAnimal[] confArr = ConfAnimal.GetArray();
            int length = _archiveCtnrElems.Length;
            int defaultCheckedIndex = -1;
            for (int i = 0; i < length; i++) {
                _archiveCtnrElems[i].SetInfo(confArr[i]);
                if (ArchiveModule.Instance.GetArchiveState(confArr[i].id) && defaultCheckedIndex == -1) {
                    defaultCheckedIndex = i;
                }
            }
            if (defaultCheckedIndex == -1) {
                defaultCheckedIndex = 0;
            }
            _archiveCtnrElems[defaultCheckedIndex].GetComponent<Toggle>().isOn = true;
            ShowDetails(confArr[defaultCheckedIndex]);
        }

        private void ShowDetails(ConfAnimal conf) {
            bool unlocked = ArchiveModule.Instance.GetArchiveState(conf.id);
            _detailsCpnt.SetActive(unlocked);
            _lockedCpnt.SetActive(!unlocked);
            if (unlocked) {
                _nameTxt.text = conf.name;
                _distributionTxt.text = conf.distribution;
                _iconImg.sprite = AssetModule.Instance.LoadAsset<Sprite>(conf.icon);
                _latinNameTxt.text = conf.latinName;
                _descriptionTxt.text = conf.description;
            }
        }
    }
}
