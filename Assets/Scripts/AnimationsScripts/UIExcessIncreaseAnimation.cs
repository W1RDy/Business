using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "UIExcessIncreaseAnimation", menuName = "UIAnimations/New UI Excess Increase Animation")]
public class UIExcessIncreaseAnimation : UIScaleAnimation
{
    [SerializeField] protected float _iterationTime = 0.2f;

    private float _startScale;

    public override void Play(Action callback)
    {
        base.Play(callback);

        _isFinished = false;

        _startScale = _transform.localScale.x;

        _sequence = DOTween.Sequence();

        _transform.localScale = Vector2.zero;

        _sequence
            .Append(_transform.DOScale(_startScale + 0.3f, _iterationTime))
            .Append(_transform.DOScale(_startScale, _iterationTime))
            .AppendCallback(() => _finishCallback.Invoke());
    }

    protected override void Release()
    {
        _transform.localScale = new Vector3(_startScale, _startScale, _transform.localScale.z);
    }
}