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
    private Vector2 _startScale;

    public override void Play(Action callback)
    {
        if (!IsFinished) Kill();
        _isFinished = false;

        _startScale = _transform.localScale;
        var size = _startScale.x - _sizeChangeValue;
        if (_sizeChangeValue < 0) size = 0;

        _finishCallback = () =>
        {
            _isFinished = true;
            _transform.localScale = _startScale;
            callback?.Invoke();

            Release();
            _finishCallback = null;
        };

        _sequence = DOTween.Sequence();

        _sequence
            .Append(_transform.DOScale(size, _iterationTime))
            .AppendCallback(() => _finishCallback?.Invoke());
    }

    protected override void Release()
    {
        _transform.localScale = new Vector3(_startScale.x, _startScale.y, 1);
    }
}