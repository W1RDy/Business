using CoinsCounter;
using System;
using UnityEngine;

public class GamesConditionChecker : IService
{
    private int _minCoins = 40;

    private GameController _gameController;

    private BankCoinsCounter _bankCoinsCounter;
    private HandsCoinsCounter _handsCoinsCounter;

    private PCService _pcService;

    private ButtonChangeController _buttonChangeController;

    private Action InitDelegate;
    public event Action<int> HandsCoinsCheck;

    public GamesConditionChecker()
    {
        InitDelegate = () =>
        {
            _gameController = ServiceLocator.Instance.Get<GameController>();
            _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();
            _handsCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
            _pcService = ServiceLocator.Instance.Get<PCService>();
            _buttonChangeController = ServiceLocator.Instance.Get<ButtonChangeController>();

            _handsCoinsCounter.CoinsChanged += CheckLoseCondition;
            _handsCoinsCounter.CoinsChanged += CheckHandsCoinsConditions;
            _bankCoinsCounter.CoinsChanged += CheckLoseCondition;
            _pcService.ItemsUpdated += CheckInventoryConditions;

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void CheckLoseCondition()
    {
        if (_handsCoinsCounter.Coins + _bankCoinsCounter.Coins <= _minCoins)
        {
            _gameController.FinishGame();
            Unsubscribe();
        }
    }

    public void CheckHandsCoinsConditions()
    {
        _buttonChangeController.ChangeButtonsToDistributeButtons();
    }

    public void CheckInventoryConditions()
    {
        _buttonChangeController.ChangeButtonsToSendButtons();
    }

    public bool IsHasInInventory(GoodsType goodsType)
    {
        return _pcService.HasPC((int)goodsType);
    }

    public bool IsEnoughCoins(int coins)
    {
        return _handsCoinsCounter.Coins >= coins;
    }

    private void Unsubscribe()
    {
        _handsCoinsCounter.CoinsChanged -= CheckLoseCondition;
        _handsCoinsCounter.CoinsChanged -= CheckHandsCoinsConditions;
        _bankCoinsCounter.CoinsChanged -= CheckLoseCondition;
        _pcService.ItemsUpdated -= CheckInventoryConditions;
    }
}
