using UnityEngine;

public class CloseResultsButton : CloseButton, IChangeButton
{
    [SerializeField] private CustomButton _buttonForChange;
    public CustomButton ButtonForChange => _buttonForChange;

    private ButtonChangeController _buttonChangeController;
    private GamesConditionChecker _conditionChecker;

    protected override void Init()
    {
        base.Init();
        _buttonChangeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        _buttonChangeController.ChangeButton(this);
        _buttonChangeController.AddChangeButton(this);
    }

    public bool CheckChangeCondition()
    {
        return _conditionChecker.IsGameFinished();
    }

    private void OnEnable()
    {
        if (_buttonChangeController != null) _buttonChangeController.ChangeButton(this);
    }

    public void OnDestroy()
    {
        if (_buttonChangeController != null)
        {
            _buttonChangeController.RemoveChangeButton(this);
        }
    }
}