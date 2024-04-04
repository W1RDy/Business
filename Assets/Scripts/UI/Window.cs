using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
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
            _openAnimation.Play();
        }
    }

    public void DeactivateWindow()
    {
        if (_closeAnimation)
        {
            _closeAnimation.Play(() => gameObject.SetActive(false));
        }
        else gameObject.SetActive(false);
    }
}
