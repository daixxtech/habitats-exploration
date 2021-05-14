using System;
using UnityEngine;

namespace Game.Facade {
    public static class Player {
        public static Action<Collider, bool> OnTriggeredClue;
        public static Action<int> OnInteractedClue;
    }
}
