using Frame.Runtime.Utils;
using Game.Config;
using UnityEngine;

namespace Game.Views.Scene {
    [RequireComponent(typeof(CharacterController3D))]
    public class PlayerController : MonoBehaviour {
        [Header("Player Input")]
        [SerializeField] [InspectorReadOnly] private float _inputV;
        [SerializeField] [InspectorReadOnly] private float _inputH;
        [SerializeField] [InspectorReadOnly] private bool _jumpPressed;

        [Header("Player State")]
        [SerializeField] [InspectorReadOnly] private bool _animatorJumping;

        [Header("Player Control")]
        [SerializeField] [InspectorReadOnly] private CharacterController3D _controller;
        [SerializeField] [InspectorReadOnly] private Animator _animator;
        [SerializeField] [InspectorReadOnly] private Transform _cameraTrans;

        private void Awake() {
            _controller = gameObject.GetComponent<CharacterController3D>();
            _controller.MoveSpeed = GameConfig.PlayerMoveSpeed;
            _controller.JumpForce = GameConfig.PlayerJumpForce;
            _controller.JumpCount = GameConfig.PlayerJumpCount;
            _animator = GetComponent<Animator>();
            _cameraTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        private void OnEnable() {
            Facade.Input.OnJoystickDragged += OnJoystickDragged;
            Facade.Input.OnJumpPressed += OnJumpPressed;
        }

        private void OnDisable() {
            Facade.Input.OnJoystickDragged -= OnJoystickDragged;
            Facade.Input.OnJumpPressed -= OnJumpPressed;
        }

#if UNITY_EDITOR
        private void Update() {
            if (Input.GetButtonDown("Jump")) {
                _jumpPressed = true;
            }
        }

#endif

        private void FixedUpdate() {
            // 动画状态机是否处于跳跃状态（用于针对跳跃动画做额外处理）
            _animatorJumping = _animator.GetCurrentAnimatorStateInfo(0).IsName("JUMP00");
            // 更新动画控制器状态
            UpdateAnimatorState();
            // 更新角色控制器状态
            UpdateControllerState();
            // 重置跳跃输入
            _jumpPressed = false;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Clue")) {
                Facade.Player.OnTriggeredClue?.Invoke(other, true);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Clue")) {
                Facade.Player.OnTriggeredClue?.Invoke(other, false);
            }
        }

        private void OnJoystickDragged(Vector2 direction) {
            _inputV = direction.y;
            _inputH = direction.x;
        }

        private void OnJumpPressed() {
            _jumpPressed = true;
        }

        /// <summary> 更新动画控制器状态 </summary>
        private void UpdateAnimatorState() {
            if (_jumpPressed && _controller.CanJump && !_animatorJumping && (_controller.IsGrounded || _controller.IsFalling) /* 防止多段跳时多次触发 Jump 动画 */) {
                _animator.SetTrigger("Jump"); // 放在 Update 里会导致重复触发
            }
            if (_controller.IsGrounded) {
                _animator.SetFloat("InputV", Mathf.Abs(_inputV));
                _animator.SetFloat("InputH", Mathf.Abs(_inputH));
            }
        }

        /// <summary> 更新角色控制器状态 </summary>
        private void UpdateControllerState() {
            // 计算角色移动方向
            Vector3 forward = _cameraTrans.forward;
            forward.y = 0;
            Vector3 direction = (forward.normalized * _inputV + _cameraTrans.right * _inputH).normalized;
            // 输入都为 0 也要调用，保证无摩擦力的情况下，主角平地不会滑动
            _controller.Move(direction);
            // 输入都为 0 不要调用，保证无输入的情况下，主角朝向不会变化
            if (_inputV != 0 || _inputH != 0) {
                _controller.Direction = direction;
            }
            // 检测 Update 中是否有跳跃输入
            if (_jumpPressed && !_animatorJumping) {
                _controller.Jump();
            }
        }
    }
}
