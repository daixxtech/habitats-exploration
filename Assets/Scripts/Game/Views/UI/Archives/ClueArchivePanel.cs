using Frame.Runtime.Modules;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Modules;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI.Archives {
    public class ClueArchivePanel : MonoBehaviour {
        private GameObject _infoCpnt, _lockedCpnt;
        private UIContainer _clueArchiveCtnr;
        private Text _nameTxt;
        private Text _animalNameTxt, _habitatNameTxt;
        private Image _image;
        private Text _descriptionTxt;

        private void Awake() {
            _infoCpnt = transform.Find("Info").gameObject;
            _lockedCpnt = transform.Find("Locked").gameObject;
            _clueArchiveCtnr = transform.Find("List/Ctnr/Viewport/Content").gameObject.AddComponent<UIContainer>();
            _nameTxt = transform.Find("Info/Header/NameTxt").GetComponent<Text>();
            _animalNameTxt = transform.Find("Info/Brief/AnimalNameTxt").GetComponent<Text>();
            _habitatNameTxt = transform.Find("Info/Brief/HabitatNameTxt").GetComponent<Text>();
            _image = transform.Find("Info/Brief/Image").GetComponent<Image>();
            _descriptionTxt = transform.Find("Info/Details/Description/Viewport/Content").GetComponent<Text>();
        }

        public void Show() {
            gameObject.SetActive(true);
            var clueConfs = CClue.GetArray();
            int clueCount = clueConfs.Length;
            _clueArchiveCtnr.SetCount<ClueArchiveCtnrElem>(clueCount);
            Action<CClue> onClicked = ShowInfo;
            for (int i = 0; i < clueCount; i++) {
                var elem = (ClueArchiveCtnrElem) _clueArchiveCtnr.Children[i];
                elem.SetInfo(clueConfs[i]);
                elem.onClicked = onClicked;
            }
            _clueArchiveCtnr.Children[0].GetComponent<Toggle>().isOn = true;
            ShowInfo(clueConfs[0]);
        }

        private void ShowInfo(CClue conf) {
            bool unlocked = ArchiveModule.Instance.GetClueArchiveState(conf.id);
            _infoCpnt.SetActive(unlocked);
            _lockedCpnt.SetActive(!unlocked);
            if (unlocked) {
                _nameTxt.text = conf.name;
                _animalNameTxt.text = CAnimal.Get(conf.animalID).name;
                _habitatNameTxt.text = CHabitat.Get(conf.habitatID).name;
                _image.sprite = AssetModule.Instance.LoadAsset<Sprite>(conf.image);
                _descriptionTxt.text = conf.description;
            }
        }
    }
}
