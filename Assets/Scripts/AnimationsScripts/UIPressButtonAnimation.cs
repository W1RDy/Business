using DG.Tweening;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIPressButtonAnimation", menuName = "UIAnimations/New UI Press Button Animation")]
public class UIPressButtonAnimation : UIScaleAnimation
{
    [SerializeField] private float _sizeChangeValue = -1;

    [SerializeField] private float _shrinkTime;
    [SerializeField] private float _intervalTime;
    [SerializeField] private float _increaseTime;

    private Vector2 _startScale;

    public override void Play(Action callback)
    {
        base.Play(callback);
        _isFinished = false;
        if (_sizeChangeValue < 0) throw new System.ArgumentException("Ivalid argument. SizeChangeValue can't be negative!");

        _startScale = new Vector2(_transform.localScale.x, _transform.localScale.y);
        var shrinkedScale = _startScale.Sum(-_sizeChangeValue);

        _sequence = DOTween.Sequence();

        _sequence
            .Append(_transform.DOScale(new Vector3(shrinkedScale.x, shrinkedScale.y, 1), _shrinkTime))
            .AppendInterval(_intervalTime)
            .Append(_transform.DOScale(new Vector3(_startScale.x, _startScale.y, 1), _increaseTime))
            .AppendCallback(Finish);
    }

    protected override void Release()
    {
        base.Release();
        _transform.localScale = new Vector3(_startScale.x, _startScale.y, 1);
    }
}