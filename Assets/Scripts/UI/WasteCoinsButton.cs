using System;
using TMPro;
using UnityEngine;

public class WasteCoinsButton : CustomButton, IChangeButton
{
    [SerializeField] private int _coinsValue;

    [SerializeField] private TextMeshProUGUI _buttonText;

    [SerializeField] private CustomButton _buttonForChange;
    public CustomButton ButtonForChange => _buttonForChange;

    private GamesConditionChecker _conditionsChecker;
    private ButtonChangeController _changeController;

    protected override void Init()
    {
        base.Init();
        _conditionsChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();
        _changeController = ServiceLocator.Instance.Get<ButtonChangeController>();

        _changeController.AddChangeButton(this);

        SetText();
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.WasteCoinsByProblems(_coinsValue);
    }

    public void SetCoinsValue(int value)
    {
        _coinsValue = value;
    }

    private void SetText()
    {
        _buttonText.text = "Ok";
    }

    public bool CheckChangeCondition()
    {
        return _conditionsChecker.IsEnoughCoins(_coinsValue);
    }

    private void OnDestroy()
    {
        _changeController.RemoveChangeButton(this);
    }
}
