using UnityEditor;
using UnityEngine;

namespace Editor {
    public static class ClueUtil {
        [MenuItem("Tools/Clues/Print Positions of Clue Samples")]
        public static void PrintCluePositions() {
            var samples = GameObject.Find("ClueSamples").GetComponentsInChildren<Transform>();
            foreach (var sample in samples) {
                var position = sample.position;
                Debug.Log(string.Format("{0},{1},{2}", position[0], position[1], position[2]));
            }
        }
    }
}
