using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : ObjectForInitializationWithChildren
{
    [SerializeField] private WindowType _type;
    public WindowType Type => _type;

    [SerializeField] private UIAnimation _openAnimation;
    [SerializeField] private UIAnimation _closeAnimation;
    private bool _animationIsInitialized;

    private void InitAnimations()
    {
        _animationIsInitialized = true;

        _openAnimation = Instantiate(_openAnimation);
        _closeAnimation = Instantiate(_closeAnimation);

        if (_openAnimation is UIScaleAnimation _openScaleAnimation) _openScaleAnimation.SetParametres(transform);
        if (_closeAnimation is UIScaleAnimation _closeScaleAnimation) _closeScaleAnimation.SetParametres(transform);
    }

    public void ActivateWindow()
    {
        gameObject.SetActive(true);

        if (_openAnimation)
        {
            if (!_animationIsInitialized) InitAnimations();
            InteruptActivatedAnimations();
            _openAnimation.Play();
        }
    }

    public void DeactivateWindow()
    {
        if (_closeAnimation)
        {
            InteruptActivatedAnimations();
            _closeAnimation.Play(() =>
            {
                gameObject.SetActive(false);
            });
        }
        else gameObject.SetActive(false);
    }

    private void InteruptActivatedAnimations()
    {
        if (_openAnimation != null && !_openAnimation.IsFinished) _openAnimation.Kill();
        else if (_closeAnimation != null && !_closeAnimation.IsFinished) _closeAnimation.Kill();
    }

    private void OnDisable()
    {
        InteruptActivatedAnimations();
    }
}