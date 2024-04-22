using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderService : ClassForInitialization, IService, ISubscribable
{
    protected Dictionary<int, IOrder> _ordersDict = new Dictionary<int, IOrder>();

    protected DataSaver _dataSaver;
    protected SubscribeController _subscribeController;

    protected Action SaveDelegate;

    public OrderService() : base()
    {

    }

    public override void Init()
    {
        _dataSaver = ServiceLocator.Instance.Get<DataSaver>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        Subscribe();
    }

    public virtual void AddOrder(IOrder order)
    {
        if (_ordersDict.ContainsKey(order.ID)) throw new System.ArgumentException("Order with id " +  order.ID + " already exists!");
        _ordersDict.Add(order.ID, order);
    }

    public virtual void RemoveOrder(IOrder order)
    {
        if (!_ordersDict.ContainsKey(order.ID)) throw new System.ArgumentException("Order with id " + order.ID + " doesnt exist!");
        _ordersDict.Remove(order.ID);
    }

    public virtual IOrder GetOrder(int id)
    {
        if (_ordersDict.TryGetValue(id, out var order)) return order;
        return null;
    }

    public int GetOrdersCount() => _ordersDict.Count;

    public IOrder[] GetOrders() => _ordersDict.Values.ToArray();

    public virtual void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        SaveDelegate = () => _dataSaver.SaveOrders(_ordersDict.Values.Select(order => order as Order).ToArray());
        _dataSaver.OnStartSaving += SaveDelegate;
    }

    public void Unsubscribe()
    {
        _dataSaver.OnStartSaving -= SaveDelegate;
    }
}

public class ActiveOrderService : OrderService
{
    public ActiveOrderService() { }

    public override void Subscribe()
    {

    }
}

public class DeliveryOrderService : OrderService
{
    public DeliveryOrderService() : base() { }

    public override void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        SaveDelegate = () => _dataSaver.SaveDeliveryOrders(_ordersDict.Values.Select(order => order as DeliveryOrder).ToArray());
        _dataSaver.OnStartSaving += SaveDelegate;
    }
}

public class RememberedOrderService : IService
{
    private Queue<OrderInstanceConfig> _rememberedOrder = new Queue<OrderInstanceConfig>();

    public void RememberOrder(Order order)
    {
        _rememberedOrder.Enqueue(order.OrderConfig);
    }

    public void ClearLastOrdersExcept(int savingsOrdersCount)
    {
        while (_rememberedOrder.Count > savingsOrdersCount)
        {
            _rememberedOrder.Dequeue();
        } 
    }

    public OrderInstanceConfig PopOrder()
    {
        var order = _rememberedOrder.Dequeue();
        return order;
    }

    public int GetOrdersCount()
    {
        return _rememberedOrder.Count;
    }
}