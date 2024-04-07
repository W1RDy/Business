using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : IService
{
    private TimeController _timeController;
    private PeriodSkipController _gameLifeController;

    private HandsCoinsCounter _handCoinsCounter;

    private WindowActivator _windowActivator;

    private OrderProgressChecker _orderProgressChecker;
    private OrderApplyHandler _orderApplyHandler;

    private SuggestionGenerator _suggestionGenerator;

    private ResultsOfTheMonthService _resultsOfTheMonthService;

    public ButtonService() 
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();
        _gameLifeController = ServiceLocator.Instance.Get<PeriodSkipController>();

        _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();

        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();

        _orderProgressChecker = ServiceLocator.Instance.Get<OrderProgressChecker>();
        _orderApplyHandler = new OrderApplyHandler();

        _suggestionGenerator = ServiceLocator.Instance.Get<SuggestionGenerator>();

        _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
    }

    public void TryAddTime(int time)
    {
        _suggestionGenerator.GenerateSuggestion("SkipTime", time);
        OpenWindow(WindowType.SuggestionWindow);
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
        CloseWindow(WindowType.FinishPeriodWindow);
        _gameLifeController.ContinueNewDay();
        _timeController.UpdateMonth();
        _resultsOfTheMonthService.ActivateNewResults();
    }

    public void CloseResultsWindow()
    {
        CloseWindow(WindowType.Results);
        OpenWindow(WindowType.FinishPeriodWindow);
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

    public void DistributeCoins(int time, CoinsDistributor coinsDistributor)
    {
        if (time > 0) TryAddTime(time);
        coinsDistributor.ApplyDistributing();
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
        _orderApplyHandler.ApplyOrder(order);
    }

    public void AddDeliveryOrder(Delivery delivery)
    {
        delivery.AddDeliveryOrder();
    }

    public void ConstructPC(Goods goods)
    {
        goods.ConstructPC();
        TryAddTime(goods.Time);
    }

    public void ThrowOut(IThrowable throwable)
    {
        throwable.ThrowOut();
    }

    public void ConfirmSuggestion(Suggestion suggestion)
    {
        suggestion.Apply();
    }

    public void CancelSuggestion(Suggestion suggestion)
    {
        suggestion.Skip();
    }
}
