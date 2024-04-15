using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : IService
{
    private TimeController _timeController;
    private PeriodSkipController _gameLifeController;

    private WasteCoinsHandler _wasteCoinsHandler;

    private WindowActivator _windowActivator;

    private OrderProgressChecker _orderProgressChecker;
    private OrderApplyHandler _orderApplyHandler;
    private PCConstructHandler _pcConstructHandler;

    private SuggestionGenerator _suggestionGenerator;

    private ResultsOfTheMonthService _resultsOfTheMonthService;
    private GameController _gameController;

    public ButtonService() 
    {
        _timeController = ServiceLocator.Instance.Get<TimeController>();
        _gameLifeController = ServiceLocator.Instance.Get<PeriodSkipController>();

        _wasteCoinsHandler = new WasteCoinsHandler();

        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();

        _orderProgressChecker = ServiceLocator.Instance.Get<OrderProgressChecker>();
        _orderApplyHandler = new OrderApplyHandler();
        _pcConstructHandler = new PCConstructHandler();

        _suggestionGenerator = ServiceLocator.Instance.Get<SuggestionGenerator>();

        _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();

        _gameController = ServiceLocator.Instance.Get<GameController>();
    }

    public void TryAddTime(int time)
    {
        _suggestionGenerator.GenerateSuggestion(ConfirmType.SkipTime, time);
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

    public void DistributeCoins(CoinsDistributor coinsDistributor)
    {
        DistributeCoinsBySuggestion(null, coinsDistributor);
    }

    public void DistributeCoinsBySuggestion(Suggestion suggestion, CoinsDistributor coinsDistributor)
    {
        coinsDistributor.ApplyDistributing();

        if (_timeController.PeriodFinished()) ClosePeriodFinishWindow();
        else CloseWindow(WindowType.DistributeSuggestionWindow);

        if (suggestion != null) suggestion.Apply();
    }

    public void SendOrder(IOrderWithCallbacks order)
    {
        _resultsOfTheMonthService.UpdateResults(0, 0, order.Cost, 0);
        _orderProgressChecker.CompleteOrder(order as Order);
    }

    public void ApplyOrder(IOrderWithCallbacks order)
    {
        _orderApplyHandler.ApplyOrder(order);
    }

    public void ApplyOrderWithConfirm(IOrderWithCallbacks order)
    {
        _orderApplyHandler.ApplyWithConfitm(order);
    }

    public void AddDeliveryOrder(Delivery delivery)
    {
        delivery.AddDeliveryOrder();
    }

    public void ConstructPC(Goods goods)
    {
        _pcConstructHandler.ConstructPC(goods);
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

    public void RestartGame()
    {
        _gameController.RestartGame();
    }
}
