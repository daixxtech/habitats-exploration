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
        public Action<CClue> onClicked;

        private void Awake() {
            _nameTxt = transform.Find("Content/NameTxt").GetComponent<Text>();
            _lockedCpnt = transform.Find("Locked").gameObject;
        }

        public void SetInfo(CClue conf) {
            _conf = conf;
            _nameTxt.text = _conf.name;
            _lockedCpnt.SetActive(!ArchiveModule.Instance.GetClueArchiveState(_conf.id));
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
