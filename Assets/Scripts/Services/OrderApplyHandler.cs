using CoinsCounter;
using System;
using UnityEngine;

public class OrderApplyHandler
{
    private ActiveOrderService _activeOrderService;
    private ResultsOfTheMonthService _resultsOfTheMonthService;
    private HandsCoinsCounter _handCoinsCounter;
    private SuggestionGenerator _suggestionGenerator;

    private ButtonService _buttonService;

    private Action InitDelegate;
    private Action ConfirmRememberedOrder;

    private Suggestion _rememberedSuggestion;

    public OrderApplyHandler()
    {
        InitDelegate = () =>
        {
            _activeOrderService = ServiceLocator.Instance.Get<ActiveOrderService>();
            _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
            _buttonService = ServiceLocator.Instance.Get<ButtonService>();
            _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
            _suggestionGenerator = ServiceLocator.Instance.Get<SuggestionGenerator>();
            
            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void ApplyOrder(IOrder order)
    {
        RememberOrder(order);

        if (order as Order != null) ConfirmOrderApplying();
        else OpenSuggestionWindow(order);
    }

    private void RememberOrder(IOrder order)
    {
        ConfirmRememberedOrder = GetConfirmRememberedOrderAction(order);
    }

    private void OpenSuggestionWindow(IOrder order)
    {
        _rememberedSuggestion = _suggestionGenerator.GenerateSuggestion("SkipTime", order.Time);

        _rememberedSuggestion.SuggestionApplied += ConfirmOrderApplying;
        _rememberedSuggestion.SuggestionSkipped += CancelOrderApplying;

        _buttonService.OpenWindow(WindowType.SuggestionWindow);
    }

    private void ConfirmOrderApplying()
    {
        ConfirmRememberedOrder?.Invoke();
        CancelOrderApplying();
    }

    private void CancelOrderApplying()
    {
        if (ConfirmRememberedOrder != null)
        {
            ConfirmRememberedOrder = null;

            if (_rememberedSuggestion != null)
            {
                _rememberedSuggestion.SuggestionApplied -= ConfirmOrderApplying;
                _rememberedSuggestion.SuggestionSkipped -= CancelOrderApplying;
                _rememberedSuggestion = null;
            }
        }
    }

    private Action GetConfirmRememberedOrderAction(IOrder order)
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
                    order.ApplyOrder();
                }
            };
        }
        else return null;
    }
}
