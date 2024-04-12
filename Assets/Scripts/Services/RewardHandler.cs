using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardHandler : IService
{
    private HandsCoinsCounter _coinCounter;

    public RewardHandler()
    {
        _coinCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
    }

    public void ApplyRewardForOrder(IOrderWithCallbacks order)
    {
        _coinCounter.AddCoins(order.Cost);
    }
}
