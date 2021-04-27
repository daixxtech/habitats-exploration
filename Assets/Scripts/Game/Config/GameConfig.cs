using UnityEngine;

namespace Game.Config {
    public static class GameConfig {
        public static readonly LayerMask GroundLayer = LayerMask.GetMask("Ground");
        public static readonly float CameraTargetDefaultDistance = 4;
    }
}
