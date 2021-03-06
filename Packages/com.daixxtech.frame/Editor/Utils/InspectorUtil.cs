using Frame.Runtime.Utils;
using UnityEditor;
using UnityEngine;

namespace Frame.Editor.Utils {
    [CustomPropertyDrawer(typeof(InspectorReadOnlyAttribute))]
    public class InspectorReadOnlyDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUI.enabled = false; // 切换状态，用于绘制不可交互的属性
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true; // 恢复状态
        }
    }
}
