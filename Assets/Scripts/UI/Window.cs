using System;
using UnityEngine;

public class Window : ObjectForInitializationWithChildren
{
    [SerializeField] private WindowType _type;
    public WindowType Type => _type;

    [SerializeField] private UIAnimation _openAnimation;
    [SerializeField] private UIAnimation _closeAnimation;

    private UIAnimation _openAnimationInstance;
    private UIAnimation _closeAnimationInstance;

    public bool IsChanging { get; private set; }
    public event Action OnWindowChanged;

    public override void Init()
    {
        base.Init();

        if (_openAnimation != null) _openAnimationInstance = Instantiate(_openAnimation);
        if (_closeAnimation != null) _closeAnimationInstance = Instantiate(_closeAnimation);

        if (_openAnimationInstance is UIScaleAnimation _openScaleAnimation) _openScaleAnimation.SetParametres(transform);
        if (_closeAnimationInstance is UIScaleAnimation _closeScaleAnimation) _closeScaleAnimation.SetParametres(transform);
    }

    public virtual void ActivateWindow()
    {
        gameObject.SetActive(true);
        if (_type == WindowType.FinishPeriodWindow) Debug.Log(_openAnimation);

        Action endChangeDelegate = () =>
        {
            IsChanging = false;
            OnWindowChanged?.Invoke();
        };

        if (_openAnimationInstance != null)
        {
            InteruptActivatedAnimations();
            IsChanging = true;
            _openAnimationInstance.Play(endChangeDelegate);
        }
        else endChangeDelegate.Invoke();
    }

    public virtual void DeactivateWindow()
    {
        Action endChangeDelegate = () =>
        {
            gameObject.SetActive(false);
            IsChanging = false;
            OnWindowChanged?.Invoke();
        };

        if (_closeAnimationInstance != null)
        {
            InteruptActivatedAnimations();
            IsChanging = true;
            _closeAnimationInstance.Play(endChangeDelegate);

        }
        else endChangeDelegate?.Invoke();
    }

    private void InteruptActivatedAnimations()
    {
        if (IsAnimationInProcess(_openAnimationInstance)) _openAnimationInstance.Kill();
        else if (IsAnimationInProcess(_closeAnimationInstance)) _closeAnimationInstance.Kill();
    }

    private bool IsAnimationInProcess(UIAnimation animation)
    {
        return animation != null && !animation.IsFinished;
    }

    private void OnDisable()
    {
        InteruptActivatedAnimations();
    }
}
