using Framework.Modules;
using Framework.Modules.UI;
using Framework.Views.UI;
using Game.Config;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.CLUE_TIPS)]
    public class ClueTipsUIHandler : AUIHandler {
        private Text _nameTxt;
        private Text _descriptionTxt;

        protected override void Awake() {
            base.Awake();

            UnityAction closeTips = () => gameObject.SetActive(false);
            Button background = transform.Find("Background").GetComponent<Button>();
            background.onClick.AddListener(closeTips);
            Button closeBtn = transform.Find("Root/Header/CloseBtn").GetComponent<Button>();
            closeBtn.onClick.AddListener(closeTips);

            _nameTxt = transform.Find("Root/Tips/NameTxt").GetComponent<Text>();
            _descriptionTxt = transform.Find("Root/Tips/DescTxt").GetComponent<Text>();
        }

        protected override void OnEnable() {
            base.OnEnable();

            if (UIModule.Instance.Param is ConfClue confClue) {
                _nameTxt.text = confClue.name;
                _descriptionTxt.text = confClue.description;
            }
        }
    }
}
