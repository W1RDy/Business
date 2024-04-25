using CoinsCounter;
using System;
using UnityEngine;
using YG;

public class Rewarder : ClassForInitialization, IService, ISubscribable
{
    private int _rewardCoins;

    private HandsCoinsCounter _handsCoinsCounter;
    private ContinueHandler _continueHandler;

    private SubscribeController _subscribeController;
    private Action<int> ApplyDelegate;

    public Rewarder() : base()
    {

    }

    public override void Init()
    {
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _continueHandler = new ContinueHandler();
        Subscribe();
    }

    public void ApplyReward(int rewardIndex)
    {
        Debug.Log("ApplyReward");
        if (rewardIndex == 0)
        {
            _handsCoinsCounter.AddCoins(_rewardCoins);
        }
        else
        {
            _continueHandler.Continue();
        }
    }

    public void SetRewardCoins(int rewardCoins)
    {
        _rewardCoins = rewardCoins;
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        ApplyDelegate = index => ApplyReward(index);
        YandexGame.RewardVideoEvent += ApplyDelegate;
    }

    public void Unsubscribe()
    {
        YandexGame.RewardVideoEvent -= ApplyDelegate;
    }
}
