using System;
using TMPro;
using UnityEngine;

public class WasteCoinsButton : CustomButton, IButtonWithNewButton
{
    [SerializeField] private int _coinsValue;

    [SerializeField] private TextMeshProUGUI _buttonText;

    [SerializeField] private CustomButton _buttonForChange;
    [SerializeField] private CustomButton _secondButtonForChange;
    [SerializeField] private ChangeCondition[] _changeConditions;

    public ChangeCondition[] ChangeConditions => _changeConditions;
    public CustomButton ButtonForChange {get; private set;}

    private GamesConditionChecker _conditionsChecker;
    private ButtonChangeController _changeController;

    protected override void Init()
    {
        base.Init();
        _conditionsChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();
        _changeController = ServiceLocator.Instance.Get<ButtonChangeController>();

        ButtonForChange = _buttonForChange;
        _changeController.ChangeButtonToNewButton(this);
        _changeController.AddChangeButton(this);

        SetText();
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.WasteCoinsByProblems(_coinsValue);
    }

    private void OnEnable()
    {
        if (_changeController != null) _changeController.ChangeButtonToNewButton(this);
    }

    public void SetCoinsValue(int value)
    {
        _coinsValue = value;
    }

    private void SetText()
    {
        _buttonText.text = "Ok";
    }

    public bool CheckButtonChangeCondition()
    {
        if (_secondButtonForChange != null && !_conditionsChecker.IsEnoughCoins(_coinsValue))
        {
            ButtonForChange = _secondButtonForChange;
            return true;
        }

        //if (ButtonForChange != _buttonForChange) ButtonForChange = _buttonForChange;
        return !_conditionsChecker.IsEnoughCoinsInHands(_coinsValue);
    }

    private void OnDestroy()
    {
        if ( _changeController != null )
        {
            _changeController.RemoveChangeButton(this);
        }
    }
}
