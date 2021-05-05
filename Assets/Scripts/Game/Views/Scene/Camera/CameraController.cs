using Game.Config;
using Game.Utils;
using UnityEngine;

namespace Game.Views.Scene {
    public class CameraController : MonoBehaviour {
        [Header("Camera Rotation")]
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _rotateDegXUpperLimit;
        [SerializeField] private float _rotateDegXLowerLimit;
        private float _rotateRadXLowerLimit;
        private float _rotateRadXUpperLimit;
        [SerializeField] [InspectorReadOnly] private float _rotateRadX;
        [SerializeField] [InspectorReadOnly] private float _rotateRadY;

        [Header("Camera Position")]
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private float _distanceLowerLimit;
        [SerializeField] private float _distanceUpperLimit;
        [SerializeField] [InspectorReadOnly] private float _distance;
        [SerializeField] [InspectorReadOnly] private float _actualDistance;
        [SerializeField] [InspectorReadOnly] private Vector3 _offsetPos;

        [Header("Camera Target")]
        [SerializeField] [InspectorReadOnly] private Transform _playerTrans;
        [SerializeField] private Vector3 _playerPosOffset;

        private void Awake() {
            _playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
            _distance = GameConfig.CameraTargetDefaultDistance;
        }

        private void Start() {
            // 锁定鼠标指针
            Cursor.lockState = CursorLockMode.Locked;
            // 旋转上下限的角度转弧度
            _rotateRadXLowerLimit = _rotateDegXLowerLimit * Mathf.Deg2Rad;
            _rotateRadXUpperLimit = _rotateDegXUpperLimit * Mathf.Deg2Rad;
        }

        private void Update() {
            // 按下 Alt 解锁鼠标指针
            if (Input.GetKeyDown(KeyCode.LeftAlt)) {
                Cursor.lockState = CursorLockMode.None;
            }
            // 松开 Alt 锁定鼠标指针
            if (Input.GetKeyUp(KeyCode.LeftAlt)) {
                Cursor.lockState = CursorLockMode.Locked;
            }
            // 鼠标指针为锁定状态，则相机跟随移动
            if (Cursor.lockState == CursorLockMode.Locked) {
                // 计算 X & Y 轴的旋转角度、相机与角色的距离
                const float DOUBLE_PI = Mathf.PI * 2;
                _rotateRadX = Mathf.Clamp(_rotateRadX - Input.GetAxis("Mouse Y") * _rotateSpeed, _rotateRadXLowerLimit, _rotateRadXUpperLimit);
                _rotateRadY = (_rotateRadY - Input.GetAxis("Mouse X") * _rotateSpeed) % DOUBLE_PI;
                _distance = Mathf.Clamp(_distance - Input.GetAxis("Mouse ScrollWheel") * _scrollSpeed, _distanceLowerLimit, _distanceUpperLimit);
                // 计算相机与角色的实际距离（地形碰撞检测）
                Vector3 playerPos = _playerTrans.position + _playerPosOffset;
                Ray cameraRay = new Ray(playerPos, Vector3.Normalize(transform.position - playerPos));
                _actualDistance = Physics.Raycast(cameraRay, out var hitInfo, _distance, GameConfig.GroundLayer) ? hitInfo.distance : _distance;
                // 计算相机与角色的位置偏移
                float newRadius = _actualDistance * Mathf.Cos(_rotateRadX);
                _offsetPos.x = newRadius * Mathf.Sin(_rotateRadY);
                _offsetPos.y = _actualDistance * Mathf.Sin(_rotateRadX);
                _offsetPos.z = -newRadius * Mathf.Cos(_rotateRadY);
            }
        }

        private void LateUpdate() {
            Vector3 playerPos = _playerTrans.position + _playerPosOffset;
            transform.position = playerPos + _offsetPos;
            transform.LookAt(playerPos, Vector3.up);
        }

        private void OnDestroy() {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
