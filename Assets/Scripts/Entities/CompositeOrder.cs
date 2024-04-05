using CoinsCounter;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompositeOrder : MonoBehaviour, IOrder, IService
{
    private List<IOrder> _orders = new List<IOrder>();

    #region Values

    public int ID { get; private set; }
    public int Cost { get; private set; }
    public int Time { get; private set; }

    #endregion

    #region View

    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private ApplyOrderButton _applyOrderButton;
    [SerializeField] private OpenDistributeCoinsButton _openDistributeCoinsButton;

    private CompositeOrderView _view;

    #endregion

    public bool IsApplied {get; private set;}

    private IDGenerator _idGenerator;
    private HandsCoinsCounter _handCoinsCounter;

    private Action InitDelegate;

    public void InitInstance()
    {
        InitDelegate = () =>
        {
            _view = new CompositeOrderView(_priceText, _timeText, _applyOrderButton, _openDistributeCoinsButton);
            _idGenerator = new IDGenerator();

            _handCoinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
            _handCoinsCounter.CoinsChanged += ChangeButtons;

            _view.SetView("Delivery", 0, 0);

            ID = _idGenerator.GetID();

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };

        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate?.Invoke();
    }

    public void ChangeButtons()
    {
        _view.ChangeButtons(_handCoinsCounter.Coins >= Cost);
    }

    public void AddOrder(IOrder order)
    {
        _orders.Add(order);

        Cost += order.Cost;
        Time += order.Time;

        ChangeButtons();
        _view.SetView("Delivery", Cost, Time);
        _view.ChangeState(false);
    }

    public void RemoveOrder(IOrder order)
    {
        _orders.Remove(order);

        Cost -= order.Cost;
        Time -= order.Time;

        ChangeButtons();
        _view.SetView("Delivery", Cost, Time);
    }

    public void ChangeOrder(int oldCost, int oldTime, IOrder newOrder)
    {
        Cost -= oldCost;
        Time -= oldTime;

        Cost += newOrder.Cost;
        Time += newOrder.Time;

        ChangeButtons();
        _view.SetView("Delivery", Cost, Time);
        _view.ChangeState(false);
    }

    public void ApplyOrder()
    {
        var ordersCopy = new List<IOrder>(_orders);
        foreach (IOrder order in ordersCopy)
        {
            order.ApplyOrder();
        }
        IsApplied = _orders.Count > 0;
        _view.ChangeState(true);
    }

    public void CancelOrder()
    {
        foreach (IOrder order in _orders)
        {
            order.CancelOrder();
        }
        IsApplied = false;
    }

    public void CompleteOrder()
    {
        foreach (IOrder order in _orders)
        {
            order.CompleteOrder();
        }
        IsApplied = false;
    }

    public void OnDestroy()
    {
        _handCoinsCounter.CoinsChanged -= ChangeButtons;
    }
}

public class CompositeOrderView
{
    private TextMeshProUGUI _priceText;
    private TextMeshProUGUI _timeText;

    private ApplyOrderButton _applyButton;
    private OpenDistributeCoinsButton _openDistributeButton;

    ButtonChanger _buttonsChanger;

    public CompositeOrderView(TextMeshProUGUI priceText, TextMeshProUGUI timeText, ApplyOrderButton applyOrderButton, OpenDistributeCoinsButton openDistributeCoinsButton)
    {
        _priceText = priceText;
        _timeText = timeText;

        _applyButton = applyOrderButton;
        _openDistributeButton = openDistributeCoinsButton;

        _buttonsChanger = new ButtonChanger(_applyButton, _openDistributeButton);
    }

    public void SetView(string orderType, int price, int time)
    {
        _priceText.text = orderType + " cost: " + price;
        _timeText.text = orderType + " time: " + time;
    }

    public void ChangeState(bool isApplied)
    {
        _applyButton.ChangeState(isApplied);
    }

    public void ChangeButtons(bool activateDefaultButton)
    {
        _buttonsChanger.ChangeButtons(activateDefaultButton);
    }
}
