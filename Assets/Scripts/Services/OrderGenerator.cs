using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderGenerator : MonoBehaviour
{
    [SerializeField] private OrderConfig[] _orders;
    [SerializeField] private int _maxOrdersCount;

    private Pool<Order> _pool;

    private OrderService _orderService;
    private IDGenerator _idGenerator;

    private void Start()
    {
        _pool = ServiceLocator.Instance.Get<Pool<Order>>();
        _orderService = ServiceLocator.Instance.Get<OrderService>();
        _idGenerator = new IDGenerator();
        
        StartCoroutine(GenerateCoroutine());
    }

    private IEnumerator GenerateCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            GenerateOrder();
            if (_orderService.GetOrdersCount() == _maxOrdersCount) break;
        }
    }

    private OrderConfig GetOrderConfig()
    {
        var randomIndex = Random.Range(0, _orders.Length);
        return _orders[randomIndex];
    }

    public void GenerateOrder()
    {
        var order = _pool.Get();
        var id = _idGenerator.GetID();
        Debug.Log(id);

        order.InitVariant(id, GetOrderConfig());

        _orderService.AddOrder(order);
    }
}
