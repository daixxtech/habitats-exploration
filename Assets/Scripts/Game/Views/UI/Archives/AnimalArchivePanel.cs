using Frame.Runtime.Modules;
using Frame.Runtime.Views.UI;
using Game.Config;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI.Archives {
    public class AnimalArchivePanel : MonoBehaviour {
        private UIContainer _animalArchiveCtnr;
        private Text _nameTxt;
        private Text _protectionLevelTxt, _distributionTxt;
        private Image _image;
        private Text _latinNameTxt, _descriptionTxt;
        private ScrollRect _descriptionScrollRect;

        private void Awake() {
            _animalArchiveCtnr = transform.Find("List/Ctnr/Viewport/Content").gameObject.AddComponent<UIContainer>();
            _nameTxt = transform.Find("Info/Header/NameTxt").GetComponent<Text>();
            _protectionLevelTxt = transform.Find("Info/Brief/ProtectionLevelTxt").GetComponent<Text>();
            _distributionTxt = transform.Find("Info/Brief/DistributionTxt").GetComponent<Text>();
            _image = transform.Find("Info/Brief/Image").GetComponent<Image>();
            _latinNameTxt = transform.Find("Info/Details/LatinNameTxt").GetComponent<Text>();
            _descriptionTxt = transform.Find("Info/Details/Description/Viewport/Content").GetComponent<Text>();
            _descriptionScrollRect = transform.Find("Info/Details/Description").GetComponent<ScrollRect>();
        }

        public void Show() {
            gameObject.SetActive(true);
            var animalConfs = CAnimal.GetArray();
            int animalCount = animalConfs.Length;
            _animalArchiveCtnr.SetCount<AnimalArchiveCtnrElem>(animalCount);
            Action<CAnimal> onClicked = ShowInfo;
            for (int i = 0; i < animalCount; i++) {
                var elem = (AnimalArchiveCtnrElem) _animalArchiveCtnr.Children[i];
                elem.SetInfo(animalConfs[i]);
                elem.onClicked = onClicked;
            }
            _animalArchiveCtnr.Children[0].GetComponent<Toggle>().isOn = true;
            ShowInfo(animalConfs[0]);
        }

        private void ShowInfo(CAnimal conf) {
            _nameTxt.text = conf.name;
            _protectionLevelTxt.text = conf.protectionLevel;
            _distributionTxt.text = conf.distribution;
            _image.sprite = AssetModule.Instance.LoadAsset<Sprite>(conf.image);
            _latinNameTxt.text = conf.latinName;
            _descriptionTxt.text = conf.description;
            _descriptionScrollRect.verticalNormalizedPosition = 1;
        }
    }
}
