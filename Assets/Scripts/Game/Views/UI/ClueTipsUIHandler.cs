using FrameworkRuntime.Modules;
using FrameworkRuntime.Modules.UI;
using FrameworkRuntime.Views.UI;
using Game.Config;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.CLUE_TIPS)]
    public class ClueTipsUIHandler : AUIHandler {
        private Text _nameTxt;
        private Text _descriptionTxt;

        private void Awake() {
            UnityAction closeTips = () => gameObject.SetActive(false);
            Button background = transform.Find("Background").GetComponent<Button>();
            background.onClick.AddListener(closeTips);
            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(closeTips);

            _nameTxt = transform.Find("Root/Tips/NameTxt").GetComponent<Text>();
            _descriptionTxt = transform.Find("Root/Tips/DescTxt").GetComponent<Text>();
        }

        public void OnEnable() {
            if (UIModule.Instance.Parameter is ConfClue confClue) {
                _nameTxt.text = confClue.name;
                _descriptionTxt.text = confClue.description;
            }
        }
    }
}
