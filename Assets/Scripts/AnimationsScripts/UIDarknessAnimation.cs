using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Fade Animation", menuName = "UIAnimations/New Fade Animation")]
public class UIFadeAnimationWithText : UIAnimation
{
    [SerializeField] private float _fadeEnd;
    [SerializeField] private float _time;

    private IColorable _colorable;

    public void SetParameters(IColorable _colorable)
    {
        this._colorable = _colorable;
    }

    public override void Play(Action callback)
    {
        base.Play(callback);
        _isFinished = false;

        _sequence = DOTween.Sequence();

        if (_colorable.ColorableObj is Image image)
        {
            _sequence
                .Append(image.DOFade(_fadeEnd, _time))
                .AppendCallback(() => _finishCallback.Invoke());
        }
        else if (_colorable.ColorableObj is TextMeshProUGUI text)
        {
            Debug.Log("TextChange");
            _sequence
                .Append(text.DOFade(_fadeEnd, _time))
                .AppendCallback(() => _finishCallback.Invoke());
        }
        _sequence.Play();
    }

    protected override void Release()
    {
        _colorable.Color = new Color (_colorable.Color.r, _colorable.Color.g, _colorable.Color.b, _fadeEnd);
    }
}