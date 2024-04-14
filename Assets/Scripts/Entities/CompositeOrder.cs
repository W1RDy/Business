using CoinsCounter;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompositeOrder : MonoBehaviour, IRemembable, IOrderWithCallbacks, IService
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
    [SerializeField] private OpenDistributeSuggestionButton _openDistributeSuggestionButton;

    private CompositeOrderView _view;

    #endregion

    public bool IsApplied {get; private set;}

    private IIDGenerator _idGenerator;

    private Action InitDelegate;
    public event Action OrderValuesChanged;
    public event Action OrderStateChanged;

    public void InitInstance()
    {
        InitDelegate = () =>
        {
            _view = new CompositeOrderView(_priceText, _timeText);
            _idGenerator = new IDGenerator(2);

            _view.SetView("Delivery", 0, 0);

            ID = _idGenerator.GetID();
            Debug.Log(ID);

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;

            _applyOrderButton.InitializeButton();
            _openDistributeSuggestionButton.InitVariant(this);
        };

        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate?.Invoke();
    }

    public void AddOrder(IOrder order)
    {
        _orders.Add(order);

        Cost += order.Cost;
        Time += order.Time;

        _view.SetView("Delivery", Cost, Time);
        OrderValuesChanged?.Invoke();
        Debug.Log(StateIsChanged());
        if (StateIsChanged()) OrderStateChanged?.Invoke();
    }

    public void RemoveOrder(IOrder order)
    {
        _orders.Remove(order);

        Cost -= order.Cost;
        Time -= order.Time;

        _view.SetView("Delivery", Cost, Time);
        OrderValuesChanged?.Invoke();
        if (StateIsChanged()) OrderStateChanged?.Invoke();
    }

    public void ChangeOrder(int oldCost, int oldTime, IOrder newOrder)
    {
        Debug.Log("ChangeOrder");
        Cost -= oldCost;
        Time -= oldTime;

        Cost += newOrder.Cost;
        Time += newOrder.Time;

        _view.SetView("Delivery", Cost, Time);
        OrderValuesChanged?.Invoke();
        if (StateIsChanged()) OrderStateChanged?.Invoke();
    }

    public void ApplyOrder()
    {
        var ordersCopy = new List<IOrder>(_orders);
        foreach (IOrder order in ordersCopy)
        {
            order.ApplyOrder();
        }
        IsApplied = true;
        OrderStateChanged?.Invoke();
    }

    public void CancelOrder()
    {
        foreach (IOrder order in _orders)
        {
            order.CancelOrder();
        }
        IsApplied = true;
        OrderStateChanged?.Invoke();
    }

    public void CompleteOrder()
    {
        foreach (IOrder order in _orders)
        {
            order.CompleteOrder();
        }
        IsApplied = true;
        OrderStateChanged?.Invoke();
    }

    private bool StateIsChanged()
    {
        var oldAppliedState = IsApplied;
        IsApplied = _orders.Count == 0;
        return oldAppliedState == IsApplied;
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
