using System.Text;
using UnityEditor;
using UnityEngine;

namespace Frame.Editor.Utils {
    public static class UIUtil {
        private static readonly StringBuilder Builder = new StringBuilder();

        [MenuItem("Tools/UI/Copy UI Path %#X")]
        public static void CopyUITransformPath() {
            Transform cur = Selection.activeTransform;
            if (cur == null) {
                return;
            }
            Builder.Clear().Insert(0, cur.name);
            while (true) {
                Transform parent = cur.parent;
                if (parent == null || parent.GetComponent<Canvas>() != null || parent.name == "Template") {
                    break;
                }
                Builder.Insert(0, "/").Insert(0, parent.name);
                cur = parent;
            }
            string path = Builder.ToString();
            Debug.Log($"[UI Path] {path}");
            GUIUtility.systemCopyBuffer = path;
        }
    }
}
