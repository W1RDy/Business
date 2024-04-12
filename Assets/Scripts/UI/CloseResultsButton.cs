using UnityEngine;

public class CloseResultsButton : CloseButton, IButtonWithNewButton
{
    [SerializeField] private CustomButton _buttonForChange;
    [SerializeField] private ChangeCondition[] _changeConditions;

    public ChangeCondition[] ChangeConditions => _changeConditions;
    public CustomButton ButtonForChange => _buttonForChange;

    private ButtonChangeController _buttonChangeController;
    private GamesConditionChecker _conditionChecker;

    protected override void Init()
    {
        base.Init();
        _buttonChangeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        _buttonChangeController.ChangeButtonToNewButton(this);
        _buttonChangeController.AddChangeButton(this);
    }

    public bool CheckButtonChangeCondition()
    {
        return _conditionChecker.IsGameFinished();
    }

    private void OnEnable()
    {
        if (_buttonChangeController != null) _buttonChangeController.ChangeButtonToNewButton(this);
    }

    public void OnDestroy()
    {
        if (_buttonChangeController != null)
        {
            _buttonChangeController.RemoveChangeButton(this);
        }
    }
}