using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderGenerator : MonoBehaviour
{
    [SerializeField] private OrderConfig[] _orders;
    [SerializeField] private int _maxOrdersCount;

    [SerializeField] private RandomController _randomController; 

    private Pool<Order> _pool;

    private OrderService _orderService;
    private IDGenerator _idGenerator;

    [SerializeField] private float _timeBetweenGenerate;
    private float _timePassed;

    private void Start()
    {
        _pool = ServiceLocator.Instance.Get<Pool<Order>>();
        _orderService = ServiceLocator.Instance.Get<OrderService>();
        _idGenerator = new IDGenerator();

        InitOrderConfigs();
    }

    private void InitOrderConfigs()
    {
        var orderInstances = new OrderConfig[_orders.Length];

        for (int i = 0; i < orderInstances.Length; i++)
        {
            orderInstances[i] = Instantiate(_orders[i]);
        }
        _randomController.Init(orderInstances);
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;

        if (_timePassed >= _timeBetweenGenerate)
        {
            if (!_randomController.IsBlocked)
            {
                GenerateOrder();
                if (_orderService.GetOrdersCount() == _maxOrdersCount) _randomController.BlockController();
            }
            else if (_orderService.GetOrdersCount() < _maxOrdersCount) _randomController.UnblockController();

            _timePassed = 0;
        }
    }

    private OrderConfig GetOrderConfig()
    {
        var order = _randomController.GetRandomizableWithChances() as OrderConfig;
        _randomController.BlockRandomizable(order);
        return order;
    }

    public void GenerateOrder()
    {
        var config = GetOrderConfig();

        var order = _pool.Get();
        var id = _idGenerator.GetID();

        order.InitVariant(id, config);

        _orderService.AddOrder(order);
    }
}
