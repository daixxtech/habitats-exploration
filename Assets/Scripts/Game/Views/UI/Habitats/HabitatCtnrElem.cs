﻿using Frame.Runtime.Modules;
using Game.Config;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Views.UI.Habitats {
    public class HabitatCtnrElem : MonoBehaviour, IPointerClickHandler {
        private Image _iconImg;
        private Text _nameTxt;

        private ConfHabitat _conf;
        public Action<ConfHabitat> onClicked;

        public void SetInfo(ConfHabitat conf) {
            _conf = conf;
            _iconImg.sprite = AssetModule.Instance.LoadAsset<Sprite>(_conf.icon);
            _nameTxt.text = _conf.name;
        }

        private void Awake() {
            _iconImg = transform.Find("IconImg").GetComponent<Image>();
            _nameTxt = transform.Find("NameTxt").GetComponent<Text>();
        }

        public void OnPointerClick(PointerEventData eventData) {
            onClicked?.Invoke(_conf);
        }
    }
}
