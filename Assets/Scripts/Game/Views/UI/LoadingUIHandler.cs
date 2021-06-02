using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.LOADING)]
    public class LoadingUIHandler : UIHandlerBase {
        private Text _tipsTxt;
        private RectTransform _valueRect;
        private float _startValue, _diffValue;

        private void Awake() {
            _tipsTxt = transform.Find("Root/Content/TipsTxt").GetComponent<Text>();
            _valueRect = transform.Find("Root/Content/ProgressBar/ValueImg").GetComponent<RectTransform>();
            Vector2 sizeDelta = _valueRect.sizeDelta;
            _startValue = sizeDelta.y;
            _diffValue = sizeDelta.x - sizeDelta.y;
        }

        public void OnEnable() {
            _tipsTxt.gameObject.SetActive(false);
            _valueRect.gameObject.SetActive(false);
            var tipsConfs = CLoadingTips.GetArray();
            if (tipsConfs.Length != 0) {
                int randomIndex = Random.Range(0, tipsConfs.Length);
                _tipsTxt.text = tipsConfs[randomIndex].content;
            }
            _tipsTxt.gameObject.SetActive(true);
            _valueRect.gameObject.SetActive(true);

            if (UIModule.Instance.Parameter is string sceneName) {
                AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
                StartCoroutine(RefreshProgress(operation));
            } else {
                Debug.LogError("[LoadingUIHandler] OnEnable: UI Param is null");
            }
        }

        private IEnumerator RefreshProgress(AsyncOperation operation) {
            operation.allowSceneActivation = false;
            float curValue = 0, targetValue;
            Vector2 sizeDelta = new Vector2(_startValue, _startValue);
            _valueRect.sizeDelta = sizeDelta;
            // 进度条平滑加载至当前异步操作的进度值
            while (operation.progress < 0.9F) {
                targetValue = (int) (operation.progress * 100);
                while (curValue < targetValue) {
                    sizeDelta.x = _startValue + _diffValue * (++curValue / 100.0F);
                    _valueRect.sizeDelta = sizeDelta;
                    yield return CoroutineUtil.EndOfFrame;
                }
                yield return CoroutineUtil.EndOfFrame;
            }
            // 当异步加载至 90% 时，平滑加载剩余 10%
            targetValue = 100;
            while (curValue < targetValue) {
                sizeDelta.x = _startValue + _diffValue * (++curValue / 100.0F);
                _valueRect.sizeDelta = sizeDelta;
                yield return CoroutineUtil.EndOfFrame;
            }
            operation.allowSceneActivation = true;
        }
    }
}
