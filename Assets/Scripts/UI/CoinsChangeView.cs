using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CustomText))]
public class CoinsChangeView : MonoBehaviour
{
    [SerializeField] private UIFadeAnimationWithText _activateAnimation;
    [SerializeField] private UIFadeAnimationWithText _deactivateAnimation;

    private UIFadeAnimationWithText _activateAnimationInstance;
    private UIFadeAnimationWithText _deactivateAnimationInstance;

    [SerializeField] private Color _positiveColor;
    [SerializeField] private Color _negativeColor;

    private CustomText _text;

    private bool _isInitialized;

    private void Init()
    {
        _isInitialized = true;
        _text = GetComponent<CustomText>();

        _activateAnimationInstance = Instantiate(_activateAnimation);
        _deactivateAnimationInstance = Instantiate(_deactivateAnimation);

        _activateAnimationInstance.SetParameters(_text);
        _deactivateAnimationInstance.SetParameters(_text);
    }

    public void ActivateChangeView(int changeValue)
    {
        if (!_isInitialized) Init();

        InteruptAnimations();
        SetChangeValue(changeValue);

        _activateAnimationInstance.Play(() =>
        {
            _deactivateAnimationInstance.Play();
        });
    }

    private void SetChangeValue(int value)
    {
        bool isPositive = value >= 0;

        if (isPositive) _text.Color = _positiveColor;
        else _text.Color = _negativeColor;

        var symbol = isPositive ? "+" : "-";
        _text.Text = symbol + Mathf.Abs(value);
    }

    private void InteruptAnimations()
    {
        if (_activateAnimation != null && !_activateAnimation.IsFinished) _activateAnimation.Kill();
        else if (_deactivateAnimation != null && !_deactivateAnimation.IsFinished) _deactivateAnimation.Kill();
    }

    private void OnDestroy()
    {
        InteruptAnimations();
    }
}