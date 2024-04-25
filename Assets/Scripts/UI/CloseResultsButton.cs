using I2.Loc;
using TMPro;
using UnityEngine;

public class CloseResultsButton : CloseButton, IButtonWithNewButton
{
    [SerializeField] private CustomButton _buttonForChange;
    [SerializeField] private ChangeCondition[] _changeConditions;

    [SerializeField] private TextMeshProUGUI _buttonText;

    public ChangeCondition[] ChangeConditions => _changeConditions;
    public CustomButton ButtonForChange => _buttonForChange;

    private ButtonChangeController _buttonChangeController;
    private GamesConditionChecker _conditionChecker;

    public override void Init()
    {
        base.Init();
        _buttonChangeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        _buttonChangeController.AddChangeButton(this);
        SetText();
    }

    private void OnEnable()
    {
        if (_buttonChangeController != null) _buttonChangeController.ChangeButtonToNewButton(this);
    }

    public bool CheckButtonChangeCondition()
    {
        return _conditionChecker.IsGameFinished();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_buttonChangeController != null)
        {
            _buttonChangeController.RemoveChangeButton(this);
        }
    }

    private void SetText()
    {
        _buttonText.text = LocalizationManager.GetTranslation("Ok");
    }
}