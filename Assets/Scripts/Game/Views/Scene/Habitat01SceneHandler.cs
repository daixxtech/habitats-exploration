using Frame.Runtime.Modules;
using Frame.Runtime.Modules.Scenes;
using Game.Config;
using Game.Modules;
using Game.Views.UI;
using UnityEngine;

namespace Game.Views.Scene {
    [SceneBind(SceneDef.HABITAT_01)]
    public class Habitat01SceneHandler : MonoBehaviour {
        private void Start() {
            UIModule.Instance.HideUIAll();
            UIModule.Instance.ShowUI(UIDef.HUD);

            Transform clueRoot = transform.Find("Environment/Clues");
            var clueConfs = ClueModule.Instance.GetCurHabitatClueConfs();
            int clueCount = clueConfs.Length;
            GameObject cluePrefab = AssetModule.Instance.LoadAsset<GameObject>("Scene_Clue.prefab");
            for (int i = 0; i < clueCount; i++) {
                GameObject clueInstance = Instantiate(cluePrefab, clueRoot);
                clueInstance.transform.localPosition = new Vector3(clueConfs[i].position[0], clueConfs[i].position[1], clueConfs[i].position[2]);
                ClueController clueController = clueInstance.AddComponent<ClueController>();
                clueController.SetInfo(clueConfs[i]);
            }
        }

        private void OnEnable() {
            Facade.Player.OnInteractedClue += ShowClueTips;
        }

        private void OnDisable() {
            Facade.Player.OnInteractedClue -= ShowClueTips;
        }

        private static void ShowClueTips(int clueID) {
            UIModule.Instance.ShowUI(UIDef.CLUE_TIPS, CClue.Get(clueID));
        }
    }
}
