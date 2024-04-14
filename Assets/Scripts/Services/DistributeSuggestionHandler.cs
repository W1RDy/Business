using System;
using UnityEngine;

public class DistributeSuggestionHandler
{
    private ButtonService _buttonService;
    private ConfirmHandler _confirmHandler;
    private int _distributeTimeSkip;

    public DistributeSuggestionHandler(int distributeTimeSkip)
    {
        _confirmHandler = new ConfirmHandler();
        _distributeTimeSkip = distributeTimeSkip;
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();
    }

    public void OpenDistibuteSuggestion(IOrderWithCallbacks order)
    {
        Action action = () =>
        {
            _buttonService.ApplyOrder(order);
            order.CompleteOrder();
        };
        _confirmHandler.ConfirmAction(action, ConfirmType.DistributeCoins, order.Time + _distributeTimeSkip, order.Cost);
    }

    public void OpenDistibuteSuggestion(IEventWithCoinsParameters _event)
    {
        Action action = () =>
        {
            _buttonService.CloseWindow(WindowType.ProblemWindow);
            _event.Apply();
        };
        _confirmHandler.ConfirmAction(action, ConfirmType.DistributeCoins, _distributeTimeSkip, _event.CoinsRequirements);
    }
}