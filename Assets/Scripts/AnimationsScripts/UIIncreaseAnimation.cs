using DG.Tweening;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIIncreaseAnimation", menuName = "UIAnimations/New UI Increase Animation")]
public class UIIncreaseAnimation : UIScaleAnimation
{
    [SerializeField] protected float _iterationTime = 0.2f;

    [SerializeField] private float _startSize;
    [SerializeField] private float _sizeChangeValue = -1;

    public override void Play()
    {
        Play(null);
    }

    public override void Play(Action callback)
    {
        IsFinished = false;

        var size = _startSize + _sizeChangeValue;
        if (_sizeChangeValue < 0) size = _transform.localScale.x;

        var scaleSequence = DOTween.Sequence();

        _transform.localScale = new Vector2(_startSize, _startSize);

        scaleSequence
            .Append(_transform.DOScale(size, _iterationTime))
            .AppendCallback(() => {
                IsFinished = true;
                callback?.Invoke();
            });
    }
}
