﻿using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.CLUE_TIPS)]
    public class ClueTipsUIHandler : UIHandlerBase {
        private Text _nameTxt;
        private Text _descriptionTxt;

        private void Awake() {
            _nameTxt = transform.Find("Root/Tips/NameTxt").GetComponent<Text>();
            _descriptionTxt = transform.Find("Root/Tips/DescTxt").GetComponent<Text>();

            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(() => {
                UIModule.Instance.HideUI(UIDef.CLUE_TIPS);
                if (ClueModule.Instance.LeftLockedClueCount <= 0) {
                    UIModule.Instance.ShowUI(UIDef.COMPLETION);
                }
            });
        }

        public void OnEnable() {
            if (UIModule.Instance.Parameter is ConfClue conf) {
                _nameTxt.text = conf.name;
                _descriptionTxt.text = conf.description;
            }
            Time.timeScale = 0;
        }

        private void OnDisable() {
            Time.timeScale = 1;
        }
    }
}
