using System;
using System.Collections;
using UnityEngine;

public class LoadSceneController : ClassForInitialization, IService
{
    private DarknessAnimationController _darknessAnimation;
    private ResetContoroller _resetContoroller;

    public event Action IsReseted;
    private bool _isLoaded;

    public LoadSceneController(DarknessAnimationController darknessAnimation) : base()
    {
        _darknessAnimation = darknessAnimation;
    }

    public override void Init()
    {
        _resetContoroller = ServiceLocator.Instance.Get<ResetContoroller>();
    }

    public void Reset()
    {
        _isLoaded = false;
        _darknessAnimation.StartCoroutine(WaitLoading());
    }

    public void LoadScene()
    {
        Debug.Log("LoadScene");
        Debug.Log(_isLoaded);
        if (!_isLoaded)
        {
            _darknessAnimation.PlayBrightnessAnimation(null);
            _isLoaded = true;
        }
    }

    private IEnumerator WaitLoading()
    {
        _resetContoroller.Reset();
        while (true)
        {
            yield return null;
            if (_resetContoroller.IsReseted) break;
        }
        IsReseted?.Invoke();
        Debug.Log("Reloaded");
    }
}