using System;
using UnityEngine;

public abstract class TutorialButton : CustomButton, ITutorialButton
{
    [SerializeField] private UIAnimation _tutorialAnimation;
    [SerializeField] private TutorialButtonType _tutorialButtonType;
    [SerializeField] private bool _isActiveForTutorial = false;

    private UIAnimation _tutorialAnimationInstance;

    public TutorialButtonType Type => _tutorialButtonType;

    public event Action OnClick;

    protected override void InitByTutorial()
    {
        base.InitByTutorial();
        if (_isActiveForTutorial)
        {
            var tutorialButtonService = ServiceLocator.Instance.Get<ButtonsForTutorialService>();
            tutorialButtonService.AddButton(this);

            if (_tutorialAnimation != null)
            {
                _tutorialAnimationInstance = Instantiate(_tutorialAnimation);
                if (_tutorialAnimationInstance is HighlightAndScaleAnimation highlightAndScaleAnimation) highlightAndScaleAnimation.SetParametres(transform, _button.image);
            }
        }
    }

    public void ActivateByTutorial()
    {
        if (_tutorialAnimationInstance) _tutorialAnimationInstance.Play();
        ActivateClickableByTutorial();
    }

    public void DeactivateByTutorial()
    {
        if (_tutorialAnimationInstance) _tutorialAnimationInstance.Kill();
        DeactivateClickableByTutorial();
    }

    protected override void ClickCallback()
    {
        if (_tutorialAnimationInstance) _tutorialAnimationInstance.Kill();
        base.ClickCallback();
        OnClick?.Invoke();
    }
}