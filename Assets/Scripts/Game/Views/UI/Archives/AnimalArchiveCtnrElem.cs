using Frame.Runtime.Modules;
using Game.Config;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Views.UI.Archives {
    public class AnimalArchiveCtnrElem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
        private static readonly Vector2 _ClickScale = new Vector2(0.975F, 0.975F);

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

        public void OnPointerDown(PointerEventData eventData) {
            transform.localScale = _ClickScale;
        }

        public void OnPointerUp(PointerEventData eventData) {
            transform.localScale = Vector2.one;
        }

        public void OnPointerClick(PointerEventData eventData) {
            onClicked?.Invoke(_conf);
        }
    }
}
