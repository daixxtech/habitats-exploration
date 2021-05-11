using Game.Config;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Views.UI {
    public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler {
        private Vector2 _origin;
        private RectTransform _centerTrans;

        protected void Start() {
            _origin = ((RectTransform) transform).anchoredPosition;
            _centerTrans = transform.Find("Control/Center").GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData) {
            Vector2 offset = eventData.position - _origin;
            float radius = Mathf.Clamp(offset.magnitude, 0, GameConfig.JoystickDragRadius);
            Vector2 direction = offset.normalized;
            _centerTrans.anchoredPosition = direction * radius;
            Facade.Input.OnJoystickDragged?.Invoke(direction * radius / GameConfig.JoystickDragRadius);
        }

        public void OnEndDrag(PointerEventData eventData) {
            _centerTrans.anchoredPosition = Vector2.zero;
            Facade.Input.OnJoystickDragged?.Invoke(Vector2.zero);
        }
    }
}