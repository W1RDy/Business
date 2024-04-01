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

    public ButtonService() 
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();

        _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();

        _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();
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
        order.CompleteOrder();
        _activeOrderService.RemoveOrder(order);
    }

    public void ApplyOrder(IOrder order)
    {
        order.ApplyOrder();
        _activeOrderService.AddOrder(order);
    }

    public void AddDeliveryOrder(Delivery delivery)
    {
        delivery.AddDeliveryOrder();
    }

    public void ConstructPC(Goods goods)
    {
        goods.ConstructPC();
    }
}
