using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using YG;

public class TimerBeforeAdsYG : ObjectForInitialization
{
    [SerializeField] private ADSWarning _adsWarning;

    [SerializeField,
        Tooltip("Работа таймера в реальном времени, независимо от time scale.")]
    private bool realtimeSeconds;

    [Space(20)]
    [SerializeField]
    private UnityEvent onShowTimer;
    [SerializeField]
    private UnityEvent onHideTimer;
    private int objSecCounter;

    private bool _isStopTimer;

    public override void Init()
    {
        base.Init();

        YandexGame.OpenFullAdEvent += StopTimer;
        YandexGame.OpenVideoEvent += StopTimer;

        YandexGame.CloseFullAdEvent += RestartTimer;
        YandexGame.CloseVideoEvent += StartTimer;

        StartTimer();
    }

    IEnumerator CheckTimerAd()
    {
        while (true)
        {
            if (_isStopTimer) yield break;
            if (YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
            {
                onShowTimer?.Invoke();
                objSecCounter = 0;
                _adsWarning.ActivateWarning();

                StartCoroutine(TimerAdShow());
                yield break;
            }

            if (_isStopTimer) yield break;
            if (!realtimeSeconds)
                yield return new WaitForSeconds(1.0f);
            else
                yield return new WaitForSecondsRealtime(1.0f);
            if (_isStopTimer) yield break;
        }
    }

    IEnumerator TimerAdShow()
    {
        while (true)
        {
            if (objSecCounter < 3)
            {
                objSecCounter++;
                if (!realtimeSeconds)
                    yield return new WaitForSeconds(1.0f);
                else
                    yield return new WaitForSecondsRealtime(1.0f);
            }

            if (objSecCounter == 3)
            {
                YandexGame.FullscreenShow();
                yield break;
            }
        }
    }

    private void RestartTimer()
    {
        _adsWarning.DeactivateWarning();
        onHideTimer?.Invoke();
        objSecCounter = 0;
        StartTimer();
    }

    private void StopTimer()
    {
        _isStopTimer = true;
    }

    private void StartTimer()
    {
        _isStopTimer = false;
        StartCoroutine(CheckTimerAd());
    }

    private void OnDestroy()
    {
        YandexGame.OpenFullAdEvent -= StopTimer;
        YandexGame.OpenVideoEvent -= StopTimer;

        YandexGame.CloseFullAdEvent -= RestartTimer;
        YandexGame.CloseVideoEvent -= StartTimer;
    }
}
