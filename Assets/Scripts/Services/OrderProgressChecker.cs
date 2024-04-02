using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderProgressChecker : IDisposable, IService
{
    private PCService _pcSerivce;
    private ActiveOrderService _activeOrderService;

    private Action<GoodsType> UpdateOrdersDelegate;

    public OrderProgressChecker()
    {
        _pcSerivce = ServiceLocator.Instance.Get<PCService>();
        _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();

        UpdateOrdersDelegate = goodsType => UpdateOrders(goodsType);

        _pcSerivce.ItemsUpdated += UpdateOrdersDelegate;
    }

    public bool IsCanComplete(Order order)
    {
        return _pcSerivce.HasPC((int)order.NeededGoods);
    }

    private void UpdateOrders(GoodsType goodsType)
    {
        foreach (var orderInterface in _activeOrderService.GetOrdersByGoods(goodsType))
        {
            var order = orderInterface as Order;

            var isReadyForComplete = IsCanComplete(order);
            order.ChangeOrderState(isReadyForComplete);
        }
    }

    public void CompleteOrder(Order order)
    {
        if (IsCanComplete(order))
        {
            order.CompleteOrder();
        }
    }

    public void Dispose()
    {
        Debug.Log("Unsubscribe");
        _pcSerivce.ItemsUpdated -= UpdateOrdersDelegate;
    }
}