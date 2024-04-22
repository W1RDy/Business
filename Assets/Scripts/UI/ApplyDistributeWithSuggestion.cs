using I2.Loc;
using System;
using TMPro;
using UnityEngine;

public class ApplyDistributeWithSuggestion : CustomButton, IButtonWithStates, ISubscribable
{
    [SerializeField] private CoinsDistributor _coinsDistributor;

    [SerializeField] private TextMeshProUGUI _buttonText;

    private Suggestion _suggestion;
    private ButtonChangeController _changeController;
    private SubscribeController _subscribeController;

    private Action _changeState;

    public ChangeCondition[] ChangeConditions => throw new System.NotImplementedException();

    public override void Init()
    {
        base.Init();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        _changeController = ServiceLocator.Instance.Get<ButtonChangeController>();
        Subscribe();
    }

    public void InitializeVariant(Suggestion suggestion, CoinsDistributor coinsDistributor)
    {
        if (_coinsDistributor == null) _coinsDistributor = coinsDistributor;
        _suggestion = suggestion; 
    }

    protected override void ClickCallback()
    {
        _buttonService.DistributeCoinsBySuggestion(_suggestion, _coinsDistributor);
    }

    public void ChangeStates(bool toActiveState)
    {
        _button.interactable = toActiveState;
        var key = toActiveState ? "Apply" : "Can't apply";
        SetText(key);
    }

    private void SetText(string key)
    {
        _buttonText.text = LocalizationManager.GetTranslation(key);
    }

    public bool CheckStatesChangeCondition()
    {
        if (_suggestion is IEventWithCoinsParameters coinsSuggestion)
        {
            return coinsSuggestion.CoinsRequirements <= _coinsDistributor.GetCurrentHandCoins();
        }
        else if (_suggestion is SuggestionByProblem suggestionByProblem)
        {
            return suggestionByProblem.ProblemCoins <= _coinsDistributor.GetCurrentHandCoins();
        }
        return false;
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        _changeState = () => _changeController.ChangeButtonStates(this);
        _coinsDistributor.OnCoinsTryiedDistribute += _changeState;
        _changeState.Invoke();
    }

    public void Unsubscribe()
    {
        _coinsDistributor.OnCoinsTryiedDistribute -= _changeState;
    }
}