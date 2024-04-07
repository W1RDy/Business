using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderProgressChecker : IService
{
    private GamesConditionChecker _conditionChecker;

    public OrderProgressChecker()
    {
        _conditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();
    }

    public bool IsCanComplete(Order order)
    {
        return _conditionChecker.IsHasInInventory(order.NeededGoods);
    }

    public void CompleteOrder(Order order)
    {
        if (IsCanComplete(order))
        {
            order.CompleteOrder();
        }
    }
}