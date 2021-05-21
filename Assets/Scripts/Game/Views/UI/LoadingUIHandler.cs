﻿using Frame.Runtime.Modules;
using Frame.Runtime.Modules.UI;
using Frame.Runtime.Views.UI;
using Game.Config;
using Game.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views.UI {
    [UIBind(UIDef.LOADING)]
    public class LoadingUIHandler : UIHandlerBase {
        private Text _tipsTxt;
        private Image _progressBarImg;

        private void Awake() {
            _tipsTxt = transform.Find("Tips/Text").GetComponent<Text>();
            _progressBarImg = transform.Find("ProgressBar/ValueImg").GetComponent<Image>();
        }

        public void OnEnable() {
            _tipsTxt.gameObject.SetActive(false);
            _progressBarImg.gameObject.SetActive(false);
            var tipsConfs = CLoadingTips.GetArray();
            if (tipsConfs.Length != 0) {
                int randomIndex = Random.Range(0, tipsConfs.Length);
                _tipsTxt.text = tipsConfs[randomIndex].content;
            }
            _tipsTxt.gameObject.SetActive(true);
            _progressBarImg.gameObject.SetActive(true);

            if (UIModule.Instance.Parameter is AsyncOperation operation) {
                StartCoroutine(RefreshProgress(operation));
            } else {
                Debug.LogError("[LoadingUIHandler] OnEnable: UI Param is null");
            }
        }

        private IEnumerator RefreshProgress(AsyncOperation operation) {
            float curValue, targetValue;
            _progressBarImg.fillAmount = curValue = 0;
            // 进度条平滑加载至当前异步操作的进度值
            while (operation.progress < 0.9F) {
                targetValue = (int) (operation.progress * 100);
                while (curValue < targetValue) {
                    _progressBarImg.fillAmount = ++curValue / 100.0F;
                    yield return CoroutineUtil.EndOfFrame;
                }
                yield return CoroutineUtil.EndOfFrame;
            }
            // 当异步加载至 90% 时，平滑加载剩余 10%
            targetValue = 100;
            while (curValue < targetValue) {
                _progressBarImg.fillAmount = ++curValue / 100.0F;
                yield return CoroutineUtil.EndOfFrame;
            }
            yield return new WaitForSeconds(0.5F);
            operation.allowSceneActivation = true;
        }
    }
}
