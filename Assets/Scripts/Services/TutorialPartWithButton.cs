using UnityEngine;

public class TutorialPartWithButton : TutorialSegmentPart
{
    [SerializeField] private TutorialButtonType _buttonType;
    private ButtonForTutorialActivator _activator;
    private ITutorialButton _tutorialButton;
    
    private bool _isCompleted;
    private bool _isInitialized;

    private void Init()
    {
        _activator = ServiceLocator.Instance.Get<ButtonForTutorialActivator>();
        _isInitialized = true;
    }

    public override void Activate()
    {
        if (!_isInitialized) Init();
        _tutorialButton = _activator.ActivateButton(_buttonType);
        _tutorialButton.OnClick += CompleteDelegate;
        _isCompleted = false;
    }

    public override void Deactivate()
    {
        _activator.DeactivateButton();
    }

    public override bool ConditionCompleted()
    {
        return _isCompleted;
    }

    private void CompleteDelegate()
    {
        _isCompleted = true;
        _tutorialButton.OnClick -= CompleteDelegate;
    }

    private void OnDestroy()
    {
        if (_tutorialButton != null) _tutorialButton.OnClick -= CompleteDelegate;
    }
}
