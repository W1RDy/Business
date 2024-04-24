using CoinsCounter;
using System;
using System.Collections;
using UnityEngine;

public class ADSButtonActivator : ObjectForInitialization, ISubscribable
{
    private int _maxReachedCoins;

    [SerializeField] private ShowADSForContinueButton _continueButton;
    [SerializeField] private ShowADSForCoins _coinsRewardButton;
    private Rewarder _rewarder;

    private GameController _gameController;
    private HandsCoinsCounter _handsCoinsCounter;
    private BankCoinsCounter _bankCoinsCounter;

    private SubscribeController _subscribeController;

    public ADSButtonActivator() : base() { }

    public override void Init()
    {
        _gameController = ServiceLocator.Instance.Get<GameController>();
        _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _rewarder = ServiceLocator.Instance.Get<Rewarder>();

        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();

        Subscribe();
    }

    public void TryActivateButtonWithCoinsReward()
    {
        var currentCoins = _handsCoinsCounter.Coins + _bankCoinsCounter.Coins;
        Debug.Log(currentCoins);

        if (_maxReachedCoins < currentCoins) _maxReachedCoins = currentCoins;
        if (_maxReachedCoins > currentCoins)
        {
            var divideValue = (float)currentCoins / _maxReachedCoins;
            Debug.Log(divideValue);
            if (divideValue < 0.45f) ActivateButtonWithCoinsReward((int)(((0.5f - divideValue) / divideValue) * currentCoins));  
        }
    }

    public void ActivateButtonWithCoinsReward(int coinsReward)
    {
        _rewarder.SetRewardCoins(coinsReward);
        _coinsRewardButton.SetCoinsView(coinsReward);

        if (!_coinsRewardButton.gameObject.activeSelf) 
        {
            _coinsRewardButton.ActivateButton();
            StartCoroutine(WaitButtonLifeDuration(_coinsRewardButton.Duration, DeactivateButtonWithCoinsReward));
        }
    }

    public void ActivateButtonWithContinueReward()
    {
        _continueButton.ActivateButton();
    }

    public void DeactivateButtonWithContinueReward()
    {
        _continueButton.HideButton();
    }

    public void DeactivateButtonWithCoinsReward()
    {
        if (_coinsRewardButton.gameObject.activeSelf) _coinsRewardButton.HideButton();
    }

    private IEnumerator WaitButtonLifeDuration(float duration, Action callback)
    {
        Debug.Log("StartCoroutine");
        yield return new WaitForSeconds(duration);
        callback.Invoke();
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        _gameController.GameFinished += ActivateButtonWithContinueReward;
        _gameController.GameContinued += DeactivateButtonWithContinueReward;

        _handsCoinsCounter.CoinsChanged += TryActivateButtonWithCoinsReward;
        _bankCoinsCounter.CoinsChanged += TryActivateButtonWithCoinsReward;
    }

    public void Unsubscribe()
    {
        _gameController.GameFinished -= ActivateButtonWithContinueReward;
        _gameController.GameContinued -= DeactivateButtonWithContinueReward;

        _handsCoinsCounter.CoinsChanged -= TryActivateButtonWithCoinsReward;
        _bankCoinsCounter.CoinsChanged -= TryActivateButtonWithCoinsReward;
    }
}