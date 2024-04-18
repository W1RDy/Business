using CoinsCounter;
using System;
using UnityEngine;

public class OrderApplyHandler : ClassForInitialization
{
    private ActiveOrderService _activeOrderService;
    private HandsCoinsCounter _handCoinsCounter;
    private ConfirmHandler _confirmHandler;

    private ButtonService _buttonService;

    public OrderApplyHandler() : base() { }

    public override void Init()
    {
        _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();
        _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _confirmHandler = new ConfirmHandler();
    }

    public void ApplyOrder(IOrderWithCallbacks order)
    {
        var action = GetConfirmRememberedOrderAction(order);

        action.Invoke();
    }

    public void ApplyWithConfitm(IOrderWithCallbacks order)
    {
        var action = GetConfirmRememberedOrderAction(order);
        _confirmHandler.ConfirmAction(action, ConfirmType.SkipTimeAndWasteCoins, order.Time, order.Cost);

    }

    private Action GetConfirmRememberedOrderAction(IOrderWithCallbacks order)
    {
        if (order is Order standartOrder)
        {
            return () =>
            {
                order.ApplyOrder();
                _activeOrderService.AddOrder(order);
            };
        }
        else if (order is CompositeOrder compositeOrder)
        {
            return () =>
            {
                if (_handCoinsCounter.Coins >= order.Cost)
                {
                    _buttonService.CloseWindow(WindowType.BasketWindow);
                    _buttonService.OpenInventoryWindow();
                    order.ApplyOrder();
                }
            };
        }
        else return null;
    }
}
