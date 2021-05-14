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
        private UIContainer _archiveCtnr;
        private GameObject _detailsCpnt, _lockedCpnt;
        private Text _nameTxt, _distributionTxt;
        private Image _iconImg;
        private Text _latinNameTxt, _descriptionTxt;

        private void Awake() {
            _detailsCpnt = transform.Find("Root/Archive/Details").gameObject;
            _lockedCpnt = transform.Find("Root/Archive/Locked").gameObject;
            _nameTxt = transform.Find("Root/Archive/Details/Header/NameTxt").GetComponent<Text>();
            _distributionTxt = transform.Find("Root/Archive/Details/Brief/DistributionText").GetComponent<Text>();
            _iconImg = transform.Find("Root/Archive/Details/Brief/IconImg").GetComponent<Image>();
            _latinNameTxt = transform.Find("Root/Archive/Details/Content/LatinNameTxt").GetComponent<Text>();
            _descriptionTxt = transform.Find("Root/Archive/Details/Content/Description/Viewport/Content").GetComponent<Text>();
            _archiveCtnr = transform.Find("Root/Archive/Ctnr/Viewport/Content").gameObject.AddComponent<UIContainer>();

            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(() => gameObject.SetActive(false));
        }

        public void OnEnable() {
            ConfAnimal[] confs = ConfAnimal.GetArray();
            int count = confs.Length;
            _archiveCtnr.SetCount<ArchiveCtnrElem>(count);
            Action<ConfAnimal> onClicked = ShowDetails;
            for (int i = 0; i < count; i++) {
                var elem = (ArchiveCtnrElem) _archiveCtnr.Children[i];
                elem.SetInfo(confs[i]);
                elem.onClicked = onClicked;
            }
            _archiveCtnr.Children[0].GetComponent<Toggle>().isOn = true;
            ShowDetails(confs[0]);
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
