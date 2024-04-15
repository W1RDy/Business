using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderService : IService
{
    protected Dictionary<int, IOrder> _ordersDict = new Dictionary<int, IOrder>();

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
}

public class ActiveOrderService : OrderService
{

}

public class DeliveryOrderService : OrderService
{

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