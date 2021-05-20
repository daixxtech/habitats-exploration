using Frame.Runtime.Modules;
using Game.Config;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Views.UI.Habitats {
    public class HabitatCtnrElem : MonoBehaviour, IPointerClickHandler {
        private Image _iconImg;
        private Text _nameTxt;

        private CHabitat _conf;
        public Action<CHabitat> onClicked;

        private void Awake() {
            _iconImg = transform.Find("IconImg").GetComponent<Image>();
            _nameTxt = transform.Find("NameTxt").GetComponent<Text>();
        }

        public void SetInfo(CHabitat conf) {
            _conf = conf;
            _iconImg.sprite = AssetModule.Instance.LoadAsset<Sprite>(_conf.icon);
            _nameTxt.text = _conf.name;
        }

        public void OnPointerClick(PointerEventData eventData) {
            onClicked?.Invoke(_conf);
        }
    }
}
