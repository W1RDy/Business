using DG.Tweening;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIIncreaseAnimation", menuName = "UIAnimations/New UI Increase Animation")]
public class UIIncreaseAnimation : UIScaleAnimation
{
    [SerializeField] protected float _iterationTime = 0.2f;

    [SerializeField] private float _startSize;
    [SerializeField] private float _sizeChangeValue = -1;

    private float _endSize;

    public override void Play(Action callback)
    {
        base.Play(callback);
        _isFinished = false;

        _endSize = _startSize + _sizeChangeValue;
        if (_sizeChangeValue < 0) _endSize = _transform.localScale.x;

        _sequence = DOTween.Sequence();

        _transform.localScale = new Vector2(_startSize, _startSize);

        _sequence
            .Append(_transform.DOScale(_endSize, _iterationTime))
            .AppendCallback(() => _finishCallback.Invoke());
    }

    protected override void Release()
    {
        _transform.localScale = new Vector3(_endSize, _endSize, _transform.localScale.z);
    }
}
