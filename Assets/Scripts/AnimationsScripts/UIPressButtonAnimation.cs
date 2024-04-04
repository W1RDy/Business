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

    public override void Play()
    {
        Play(null);
    }

    public override void Play(Action callback)
    {
        IsFinished = false;
        if (_sizeChangeValue < 0) throw new System.ArgumentException("Ivalid argument. SizeChangeValue can't be negative!");

        var startScale = new Vector2(_transform.localScale.x, _transform.localScale.y);
        var shrinkedScale = startScale.Sum(-_sizeChangeValue);

        var scaleSequence = DOTween.Sequence();

        scaleSequence
            .Append(_transform.DOScale(new Vector3(shrinkedScale.x, shrinkedScale.y, 1), _shrinkTime))
            .AppendInterval(_intervalTime)
            .Append(_transform.DOScale(new Vector3(startScale.x, startScale.y, 1), _increaseTime))
            .AppendCallback(() => {
                IsFinished = true;
                callback?.Invoke();
            });
    }
}