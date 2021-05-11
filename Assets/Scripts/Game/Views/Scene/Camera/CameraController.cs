using Game.Config;
using Game.Modules;
using Game.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        private Touch _preTouch0;
        private Touch _preTouch1;

        [Header("Camera Position")]
        [SerializeField] private float _scaleSpeed;
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
            // 旋转上下限的角度转弧度
            _rotateRadXLowerLimit = _rotateDegXLowerLimit * Mathf.Deg2Rad;
            _rotateRadXUpperLimit = _rotateDegXUpperLimit * Mathf.Deg2Rad;
        }

        private void Update() {
            List<Touch> sceneTouches = InputModule.Instance.SceneTouches;
            // 计算 X & Y 轴的旋转角度相机与角色的距离
            if (sceneTouches.Count == 1) {
                Touch touch0 = sceneTouches[0];
                if (touch0.phase == TouchPhase.Moved) {
                    const float DOUBLE_PI = Mathf.PI * 2;
                    _rotateRadX = Mathf.Clamp(_rotateRadX - touch0.deltaPosition.y * _rotateSpeed, _rotateRadXLowerLimit, _rotateRadXUpperLimit);
                    _rotateRadY = (_rotateRadY - touch0.deltaPosition.x * _rotateSpeed) % DOUBLE_PI;
                }
            }
            // 计算相机与角色的距离
            else if (sceneTouches.Count >= 2) {
                Touch touch0 = sceneTouches[0], touch1 = sceneTouches[1];
                if (touch0.phase != TouchPhase.Began && touch1.phase != TouchPhase.Began) {
                    float delta = (touch0.position - touch1.position).magnitude - (_preTouch0.position - _preTouch1.position).magnitude;
                    _distance = Mathf.Clamp(_distance - delta * _scaleSpeed, _distanceLowerLimit, _distanceUpperLimit);
                }
                _preTouch0 = touch0;
                _preTouch1 = touch1;
            }
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

        private void LateUpdate() {
            Vector3 playerPos = _playerTrans.position + _playerPosOffset;
            transform.position = playerPos + _offsetPos;
            transform.LookAt(playerPos, Vector3.up);
        }
    }
}
