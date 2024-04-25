using I2.Loc;
using System;
using TMPro;
using UnityEngine;

public class ConfirmEventWithCoinsButton : CustomButton, IButtonWithNewButton
{
    [SerializeField] private TextMeshProUGUI _buttonText;

    [SerializeField] private CustomButton _buttonForChange;
    [SerializeField] private CustomButton _secondButtonForChange;
    [SerializeField] private ChangeCondition[] _changeConditions;

    public ChangeCondition[] ChangeConditions => _changeConditions;
    public CustomButton ButtonForChange {get; private set;}

    private GamesConditionChecker _conditionsChecker;
    private ButtonChangeController _changeController;

    private IEventWithCoinsParameters _coinsEvent;
    private ClicksBlocker _clicksBlocker;

    public override void Init()
    {
        base.Init();
        _conditionsChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();
        _changeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        _clicksBlocker = ServiceLocator.Instance.Get<ClicksBlocker>();

        ButtonForChange = _buttonForChange;
        _changeController.AddChangeButton(this);

        SetText();
    }

    public void SetEvent(IEventWithCoinsParameters coinsEvent)
    {
        _coinsEvent = coinsEvent;
        _changeController.ChangeButtonToNewButton(this);
    }

    private void OnEnable()
    {
        if (_changeController != null) _changeController.ChangeButtonToNewButton(this);
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _clicksBlocker.UnblockClicks();
        _coinsEvent.Apply();
        _buttonService.CloseWindow(WindowType.ProblemWindow);
    }

    private void SetText()
    {
        _buttonText.text = LocalizationManager.GetTranslation("Ok");
    }

    public bool CheckButtonChangeCondition()
    {
        if (_coinsEvent == null) return false;
        if (_secondButtonForChange != null && !_conditionsChecker.IsEnoughCoins(_coinsEvent.CoinsRequirements))
        {
            ButtonForChange = _secondButtonForChange;
            return true;
        }

        //if (ButtonForChange != _buttonForChange) ButtonForChange = _buttonForChange;
        return !_conditionsChecker.IsEnoughCoinsInHands(_coinsEvent.CoinsRequirements);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if ( _changeController != null )
        {
            _changeController.RemoveChangeButton(this);
        }
    }
}
