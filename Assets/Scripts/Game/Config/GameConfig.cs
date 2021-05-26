using UnityEngine;

namespace Game.Config {
    public static class GameConfig {
        public static readonly LayerMask GroundLayer = LayerMask.GetMask("Ground");

        public static readonly float JoystickDragRadius = 200;
        
        public static readonly float CameraRotateSpeed = 0.005F;
        public static readonly Vector2 CameraRotateDegXRange = new Vector2(-10, 40);
        public static readonly float CameraScaleSpeed = 0.005F;
        public static readonly Vector2 CameraDistanceRange = new Vector2(1, 5);
        public static readonly float CameraTargetDefaultDistance = 4;
        public static readonly Vector3 CameraTargetPositionOffset = new Vector3(0, 1.35F, 0);

        public static readonly float PlayerMoveSpeed = 4;
        public static readonly float PlayerJumpForce = 6;
        public static readonly int PlayerJumpCount = 1;

    }
}
