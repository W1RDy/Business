using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : IService
{
    private TimeController _timeController;

    private HandsCoinsCounter _handCoinsCounter;
    private BankCoinsCounter _bankCoinsCounter;

    public ButtonService() 
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();

        _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();
    }

    public void AddTime(int time)
    {
        _timeController.AddTime(time);
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
}
