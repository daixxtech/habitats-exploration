using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Modules;
using Game.Views.Scene;
using Game.Views.UI.HUD;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.HUD)]
    public class HUDUIHandler : UIHandlerBase {
        private Text _clueCountTxt;
        private UIContainer _clueCtnr;

        private Text _minimapTxt;
        private Image _minimapImg;
        private UIContainer _minimapClueCtnr;
        private Transform _scenePlayer;
        private RectTransform _minimapPlayer;
        private float _minimapScale;

        private Button _interactBtn;

        private int _clueCount;
        private CClue _clueConf;

        private void Awake() {
            _clueCountTxt = transform.Find("Root/ClueList/Title/ClueCountTxt").GetComponent<Text>();
            _clueCtnr = transform.Find("Root/ClueList/Ctnr/Viewport/Content").gameObject.AddComponent<UIContainer>();

            _minimapTxt = transform.Find("Root/Minimap/Name/Text").GetComponent<Text>();
            _minimapImg = transform.Find("Root/Minimap/Map").GetComponent<Image>();
            _minimapClueCtnr = transform.Find("Root/Minimap/Map/ClueCtnr").gameObject.AddComponent<UIContainer>();
            _minimapPlayer = transform.Find("Root/Minimap/Map/Player").GetComponent<RectTransform>();

            _interactBtn = transform.Find("Root/InteractBtn").GetComponent<Button>();
            _interactBtn.onClick.AddListener(() => Facade.Player.OnInteractedClue?.Invoke(_clueConf.id));

            transform.Find("Root/Joystick").gameObject.AddComponent<JoystickElem>();
            Button pauseBtn = transform.Find("Root/PauseBtn").GetComponent<Button>();
            pauseBtn.onClick.AddListener(() => UIModule.Instance.ShowUI(UIDef.PAUSE));
            Button jumpBtn = transform.Find("Root/JumpBtn").GetComponent<Button>();
            jumpBtn.onClick.AddListener(() => Facade.Input.OnJumpPressed?.Invoke());
        }

        public void OnEnable() {
            _scenePlayer = GameObject.FindGameObjectWithTag("Player").transform;
            _minimapScale = _minimapImg.rectTransform.sizeDelta.x / Terrain.activeTerrain.terrainData.size.x;

            CHabitat habitatConf = CHabitat.Get(GameSceneModule.Instance.GetCurSceneConf().habitatID);
            if (habitatConf != null) {
                _minimapTxt.text = habitatConf.name;
                _minimapImg.sprite = AssetModule.Instance.LoadAsset<Sprite>(habitatConf.minimapImg);
            }
            _interactBtn.gameObject.SetActive(false);

            CClue[] clueConfs = ClueModule.Instance.GetCurHabitatClueConfs();
            if (clueConfs == null) {
                _clueCtnr.SetCount<ClueCtnrElem>(0);
                _minimapClueCtnr.SetCount<Image>(0);
            } else {
                _clueCount = clueConfs.Length;
                _clueCtnr.SetCount<ClueCtnrElem>(_clueCount);
                Action<CClue> onClicked = conf => UIModule.Instance.ShowUI(UIDef.CLUE_TIPS, conf);
                for (int i = 0; i < _clueCount; i++) {
                    var elem = (ClueCtnrElem) _clueCtnr.Children[i];
                    elem.SetInfo(clueConfs[i]);
                    elem.onClicked = onClicked;
                }
                _minimapClueCtnr.SetCount<Image>(_clueCount);
                for (int i = 0; i < _clueCount; i++) {
                    var elem = _minimapClueCtnr.Children[i].GetComponent<RectTransform>();
                    elem.anchoredPosition = new Vector2(clueConfs[i].position[0], clueConfs[i].position[2]) * _minimapScale;
                }
            }
            RefreshClueCount(0);

            Facade.Player.OnTriggeredClue += OnPlayerTriggeredClue;
            Facade.Clue.OnClueUnlocked += RefreshClueCount;
        }

        private void OnDisable() {
            Facade.Player.OnTriggeredClue -= OnPlayerTriggeredClue;
            Facade.Clue.OnClueUnlocked -= RefreshClueCount;
        }

        private void LateUpdate() {
            _minimapPlayer.anchoredPosition = new Vector2(_scenePlayer.position.x, _scenePlayer.position.z) * _minimapScale;
            _minimapPlayer.eulerAngles = new Vector3(0, 0, -_scenePlayer.eulerAngles.y);
        }

        private void OnPlayerTriggeredClue(Collider other, bool enter) {
            _interactBtn.gameObject.SetActive(enter);
            _clueConf = enter ? other.GetComponent<ClueController>().Conf : null;
        }

        private void RefreshClueCount(int clueID) {
            _clueCountTxt.text = string.Format("{0} / {1}", _clueCount - ClueModule.Instance.LeftLockedClueCount, _clueCount);
        }
    }
}
