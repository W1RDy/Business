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

    public override void Play()
    {
        Play(null);
    }

    public override void Play(Action callback)
    {
        IsFinished = false;

        var fadeSequence = DOTween.Sequence();

        fadeSequence
            .Append(_image.DOFade(_darknessEnd, _time))
            .AppendCallback(() =>
            {
                IsFinished = true;
                callback?.Invoke();
            });
    }
}