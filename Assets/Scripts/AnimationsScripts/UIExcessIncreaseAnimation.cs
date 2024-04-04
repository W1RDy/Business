using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "UIExcessIncreaseAnimation", menuName = "UIAnimations/New UI Excess Increase Animation")]
public class UIExcessIncreaseAnimation : UIScaleAnimation
{
    [SerializeField] protected float _iterationTime = 0.2f;

    public override void Play()
    {
        Play(null);
    }

    public override void Play(Action callback)
    {
        IsFinished = false;

        var startScale = _transform.localScale;

        var scaleSequence = DOTween.Sequence();

        _transform.localScale = Vector2.zero;

        scaleSequence
            .Append(_transform.DOScale(startScale.x + 0.3f, _iterationTime))
            .Append(_transform.DOScale(startScale.x, _iterationTime))
            .AppendCallback(() => {
                IsFinished = true;
                callback?.Invoke();
            });
    }
}