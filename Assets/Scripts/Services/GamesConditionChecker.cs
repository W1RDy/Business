using CoinsCounter;
using System;
using UnityEngine;

public class GamesConditionChecker : IService
{
    private int _minCoins = 40;

    private TimeController _timeController;

    private BankCoinsCounter _bankCoinsCounter;
    private HandsCoinsCounter _handsCoinsCounter;

    private PCService _pcService;

    private Action InitDelegate;
    public event Action<int> HandsCoinsCheck;

    public GamesConditionChecker()
    {
        InitDelegate = () =>
        {
            _timeController = ServiceLocator.Instance.Get<TimeController>();

            _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();
            _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
            _pcService = ServiceLocator.Instance.Get<PCService>();

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public bool IsGameFinished()
    {
        return _handsCoinsCounter.Coins + _bankCoinsCounter.Coins <= _minCoins;
    }

    public bool IsHasInInventory(GoodsType goodsType)
    {
        return _pcService.HasPCByThisGoodsOrOver(goodsType);
    }

    public bool IsEnoughCoins(int coins)
    {
        return _handsCoinsCounter.Coins >= Mathf.Abs(coins);
    }

    public bool IsPeriodFinished()
    {
        return _timeController.PeriodFinished();
    }
}