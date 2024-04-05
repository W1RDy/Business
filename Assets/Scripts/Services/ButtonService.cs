using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : IService
{
    private TimeController _timeController;

    private HandsCoinsCounter _handCoinsCounter;
    private CoinsDistributor _coinsDistributor;

    private WindowActivator _windowActivator;

    private ActiveOrderService _activeOrderService;

    private OrderProgressChecker _orderProgressChecker;

    private ResultsOfTheMonthService _resultsOfTheMonthService;

    public ButtonService() 
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();

        _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _coinsDistributor = ServiceLocator.Instance.Get<CoinsDistributor>();

        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();

        _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();
        _orderProgressChecker = ServiceLocator.Instance.Get<OrderProgressChecker>();

        _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
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

    public void ClosePeriodFinishWindow()
    {
        CloseWindow(WindowType.DistributeCoinsWindow);
        _timeController.UpdateMonth();
        _resultsOfTheMonthService.ActivateNewResults();
    }

    public void CloseResultsWindow()
    {
        CloseWindow(WindowType.ResultsOfTheMonth);
        OpenWindow(WindowType.DistributeCoinsWindow);
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

    public void WasteCoinsByProblems(int value)
    {
        RemoveHandsCoins(value);
        CloseWindow(WindowType.ProblemWindow);

        _resultsOfTheMonthService.UpdateResults(0, -value, 0, 0);
    }

    public void DistributeCoins()
    {
        _coinsDistributor.ApplyDistributing();
        if (_timeController.PeriodFinished()) ClosePeriodFinishWindow();
        else CloseWindow(WindowType.DistributeCoinsWindow);
    }

    public void SendOrder(IOrder order)
    {
        _resultsOfTheMonthService.UpdateResults(0, 0, order.Cost, 0);
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
            _resultsOfTheMonthService.UpdateResults(-order.Cost, 0, 0, 0);

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
