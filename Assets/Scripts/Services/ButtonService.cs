using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : IService
{
    private TimeController _timeController;

    private HandsCoinsCounter _handCoinsCounter;
    private BankCoinsCounter _bankCoinsCounter;

    private WindowActivator _windowActivator;

    public ButtonService() 
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();

        _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
    }

    public void AddTime(int time)
    {
        _timeController.AddTime(time);
    }

    public void ClosePeriodWindow()
    {
        _windowActivator.DeactivateWindow(WindowType.PeriodFinish);
        _timeController.UpdateMonth();
    }

    public void RemoveHandsCoins(int value)
    {
        _handCoinsCounter.RemoveCoins(value);
    }

    public void DebitCoinsFromBank(int value)
    {
        if (_bankCoinsCounter.Coins >= value)
        {
            _bankCoinsCounter.RemoveCoins(value);
            _handCoinsCounter.AddCoins(value);
        }
    }

    public void PutCoinsOnBunk(int value)
    {
        if (_handCoinsCounter.Coins >= value)
        {
            _handCoinsCounter.RemoveCoins(value);
            _bankCoinsCounter.AddCoins(value);
        }
    }
}
