using Frame.Runtime.Modules;
using Frame.Runtime.Utils;
using Game.Config;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Views.Scene {
    public class CameraController : MonoBehaviour {
        [Header("Camera Rotation")]
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private Vector2 _rotateDegXRange;
        private Vector2 _rotateRadXRange;
        [SerializeField] [InspectorReadOnly] private float _rotateRadX;
        [SerializeField] [InspectorReadOnly] private float _rotateRadY;
#if UNITY_ANDROID
        private Touch _preTouch0, _preTouch1;
#endif

        [Header("Camera Position")]
        [SerializeField] private float _scaleSpeed;
        [SerializeField] private Vector2 _distanceRange;
        [SerializeField] [InspectorReadOnly] private float _distance;
        [SerializeField] [InspectorReadOnly] private float _actualDistance;
        [SerializeField] [InspectorReadOnly] private Vector3 _offsetPos;

        [Header("Camera Target")]
        [SerializeField] [InspectorReadOnly] private Transform _playerTrans;
        [SerializeField] private Vector3 _targetPositionOffset;

        private void Awake() {
            _playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
            _rotateSpeed = GameConfig.CameraRotateSpeed;
            _rotateDegXRange = GameConfig.CameraRotateDegXRange;
            _rotateRadXRange = GameConfig.CameraRotateDegXRange * Mathf.Deg2Rad;
            _scaleSpeed = GameConfig.CameraScaleSpeed;
            _distance = GameConfig.CameraTargetDefaultDistance;
            _distanceRange = GameConfig.CameraDistanceRange;
            _targetPositionOffset = GameConfig.CameraTargetPositionOffset;
        }

        private void Update() {
            const float DOUBLE_PI = Mathf.PI * 2;
#if UNITY_EDITOR
            if (Input.GetMouseButton(0)) {
                _rotateRadX = Mathf.Clamp(_rotateRadX - Input.GetAxis("Mouse Y") * _rotateSpeed * 20, _rotateRadXRange.x, _rotateRadXRange.y);
                _rotateRadY = (_rotateRadY - Input.GetAxis("Mouse X") * _rotateSpeed * 20) % DOUBLE_PI;
                _distance = Mathf.Clamp(_distance - Input.GetAxis("Mouse ScrollWheel") * _scaleSpeed * 100, _distanceRange.x, _distanceRange.y);
            } else {
                Time.timeScale = Mathf.Clamp(Time.timeScale + Input.GetAxis("Mouse ScrollWheel"), 0, 2);
            }
            /* Start: SceneView 的 Camera 对齐 GameView 的 Camera，需要时解除注释即可 */
            // Camera cameraMain = transform.GetComponent<Camera>();
            // var sceneView = UnityEditor.SceneView.lastActiveSceneView;
            // if (sceneView && cameraMain) {
            //     sceneView.cameraSettings.nearClip = cameraMain.nearClipPlane;
            //     sceneView.cameraSettings.fieldOfView = cameraMain.fieldOfView;
            //     sceneView.pivot = transform.position + transform.forward * sceneView.cameraDistance;
            //     sceneView.rotation = transform.rotation;
            // }
            /* End */
#endif
#if UNITY_ANDROID
            List<Touch> sceneTouches = InputModule.Instance.SceneTouches;
            // 计算 X & Y 轴的旋转角度相机与角色的距离
            if (sceneTouches.Count == 1) {
                Touch touch0 = sceneTouches[0];
                if (touch0.phase == TouchPhase.Moved) {
                    _rotateRadX = Mathf.Clamp(_rotateRadX - touch0.deltaPosition.y * _rotateSpeed, _rotateRadXRange.x, _rotateRadXRange.y);
                    _rotateRadY = (_rotateRadY - touch0.deltaPosition.x * _rotateSpeed) % DOUBLE_PI;
                }
            }
            // 计算相机与角色的距离
            else if (sceneTouches.Count >= 2) {
                Touch touch0 = sceneTouches[0], touch1 = sceneTouches[1];
                if (touch0.phase != TouchPhase.Began && touch1.phase != TouchPhase.Began) {
                    float delta = (touch0.position - touch1.position).magnitude - (_preTouch0.position - _preTouch1.position).magnitude;
                    _distance = Mathf.Clamp(_distance - delta * _scaleSpeed, _distanceRange.x, _distanceRange.y);
                }
                _preTouch0 = touch0;
                _preTouch1 = touch1;
            }
#endif
            // 计算相机与角色的实际距离（地形碰撞检测）
            Vector3 playerPos = _playerTrans.position + _targetPositionOffset;
            Ray cameraRay = new Ray(playerPos, Vector3.Normalize(transform.position - playerPos));
            bool hitGround = Physics.Raycast(cameraRay, out var hitInfo, _distance, GameConfig.GroundLayer);
            _actualDistance = hitGround ? hitInfo.distance : _distance;
            // 计算相机与角色的位置偏移
            float newRadius = _actualDistance * Mathf.Cos(_rotateRadX);
            _offsetPos.x = newRadius * Mathf.Sin(_rotateRadY);
            _offsetPos.y = _actualDistance * Mathf.Sin(_rotateRadX);
            _offsetPos.z = -newRadius * Mathf.Cos(_rotateRadY);
        }

#if UNITY_EDITOR
        private void OnGUI() {
            string text = $"<color=#FFFFFF>TimeScale: {Time.timeScale:F2}</color>";
            GUI.Label(new Rect(300, 80, 300, 100), text, new GUIStyle {fontSize = 64, fontStyle = FontStyle.Bold});
        }
#endif

        private void LateUpdate() {
            Vector3 playerPos = _playerTrans.position + _targetPositionOffset;
            transform.position = playerPos + _offsetPos;
            transform.LookAt(playerPos, Vector3.up);
        }
    }
}
