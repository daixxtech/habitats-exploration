using Game.Config;
using Game.Modules;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Views.UI.Habitats {
    public class HabitatCtnrElem : MonoBehaviour, IPointerClickHandler {
        private Image _iconImg;
        private Text _nameTxt;

        private ConfHabitat _conf;
        public event Action<ConfHabitat> clicked;

        public void SetInfo(ConfHabitat conf) {
            _conf = conf;
            _iconImg.sprite = ResourceModule.Instance.LoadRes<Sprite>(_conf.icon);
            _nameTxt.text = _conf.name;
        }

        private void Awake() {
            _iconImg = transform.Find("IconImg").GetComponent<Image>();
            _nameTxt = transform.Find("NameTxt").GetComponent<Text>();
        }

        public void OnPointerClick(PointerEventData eventData) {
            clicked?.Invoke(_conf);
        }
    }
}
