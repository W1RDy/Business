using DG.Tweening;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIIncreaseAnimation", menuName = "UIAnimations/New UI Increase Animation")]
public class UIIncreaseAnimation : UIScaleAnimation
{
    [SerializeField] protected float _iterationTime = 0.2f;

    [SerializeField] private float _startSize;
    [SerializeField] private float _sizeChangeValue = -1;

    private Vector3 _rememberedSize;

    public override void Play(Action callback)
    {
        base.Play(callback);
        _isFinished = false;

        _rememberedSize = _transform.localScale;

        var endSize = _startSize + _sizeChangeValue;
        if (_sizeChangeValue < 0) endSize = _transform.localScale.x;

        _sequence = DOTween.Sequence();

        _transform.localScale = new Vector2(_startSize, _startSize);

        _sequence
            .Append(_transform.DOScale(endSize, _iterationTime))
            .AppendCallback(Finish);
    }

    protected override void Release()
    {
        base.Release();
        _transform.localScale = new Vector3(_rememberedSize.x, _rememberedSize.y, 1);
    }
}
