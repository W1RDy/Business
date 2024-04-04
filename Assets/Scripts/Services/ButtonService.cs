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

    private ActiveOrderService _activeOrderService;

    private OrderProgressChecker _orderProgressChecker;

    public ButtonService() 
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();

        _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();

        _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();

        _orderProgressChecker = ServiceLocator.Instance.Get<OrderProgressChecker>();
    }

    public void AddTime(int time)
    {
        _timeController.AddTime(time);
    }

    #region WindowsControl

    public void OpenWindow(WindowType windowType)
    {
        _windowActivator.ActivateWindow(windowType);
    }

    public void CloseWindow(WindowType windowType)
    {
        _windowActivator.DeactivateWindow(windowType);
    }

    public void ClosePeriodWindow()
    {
        CloseWindow(WindowType.PeriodFinish);
        _timeController.UpdateMonth();
    }

    public void OpenInventoryWindow()
    {
        OpenWindow(WindowType.InventoryWindow);
        CloseWindow(WindowType.DeliveryWindow);
    }

    public void OpenDeliveryWindow()
    {
        OpenWindow(WindowType.DeliveryWindow);
        CloseWindow(WindowType.InventoryWindow);
    }

    #endregion

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

    public void SendOrder(IOrder order)
    {
        _orderProgressChecker.CompleteOrder(order as Order);
    }

    public void ApplyOrder(IOrder order)
    {
        if (order as Order != null)
        {
            order.ApplyOrder();
            _activeOrderService.AddOrder(order);
        }
        else if (_handCoinsCounter.Coins >= order.Cost)
        {
            Debug.Log(order.Cost);
            AddTime(order.Time);
            RemoveHandsCoins(order.Cost);
            order.ApplyOrder();
        }
    }

    public void AddDeliveryOrder(Delivery delivery)
    {
        delivery.AddDeliveryOrder();
    }

    public void ConstructPC(Goods goods)
    {
        goods.ConstructPC();
        AddTime(goods.Time);
    }

    public void ThrowOut(IThrowable throwable)
    {
        throwable.ThrowOut();
    }
}
