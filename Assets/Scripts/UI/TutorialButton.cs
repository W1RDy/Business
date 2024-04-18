using System;
using UnityEngine;

public abstract class TutorialButton : CustomButton, ITutorialButton
{
    [SerializeField] private UIAnimation _tutorialAnimation;
    [SerializeField] private TutorialButtonType _tutorialButtonType;
    [SerializeField] protected bool _isActiveForTutorial = false;

    public TutorialButtonType Type => _tutorialButtonType;

    public event Action OnClick;

    public override void Init()
    {
        base.Init();
        if (_isActiveForTutorial)
        {
            var tutorialButtonService = ServiceLocator.Instance.Get<ButtonsForTutorialService>();
            tutorialButtonService.AddButton(this);
        }
    }

    public void ActivateByTutorial()
    {
        if (_tutorialAnimation) _tutorialAnimation.Play();
        ActivateClickableByTutorial();
    }

    public void DeactivateByTutorial()
    {
        if (_tutorialAnimation) _tutorialAnimation.Kill();
        DeactivateClickableByTutorial();
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        OnClick?.Invoke();
    }
}