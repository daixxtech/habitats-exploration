using System.Collections.Generic;
using UnityEngine;

namespace Frame.Runtime.Views.UI {
    public class UIContainer : MonoBehaviour {
        private GameObject _template;
        private Transform _pool;
        private List<Component> _spares;
        public List<Component> Children { get; private set; }

        private void Awake() {
            _pool = new GameObject("Pool").AddComponent<RectTransform>();
            _pool.gameObject.SetActive(false);
            _pool.SetParent(transform, false);
            _template = transform.Find("Template").gameObject;
            _template.SetActive(false);

            _spares = new List<Component>();
            Children = new List<Component>();
        }

        public void SetCount<T>(int count) where T : Component {
            if (count > Children.Count) {
                int diff = count - Children.Count;
                while (_spares.Count > 0 && diff > 0) {
                    int lastIndex = _spares.Count - 1;
                    Component spare = _spares[lastIndex];
                    _spares.RemoveAt(lastIndex);
                    spare.gameObject.SetActive(true);
                    spare.transform.SetParent(transform);
                    Children.Add(spare);
                    --diff;
                }
                while (diff > 0) {
                    GameObject instance = Instantiate(_template, transform);
                    instance.SetActive(true);
                    Children.Add(instance.GetComponent<T>() ?? instance.AddComponent<T>());
                    --diff;
                }
            } else if (count < Children.Count) {
                for (int i = Children.Count - 1; i >= count; i--) {
                    Component surplus = Children[i];
                    surplus.gameObject.SetActive(false);
                    surplus.transform.SetParent(_pool);
                    _spares.Add(surplus);
                    Children.RemoveAt(i);
                }
            }
        }
    }
}
