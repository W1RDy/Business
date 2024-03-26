using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderService : IService
{
    private Dictionary<int, IOrder> _ordersDict = new Dictionary<int, IOrder>();

    public void AddOrder(IOrder order)
    {
        if (_ordersDict.ContainsKey(order.ID)) throw new System.ArgumentException("Order with id " +  order.ID + " already exists!");
        _ordersDict.Add(order.ID, order);
        Debug.Log(_ordersDict.Count);
    }

    public void RemoveOrder(IOrder order)
    {
        if (!_ordersDict.ContainsKey(order.ID)) throw new System.ArgumentException("Order with id " + order.ID + " doesnt exist!");
        _ordersDict.Remove(order.ID);
    }

    public IOrder GetOrder(int id)
    {
        return _ordersDict[id];
    }
}

public class ActiveOrderService : OrderService
{

}
