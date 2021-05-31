using Game.Config;
using Game.Modules;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Views.UI.Archives {
    public class ClueArchiveCtnrElem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {
        private static readonly Vector2 _ClickScale = new Vector2(0.975F, 0.975F);

        private Text _nameTxt;
        private GameObject _lockedCpnt;

        private CClue _conf;
        private bool _pointerDown;
        public Action<CClue> onClicked;

        private void Awake() {
            _nameTxt = transform.Find("Content/NameTxt").GetComponent<Text>();
            _lockedCpnt = transform.Find("Locked").gameObject;
        }

        private void Update() {
            transform.localScale = _pointerDown ? (Vector3) _ClickScale : Vector3.one;
        }

        public void SetInfo(CClue conf) {
            _conf = conf;
            _nameTxt.text = _conf.name;
            _lockedCpnt.SetActive(!ArchiveModule.Instance.GetClueArchiveState(_conf.id));
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
