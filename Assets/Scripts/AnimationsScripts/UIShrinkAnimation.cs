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
    private Vector3 _startScale;

    public override void Play(Action callback)
    {
        base.Play(callback);

        _startScale = _transform.localScale;
        var size = _startScale.x - _sizeChangeValue;
        if (_sizeChangeValue < 0) size = 0;

        _sequence = DOTween.Sequence();

        _sequence
            .Append(_transform.DOScale(size, _iterationTime))
            .AppendCallback(Finish);
    }

    protected override void Release()
    {
        base.Release();
        _transform.localScale = _startScale;
    }
}