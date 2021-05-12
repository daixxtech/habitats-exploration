using FrameworkRuntime.Modules;
using FrameworkRuntime.Modules.UI;
using FrameworkRuntime.Views.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.START)]
    public class StartUIHandler : AUIHandler {
        private GameObject _guideCpnt;

        private void Awake() {
            Button startBtn = transform.Find("Root/Options/StartBtn").GetComponent<Button>();
            startBtn.onClick.AddListener(() => UIModule.Instance.ShowUI(UIDef.HABITATS));
            Button guideBtn = transform.Find("Root/Options/GuideBtn").GetComponent<Button>();
            guideBtn.onClick.AddListener(() => { _guideCpnt.SetActive(true); });
            Button guideCloseBtn = transform.Find("Root/Guide/Root/Header/CloseBtn").GetComponent<Button>();
            guideCloseBtn.onClick.AddListener(() => _guideCpnt.SetActive(false));
            Button archiveBtn = transform.Find("Root/Options/ArchiveBtn").GetComponent<Button>();
            archiveBtn.onClick.AddListener(() => UIModule.Instance.ShowUI(UIDef.ARCHIVE));
            Button quitBtn = transform.Find("Root/Options/QuitBtn").GetComponent<Button>();
            quitBtn.onClick.AddListener(Application.Quit);

            _guideCpnt = transform.Find("Root/Guide").gameObject;
        }
    }
}
