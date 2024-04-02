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

    private CompositeOrderView _view;

    #endregion

    public bool IsApplied {get; private set;}
    private bool _isInitialized;

    private IDGenerator _idGenerator;

    public void Awake()
    {
        if (!_isInitialized) InitializeDependency();
    }

    private void InitializeDependency()
    {
        _isInitialized = true;

        _view = new CompositeOrderView(_priceText, _timeText);
        _idGenerator = new IDGenerator();

        _view.SetView("Delivery", 0, 0);
    }

    public void Init()
    {
        if (!_isInitialized) InitializeDependency();

        ID = _idGenerator.GetID();
    }

    public void AddOrder(IOrder order)
    {
        _orders.Add(order);

        Cost += order.Cost;
        Time += order.Time;

        _view.SetView("Delivery", Cost, Time);
    }

    public void RemoveOrder(IOrder order)
    {
        _orders.Remove(order);

        Cost -= order.Cost;
        Time -= order.Time;

        _view.SetView("Delivery", Cost, Time);
    }

    public void ChangeOrder(int oldCost, int oldTime, IOrder newOrder)
    {
        Cost -= oldCost;
        Time -= oldTime;

        Cost += newOrder.Cost;
        Time += newOrder.Time;

        _view.SetView("Delivery", Cost, Time);
    }

    public void ApplyOrder()
    {
        foreach (IOrder order in _orders)
        {
            order.ApplyOrder();
        }
        IsApplied = _orders[0].IsApplied;
    }

    public void CancelOrder()
    {
        foreach (IOrder order in _orders)
        {
            order.CancelOrder();
        }
        IsApplied = _orders[0].IsApplied;
    }

    public void CompleteOrder()
    {
        foreach (IOrder order in _orders)
        {
            order.CancelOrder();
        }
        IsApplied = _orders[0].IsApplied;
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
        _priceText.text = orderType + " price: " + price;
        _timeText.text = orderType + " time: " + time;
    }
}
