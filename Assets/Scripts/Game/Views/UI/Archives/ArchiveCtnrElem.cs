using Frame.Runtime.Modules;
using Game.Config;
using Game.Modules;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Views.UI.Archives {
    public class ArchiveCtnrElem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
        private Image _iconImg;
        private Text _nameTxt;
        private GameObject _lockedCpnt;

        private ConfAnimal _conf;
        private bool _pointerEnter;
        private bool _pointerDown;
        public event Action<ConfAnimal> Clicked;

        private void Awake() {
            _iconImg = transform.Find("Icon/IconImg").GetComponent<Image>();
            _nameTxt = transform.Find("Name/NameTxt").GetComponent<Text>();
            _lockedCpnt = transform.Find("Locked").gameObject;
        }

        private void Update() {
            if (_pointerDown) {
                transform.localScale = Vector2.one;
            } else {
                float scale = transform.localScale.x;
                if (_pointerEnter && scale < 1.1F) {
                    scale = Mathf.Clamp(scale + 0.005F, 1.0F, 1.05F);
                    transform.localScale = new Vector2(scale, scale);
                } else if (!_pointerEnter && scale > 1.0F) {
                    scale = Mathf.Clamp(scale - 0.005F, 1.0F, 1.05F);
                    transform.localScale = new Vector2(scale, scale);
                }
            }
        }

        public void SetInfo(ConfAnimal conf) {
            _conf = conf;
            _iconImg.sprite = ResourceModule.Instance.LoadRes<Sprite>(_conf.icon);
            _nameTxt.text = _conf.name;
            _lockedCpnt.SetActive(!ArchiveModule.Instance.GetArchiveState(_conf.id));
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _pointerEnter = true;
        }

        public void OnPointerExit(PointerEventData eventData) {
            _pointerEnter = false;
        }

        public void OnPointerDown(PointerEventData eventData) {
            _pointerDown = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            _pointerDown = false;
        }

        public void OnPointerClick(PointerEventData eventData) {
            Clicked?.Invoke(_conf);
        }
    }
}
