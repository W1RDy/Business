using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "UIShrinkAnimation", menuName = "UIAnimations/New UI Shrink Animation")]
public class UIShrinkAnimation : UIScaleAnimation
{
    [SerializeField] protected float _iterationTime = 0.2f;

    [SerializeField] private float _sizeChangeValue;

    public override void Play()
    {
        Play(null);
    }

    public override void Play(Action callback)
    {
        IsFinished = false;

        var startScale = _transform.localScale;
        var size = startScale.x - _sizeChangeValue;

        if (_sizeChangeValue < 0) size = 0;

        var scaleSequence = DOTween.Sequence();

        scaleSequence
            .Append(_transform.DOScale(size, _iterationTime))
            .AppendCallback(() => {
                IsFinished = true;
                callback?.Invoke();
                _transform.localScale = startScale;
            });
    }
}