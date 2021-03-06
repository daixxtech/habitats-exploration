using Frame.Runtime.Utils;
using Game.Config;
using UnityEngine;

namespace Game.Views.Scene {
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class CharacterController3D : MonoBehaviour {
        [Header("Character Collision")]
        private static readonly Collider[] Colliders = new Collider[1]; // 碰撞检测数组（用于 OverlapCapsuleNonAlloc 函数）
        [SerializeField] [InspectorReadOnly] private Rigidbody _rig3D;
        [SerializeField] [InspectorReadOnly] private CapsuleCollider _collider;

        [Header("Character Moving")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] [InspectorReadOnly] private Vector3 _preVelocity; // 上次计算的速度（用于 SmoothDamp 函数）
#if UNITY_EDITOR
        [SerializeField] [InspectorReadOnly] private Vector3 _curVelocity; // 当前计算的速度（用于在 Inspector 中进行观测，对实际代码无影响，可删除）
#endif
        [SerializeField] [InspectorReadOnly] private Vector3 _direction;

        [Header("Character Jumping")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private int _jumpCount;
        [SerializeField] [InspectorReadOnly] private int _remainingJumpCount; // 剩余可跳跃次数（用于支持多段跳）
        [SerializeField] [InspectorReadOnly] private bool _preIsGrounded; // 上次检测时是否落地
        [SerializeField] [InspectorReadOnly] private bool _isGrounded; // 是否落地
        [SerializeField] [InspectorReadOnly] private bool _isFalling; // 是否处于下落状态

#region 对外接口
        /// <summary> 移动速度 </summary>
        public float MoveSpeed { get => _moveSpeed; internal set => _moveSpeed = value; }
        /// <summary> 跳跃力度 </summary>
        public float JumpForce { get => _jumpForce; internal set => _jumpForce = value; }
        /// <summary> 最大可跳跃次数 </summary>
        public int JumpCount { get => _jumpCount; internal set => _jumpCount = value; }
        /// <summary> 是否落地 </summary>
        public bool IsGrounded => _isGrounded;
        /// <summary> 是否处于下落状态 </summary>
        public bool IsFalling => _isFalling;
        /// <summary> 是否可跳跃 </summary>
        public bool CanJump => _remainingJumpCount > 0;
        /// <summary> 人物朝向 </summary>
        public Vector3 Direction { get => _direction; set => _direction = value; }

        /// <summary> 移动 </summary>
        public void Move(Vector3 direction) {
            if (!_isGrounded) {
                return;
            }
            // 计算角色的速度 
            Vector3 targetVelocity = new Vector3(direction.x * _moveSpeed, _rig3D.velocity.y, direction.z * _moveSpeed);
            _rig3D.velocity = Vector3.SmoothDamp(_rig3D.velocity, targetVelocity, ref _preVelocity, 0.1F, float.PositiveInfinity, Time.fixedDeltaTime);
        }

        /// <summary> 跳跃 </summary>
        public void Jump() {
            // 剩余可跳跃次数大于 0 
            if (_remainingJumpCount > 0) {
                --_remainingJumpCount;
                _rig3D.velocity = new Vector3(_rig3D.velocity.x, _jumpForce, _rig3D.velocity.z);
            }
        }
#endregion

        private void Awake() {
            _rig3D = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        }

        private void Start() {
            _remainingJumpCount = _jumpCount;
        }

#if UNITY_EDITOR
        private void Update() {
            _curVelocity = _rig3D.velocity; // 更新当前速度，用于观测
        }
#endif

        private void FixedUpdate() {
            Rotate();
            CheckGrounded();
            CheckFalling();
            CorrectRemainingJumpCount();
        }

        /// <summary> 转向 </summary>
        private void Rotate() {
            float rotateAngle = Quaternion.FromToRotation(transform.forward, _direction).eulerAngles.y;
            if (rotateAngle == 0) {
                return;
            }
            if (rotateAngle > 180) {
                rotateAngle -= 360;
            }
            const int ROTATE_THRESHOLD = 10;
            if (Mathf.Abs(rotateAngle) > ROTATE_THRESHOLD) {
                rotateAngle = rotateAngle > 0 ? ROTATE_THRESHOLD : -ROTATE_THRESHOLD;
            }
            transform.Rotate(transform.up, rotateAngle);
        }

        /// <summary> 检查人物是否落地 </summary>
        private void CheckGrounded() {
            _preIsGrounded = _isGrounded; // 更新上次检测时是否落地
            _isGrounded = false;
            Vector3 position = transform.position;
            const float COLLISION_DETECTION_RADIUS = 0.04F;
            Vector3 pointTop = position + new Vector3(0, _collider.height - _collider.radius, 0);
            Vector3 pointBtm = position + new Vector3(0, _collider.radius - COLLISION_DETECTION_RADIUS, 0);
            int colliderCount = Physics.OverlapCapsuleNonAlloc(pointTop, pointBtm, _collider.radius, Colliders, GameConfig.GroundLayer);
            _isGrounded = colliderCount > 0;
            _rig3D.useGravity = !_isGrounded;
        }

        /// <summary> 检查人物是否处于下落状态 </summary>
        private void CheckFalling() {
            // 人物在空中且速度大于 -0.1F 则说明处于下落状态
            _isFalling = !_isGrounded && _rig3D.velocity.y < -0.1F;
        }

        /// <summary> 修正剩余跳跃次数 </summary>
        private void CorrectRemainingJumpCount() {
            // 落地：重置可跳跃次数为总跳跃次数 
            if (_isGrounded) {
                _remainingJumpCount = _jumpCount;
            }
            // 未落地但上次检测时落地：重置可跳跃次数为总跳跃次数 - 1 
            else if (_preIsGrounded) {
                _remainingJumpCount = _jumpCount - 1;
            }
        }
    }
}
