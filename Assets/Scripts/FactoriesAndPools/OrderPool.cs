using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Pool;

public class OrderPool : IPool<Order>, IService
{
    private int _startPoolSize;

    private List<IPoolElement<Order>> _orderList = new List<IPoolElement<Order>>();

    private OrderFactory _factory;
    private RectTransform _poolContainer;
    private Transform _parent;

    public OrderPool(RectTransform poolContainer, Transform parent, int startPoolSize)
    {
        _poolContainer = poolContainer;
        _parent = parent;

        _factory = new OrderFactory(_poolContainer);
        _factory.LoadResources();

        _startPoolSize = startPoolSize;
    }

    public void Init()
    {
        for (int i = 0; i < _startPoolSize; i++)
        {
            var order = Create();

            if (order as IPoolElement<Order> == null) throw new System.ArgumentException("Order doesn't realize IPoolElement interface!");

            order.Release();
        }
    }

    public Order Create()
    {
        if (_orderList.Count == _startPoolSize) _startPoolSize++;

        var order = _factory.Create() as Order;

        _orderList.Add(order);
        return order;
    }

    public Order Get()
    {
        foreach (var order in _orderList)
        {
            if (order.IsFree)
            {
                order.Element.transform.SetParent(_parent);
                order.Activate();
                return order.Element;
            }
        }

        var newOrder = Create();
        newOrder.transform.SetParent(_parent);
        newOrder.Activate();

        return newOrder;
    }

    public void Release(Order element)
    {
        element.Release();
        element.transform.SetParent(_poolContainer);
    }
}


public class DeliveryOrderPool : IPool<DeliveryOrder>, IService
{
    private int _startPoolSize;

    private List<IPoolElement<DeliveryOrder>> _orderList = new List<IPoolElement<DeliveryOrder>>();

    private DeliveryOrderFactory _factory;
    private RectTransform _poolContainer;
    private Transform _parent;

    public DeliveryOrderPool(RectTransform poolContainer, Transform parent, int startPoolSize)
    {
        _poolContainer = poolContainer;
        _parent = parent;

        _factory = new DeliveryOrderFactory(_poolContainer);
        _factory.LoadResources();

        _startPoolSize = startPoolSize;
    }

    public void Init()
    {
        for (int i = 0; i < _startPoolSize; i++)
        {
            var order = Create();

            if (order as IPoolElement<DeliveryOrder> == null) throw new System.ArgumentException("Order doesn't realize IPoolElement interface!");

            order.Release();
        }
    }

    public DeliveryOrder Create()
    {
        if (_orderList.Count == _startPoolSize) _startPoolSize++;

        var order = _factory.Create() as DeliveryOrder;

        _orderList.Add(order);
        return order;
    }

    public DeliveryOrder Get()
    {
        foreach (var order in _orderList)
        {
            if (order.IsFree)
            {
                order.Element.transform.SetParent(_parent);
                order.Activate();
                return order.Element;
            }
        }

        var newOrder = Create();
        newOrder.transform.SetParent(_parent);
        newOrder.Activate();

        return newOrder;
    }

    public void Release(DeliveryOrder element)
    {
        element.Release();
        element.transform.SetParent(_poolContainer);
    }
}