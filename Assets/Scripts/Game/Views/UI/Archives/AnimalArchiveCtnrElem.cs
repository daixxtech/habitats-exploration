using Frame.Runtime.Modules;
using Game.Config;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Views.UI.Archives {
    public class AnimalArchiveCtnrElem : MonoBehaviour, IPointerClickHandler {
        private Image _image;
        private Text _nameTxt;

        private CAnimal _conf;
        public Action<CAnimal> onClicked;

        private void Awake() {
            _image = transform.Find("Content/Image").GetComponent<Image>();
            _nameTxt = transform.Find("Content/NameTxt").GetComponent<Text>();
        }

        public void SetInfo(CAnimal conf) {
            _conf = conf;
            _image.sprite = AssetModule.Instance.LoadAsset<Sprite>(_conf.image);
            _nameTxt.text = _conf.name;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
            onClicked?.Invoke(_conf);
        }
    }
}
