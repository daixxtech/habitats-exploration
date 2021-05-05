using Game.Config;
using Game.Modules;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Views.UI {
    public class ClueTipsUIHandler : MonoBehaviour {
        private Text _nameTxt;
        private Text _descTxt;

        private void Awake() {
            UnityAction closeTips = () => gameObject.SetActive(false);
            transform.Find("Background").GetComponent<Button>().onClick.AddListener(closeTips);
            transform.Find("Root/Header/CloseBtn").GetComponent<Button>().onClick.AddListener(closeTips);

            _nameTxt = transform.Find("Root/Tips/NameTxt").GetComponent<Text>();
            _descTxt = transform.Find("Root/Tips/DescTxt").GetComponent<Text>();
        }

        private void OnEnable() {
            Cursor.lockState = CursorLockMode.None;
            if (UIModule.Instance.Param is ConfClue confClue) {
                _nameTxt.text = confClue.name;
                _descTxt.text = confClue.description;
            }
        }

        private void OnDisable() {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
