using CoinsCounter;
using System;
using UnityEngine;

public class GamesConditionChecker : ClassForInitialization, IService
{
    private int _minCoins = 40;

    private TimeController _timeController;

    private BankCoinsCounter _bankCoinsCounter;
    private HandsCoinsCounter _handsCoinsCounter;
    private GameController _gameController;

    private PCService _pcService;
    public event Action<int> HandsCoinsCheck;

    public GamesConditionChecker() : base() { }

    public override void Init()
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();

        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();
        _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _pcService = ServiceLocator.Instance.Get<PCService>();
        _gameController = ServiceLocator.Instance.Get<GameController>();
    }

    public bool IsGameFinished()
    {
        return _gameController.IsFinished;
    }

    public bool IsEnoughCoinsForMinCoins()
    {
        return _handsCoinsCounter.Coins + _bankCoinsCounter.Coins <= _minCoins;
    }

    public bool IsHasInInventory(GoodsType goodsType)
    {
        return _pcService.HasPCByThisGoodsOrOver(goodsType);
    }

    public bool IsEnoughCoinsInHands(int coins)
    {
        return _handsCoinsCounter.Coins >= Mathf.Abs(coins);
    }

    public bool IsEnoughCoins(int coins)
    {
        return _bankCoinsCounter.Coins + _handsCoinsCounter.Coins  >= coins;
    }

    public bool IsPeriodFinished()
    {
        return _timeController.PeriodFinished();
    }
}