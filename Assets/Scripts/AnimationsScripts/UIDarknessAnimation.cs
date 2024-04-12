using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Darkness Animation", menuName = "UIAnimations/New Darkness Animation")]
public class UIDarknessAnimation : UIAnimation
{
    [SerializeField] private float _darknessEnd;
    [SerializeField] private float _time;

    private Image _image;

    public void SetParameters(Image _darknessView)
    {
        _image = _darknessView;
    }

    public override void Play(Action callback)
    {
        base.Play(callback);
        _isFinished = false;

        _sequence = DOTween.Sequence();

        _sequence
            .Append(_image.DOFade(_darknessEnd, _time))
            .AppendCallback(() => _finishCallback.Invoke());
    }

    protected override void Release()
    {
        _image.color = new Color (_image.color.r, _image.color.g, _image.color.b, _darknessEnd);
    }
}