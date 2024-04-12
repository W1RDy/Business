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
    private float _size;

    public override void Play(Action callback)
    {
        if (!IsFinished) Kill();
        _isFinished = false;

        var startScale = _transform.localScale;
        _size = startScale.x - _sizeChangeValue;
        if (_sizeChangeValue < 0) _size = 0;

        _finishCallback = () =>
        {
            _isFinished = true;
            _transform.localScale = startScale;
            callback?.Invoke();
        };

        _sequence = DOTween.Sequence();

        _sequence
            .Append(_transform.DOScale(_size, _iterationTime))
            .AppendCallback(() => _finishCallback.Invoke());
    }

    protected override void Release()
    {
        _transform.localScale = new Vector3(_size, _size, _transform.localScale.z);
    }
}