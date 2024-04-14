using CoinsCounter;
using System;
using UnityEngine;

public class OrderApplyHandler
{
    private ActiveOrderService _activeOrderService;
    private ResultsOfTheMonthService _resultsOfTheMonthService;
    private HandsCoinsCounter _handCoinsCounter;

    private ButtonService _buttonService;

    private Action InitDelegate;

    private ConfirmHandler _confirmHandler;

    public OrderApplyHandler()
    {
        InitDelegate = () =>
        {
            _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();
            _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
            _buttonService = ServiceLocator.Instance.Get<ButtonService>();
            _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
            _confirmHandler = new ConfirmHandler();
            
            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void ApplyOrder(IOrderWithCallbacks order)
    {
        var action = GetConfirmRememberedOrderAction(order);

        if (order is Order standartOrder) action.Invoke();
        else _confirmHandler.ConfirmAction(action, order.Time);
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
                    _resultsOfTheMonthService.UpdateResults(-order.Cost, 0, 0, 0);
                    _buttonService.RemoveHandsCoins(order.Cost);
                    _buttonService.CloseWindow(WindowType.BasketWindow);
                    order.ApplyOrder();
                }
            };
        }
        else return null;
    }
}
