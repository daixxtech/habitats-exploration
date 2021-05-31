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
        private bool _pointerDown;
        public Action<CAnimal> onClicked;

        private void Awake() {
            _image = transform.Find("Content/Image").GetComponent<Image>();
            _nameTxt = transform.Find("Content/NameTxt").GetComponent<Text>();
        }

        private void Update() {
            transform.localScale = _pointerDown ? (Vector3) _ClickScale : Vector3.one;
        }

        public void SetInfo(CAnimal conf) {
            _conf = conf;
            _image.sprite = AssetModule.Instance.LoadAsset<Sprite>(_conf.image);
            _nameTxt.text = _conf.name;
        }

        public void OnPointerDown(PointerEventData eventData) {
            _pointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            _pointerDown = false;
        }

        public void OnPointerClick(PointerEventData eventData) {
            onClicked?.Invoke(_conf);
        }
    }
}
