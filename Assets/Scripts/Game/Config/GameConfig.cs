using UnityEngine;

namespace Game.Config {
    public static class GameConfig {
        public static readonly float CameraTargetDefaultDistance = 4;
        public static readonly float JoystickDragRadius = 200;

        public static readonly LayerMask GroundLayer = LayerMask.GetMask("Ground");
    }
}
