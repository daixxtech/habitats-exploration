using Game.Modules.Base;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Modules {
    public struct TouchState {
        public bool Touching { get; set; }
        public bool OverUI { get; set; }
    }

    public class InputModule : IModule {
        private static InputModule _Instance;
        public static InputModule Instance => _Instance ?? (_Instance = new InputModule());

        private const int TOUCH_COUNT = 10;
        private TouchState[] _touchStates;

        public bool NeedUpdate { get; } = true;
        public List<Touch> SceneTouches { get; private set; }

        public void Init() {
            _touchStates = new TouchState[TOUCH_COUNT];
            SceneTouches = new List<Touch>();
        }

        public void Dispose() { }

        public void Update() {
            /* 更新 TouchStates */
            Touch[] touches = Input.touches;
            int touchCount = Input.touchCount;
            for (int i = 0; i < TOUCH_COUNT; i++) {
                bool updated = false;
                for (int j = 0; j < touchCount; j++) {
                    Touch touch = touches[j];
                    if (i == touch.fingerId) {
                        if (!_touchStates[i].Touching) {
                            _touchStates[i].Touching = true;
                            _touchStates[i].OverUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
                        }
                        updated = true;
                        break;
                    }
                }
                if (!updated) {
                    _touchStates[i].Touching = false;
                }
            }
            /* 更新 SceneTouches */
            SceneTouches.Clear();
            for (int j = 0; j < touchCount; j++) {
                Touch touch = touches[j];
                if (touch.fingerId < TOUCH_COUNT && !_touchStates[touch.fingerId].OverUI) {
                    SceneTouches.Add(touch);
                }
            }
        }
    }
}
