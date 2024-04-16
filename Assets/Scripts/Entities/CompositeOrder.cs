using CoinsCounter;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompositeOrder : ObjectForInitializationWithChildren, IRemembable, IOrderWithCallbacks, IService
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

    [SerializeField] private OpenDistributeSuggestionButton _openDistributeSuggestionButton;

    private CompositeOrderView _view;

    #endregion

    public bool IsApplied {get; private set;}

    private IIDGenerator _idGenerator;

    public event Action OrderValuesChanged;
    public event Action OrderStateChanged;

    public override void Init()
    {
        Debug.Log("Init");
        base.Init();
        _view = new CompositeOrderView(_priceText, _timeText);
        _idGenerator = new IDGenerator(2);

        _view.SetView("Delivery", 0, 0);
        IsApplied = true;
        ID = _idGenerator.GetID();

        _openDistributeSuggestionButton.InitVariant(this);
    }

    public void AddOrder(IOrder order)
    {
        _orders.Add(order);

        Cost += order.Cost;
        if (order.Time > Time) Time = order.Time;

        _view.SetView("Delivery", Cost, Time);
        OrderValuesChanged?.Invoke();
        TryChangeState();
    }

    public void RemoveOrder(IOrder order)
    {
        _orders.Remove(order);

        Cost -= order.Cost;
        if (order.Time == Time) Time = FindMaxTime();

        _view.SetView("Delivery", Cost, Time);
        OrderValuesChanged?.Invoke();
        TryChangeState();
    }

    private int FindMaxTime()
    {
        int time = 0;
        foreach (var order in _orders)
        {
            if (order.Time > time) time = order.Time;
        }
        return time;
    }

    public void ChangeOrder(int oldCost, int oldTime, IOrder newOrder)
    {
        Cost -= oldCost;
        Time -= oldTime;

        Cost += newOrder.Cost;
        Time += newOrder.Time;

        _view.SetView("Delivery", Cost, Time);
        OrderValuesChanged?.Invoke();
        TryChangeState();
    }

    public void ApplyOrder()
    {
        var ordersCopy = new List<IOrder>(_orders);
        foreach (IOrder order in ordersCopy)
        {
            order.ApplyOrder();
        }
        TryChangeState();
    }

    public void CancelOrder()
    {
        foreach (IOrder order in _orders)
        {
            order.CancelOrder();
        }
        TryChangeState();
    }

    public void CompleteOrder()
    {
        foreach (IOrder order in _orders)
        {
            order.CompleteOrder();
        }
        TryChangeState();
    }

    private void TryChangeState()
    {
        var oldAppliedState = IsApplied;
        IsApplied = _orders.Count == 0;
        if (oldAppliedState != IsApplied) OrderStateChanged?.Invoke();
    }
}

public class CompositeOrderView
{
    private TextMeshProUGUI _priceText;
    private TextMeshProUGUI _timeText;

    public CompositeOrderView(TextMeshProUGUI priceText, TextMeshProUGUI timeText)
    {
        _priceText = priceText;
        _timeText = timeText;
    }

    public void SetView(string orderType, int price, int time)
    {
        _priceText.text = orderType + " cost: " + price;
        _timeText.text = orderType + " time: " + time;
    }
}
