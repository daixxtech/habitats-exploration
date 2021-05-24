using Frame.Runtime.Modules;
using Game.Config;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Views.UI.HUD {
    public class JoystickElem : MonoBehaviour, IDragHandler, IEndDragHandler {
        private Vector2 _origin;
        private RectTransform _centerTrans;
#if UNITY_EDITOR
        private bool _onDrag;
#endif

        private void Start() {
            _origin = RectTransformUtility.WorldToScreenPoint(UIModule.Instance.UICamera, transform.position);
            _centerTrans = transform.Find("Control/Center").GetComponent<RectTransform>();
        }

#if UNITY_EDITOR
        private void Update() {
            if (!_onDrag) {
                const float RADIUS_MAX = 1.414214F;
                Vector2 offset = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                float radius = Mathf.Clamp(offset.magnitude, 0, RADIUS_MAX);
                Vector2 direction = offset.normalized;
                Facade.Input.OnJoystickDragged?.Invoke(direction * radius / RADIUS_MAX);
            }
        }
#endif

        public void OnDrag(PointerEventData eventData) {
#if UNITY_EDITOR
            _onDrag = true;
#endif
            Vector2 offset = eventData.position - _origin;
            float radius = Mathf.Clamp(offset.magnitude, 0, GameConfig.JoystickDragRadius);
            Vector2 direction = offset.normalized;
            _centerTrans.anchoredPosition = direction * radius;
            Facade.Input.OnJoystickDragged?.Invoke(direction * radius / GameConfig.JoystickDragRadius);
        }

        public void OnEndDrag(PointerEventData eventData) {
#if UNITY_EDITOR
            _onDrag = false;
#endif
            _centerTrans.anchoredPosition = Vector2.zero;
            Facade.Input.OnJoystickDragged?.Invoke(Vector2.zero);
        }
    }
}
