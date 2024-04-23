using System;
using UnityEngine;

public class DarknessAnimationController : ObjectForInitialization, IService
{
    [SerializeField] private UIFadeAnimationWithText _darknessAnimation;
    [SerializeField] private UIFadeAnimationWithText _brightnessAnimation;
    private UIFadeAnimationWithText _darknessAnimationInstance;
    private UIFadeAnimationWithText _brightnessAnimationInstance;

    [SerializeField] private CustomImage _darknessView;
    [SerializeField] private ClicksBlocker _clicksBlocker;

    public override void Init()
    {
        base.Init();

        _darknessAnimationInstance = Instantiate(_darknessAnimation);
        _brightnessAnimationInstance = Instantiate(_brightnessAnimation);

        _darknessAnimationInstance.SetParameters(_darknessView);
        _brightnessAnimationInstance.SetParameters(_darknessView);
    }

    public void PlayDarknessAnimation(Action callback)
    {
        _clicksBlocker.BlockClicks();
        _darknessAnimationInstance.Play(callback);
    }

    public void PlayBrightnessAnimation(Action callback)
    {
        _clicksBlocker.UnblockClicks();
        _brightnessAnimationInstance.Play(callback);
    }
}