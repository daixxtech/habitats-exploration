using Game.Modules;
using UnityEngine;

namespace Game.Views.UI.Base {
    public abstract class AUIHandler : MonoBehaviour {
        private const int DESTROY_TIME_LIMIT = 10;

        public float DestroyCountDown { get; set; } = DESTROY_TIME_LIMIT;

        protected virtual void Awake() {
            GetComponent<Canvas>().worldCamera = UIModule.Instance.UICamera;
        }

        protected virtual void OnEnable() {
            DestroyCountDown = DESTROY_TIME_LIMIT;
        }
    }
}
