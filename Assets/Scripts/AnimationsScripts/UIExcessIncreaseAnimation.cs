﻿using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "UIExcessIncreaseAnimation", menuName = "UIAnimations/New UI Excess Increase Animation")]
public class UIExcessIncreaseAnimation : UIScaleAnimation
{
    [SerializeField] protected float _iterationTime = 0.2f;

    private float _startScale;
    private Vector3 _rememberedSize;

    public override void Play(Action callback)
    {
        base.Play(callback);

        _isFinished = false;

        _rememberedSize = _transform.localScale;
        _startScale = _transform.localScale.x;

        _sequence = DOTween.Sequence();

        _transform.localScale = Vector2.zero;

        _sequence
            .Append(_transform.DOScale(_startScale + 0.3f, _iterationTime))
            .Append(_transform.DOScale(_startScale, _iterationTime))
            .AppendCallback(Finish);
    }

    protected override void Release()
    {
        base.Release();
        _transform.localScale = new Vector3(_rememberedSize.x, _rememberedSize.y, 1);
    }
}