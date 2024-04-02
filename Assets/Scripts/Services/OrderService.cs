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
        if (_ordersDict.TryGetValue(id, out var order)) return order;
        return null;
    }

    public int GetOrdersCount() => _ordersDict.Count;

    public List<IOrder> GetOrdersByGoods(GoodsType goodsType)
    {
        var result = new List<IOrder>();

        foreach (var orderInterface in _ordersDict.Values)
        {
            if (orderInterface is Order order)
            {
                if (order.NeededGoods == goodsType) result.Add(orderInterface);
            }
            else break;
        }

        return result;
    }
}

public class ActiveOrderService : OrderService
{

}

public class DeliveryOrderService : OrderService
{

}
