using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderGenerator : MonoBehaviour, IService, ISubscribable
{
    [SerializeField] private OrderConfig[] _orders;
    private OrderConfig[] _orderInstances;

    [SerializeField] private int _maxOrdersCount;
    [SerializeField] private int _maxOrdersCountInPeriod;
    private int _remainOrdersInPeriod;

    [SerializeField] private RandomController _randomController;

    [SerializeField] private Window _ordersWindow;

    private Pool<Order> _pool;

    private OrderService _orderService;
    private IIDGenerator _idGenerator;

    [SerializeField] private float _timeBetweenGenerate;
    private float _timePassed;

    private DifficultyController _difficultyController;
    private Action _onDifficultyChanged;

    private SubscribeController _subscribeController;

    private void Start()
    {
        _pool = ServiceLocator.Instance.Get<Pool<Order>>();
        _orderService = ServiceLocator.Instance.Get<OrderService>();
        _idGenerator = new IDGeneratorWithMinID(3, 1);
        _difficultyController = ServiceLocator.Instance.Get<DifficultyController>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();

        InitOrderConfigs();
        ActivateOrderGenerator();

        Subscribe();
    }

    private void InitOrderConfigs()
    {
        _orderInstances = new OrderConfig[_orders.Length];

        for (int i = 0; i < _orderInstances.Length; i++)
        {
            _orderInstances[i] = Instantiate(_orders[i]);
        }
        _randomController.Init(_orderInstances);
    }

    private void Update()
    {
        if (_remainOrdersInPeriod > 0)
        {
            _timePassed += Time.deltaTime;

            if (_timePassed >= _timeBetweenGenerate)
            {
                if (!_randomController.IsBlocked)
                {
                    GenerateOrder();
                    if (_orderService.GetOrdersCount() == _maxOrdersCount)
                    {
                        _randomController.BlockController();
                        return;
                    }
                }
                else if (_orderService.GetOrdersCount() < _maxOrdersCount) _randomController.UnblockController();

                _timePassed = 0;
            }
        }
    }

    public void ActivateOrderGenerator()
    {
        _remainOrdersInPeriod = _maxOrdersCountInPeriod;
    }

    private OrderInstanceConfig GetOrderConfig()
    {
        var order = _randomController.GetRandomizableWithChances() as OrderConfig;

        order.InitConfigValues();
        var orderInstance = new OrderInstanceConfig(order.Cost, order.Time, order.NeededGoods);

        _randomController.BlockRandomizable(order);
        return orderInstance;
    }

    public void GenerateOrder()
    {
        var config = GetOrderConfig();

        var order = _pool.Get();
        var id = _idGenerator.GetID();

        order.InitVariant(id, config, _idGenerator);

        _orderService.AddOrder(order);
        _remainOrdersInPeriod--;
    }

    private void ChangeOrderGenerateChancesByDifficulty()
    {
        _randomController.ChangeChances(_difficultyController.OrderChances);
    }

    private void ChangeOrdersValuesByDifficulty()
    {
        foreach (var orderInstance in _orderInstances)
        {
            orderInstance.CostDifficultyValue = _difficultyController.OrdersRewardValue;
            orderInstance.TimeDifficultyValue = _difficultyController.OrdersTimeValue;
        }
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        _onDifficultyChanged = () =>
        {
            ChangeOrderGenerateChancesByDifficulty();
            ChangeOrdersValuesByDifficulty();
        };

        _difficultyController.DifficultyChanged += _onDifficultyChanged;
        _onDifficultyChanged.Invoke();
    }

    public void Unsubscribe()
    {
        _difficultyController.DifficultyChanged -= _onDifficultyChanged;
    }
}
