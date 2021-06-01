using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor {
    public static class CameraUtil {
        private const string HABITAT_SCENE_NAME_PREFIX = "Habitat_";

        [MenuItem("Tools/Camera/Capture Minimap Image")]
        public static void CaptureMinimapImage() {
            Scene scene = SceneManager.GetActiveScene();
            if (!scene.name.StartsWith(HABITAT_SCENE_NAME_PREFIX)) {
                Debug.LogError("[MinimapUtil] CaptureMinimapImage: Current active scene is not a habitat.");
                return;
            }

            Transform lightTrans = GameObject.Find("DirectionalLight").transform;
            Vector3 lightAngles = lightTrans.eulerAngles;
            lightTrans.eulerAngles = new Vector3(95, 0, 0);

            Transform cameraTrans = new GameObject().transform;
            cameraTrans.parent = GameObject.Find("Scene").transform;
            cameraTrans.position = new Vector3(0, 64, 0);
            cameraTrans.eulerAngles = new Vector3(90, 0, 0);

            Camera camera = cameraTrans.gameObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 64;

            RenderTexture targetTex = RenderTexture.GetTemporary(512, 512, 8);
            camera.targetTexture = targetTex;
            camera.Render();
            RenderTexture.active = targetTex;
            Texture2D minimapTex = new Texture2D(512, 512);
            minimapTex.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
            minimapTex.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(targetTex);

            Object.DestroyImmediate(camera.gameObject);
            lightTrans.eulerAngles = lightAngles;

            string directory = Path.Combine(Application.dataPath, "Res/Textures/Minimap");
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            string path = Path.Combine(directory, $"tex_minimap_{scene.name.Replace(HABITAT_SCENE_NAME_PREFIX, string.Empty).ToLower()}.png");
            File.WriteAllBytes(path, minimapTex.EncodeToPNG());
            EditorUtility.RevealInFinder(path);
        }
    }
}
