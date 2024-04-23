using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrderGenerator : ObjectForInitialization, IService, ISubscribable
{
    [SerializeField] private OrderConfig[] _orders;
    [SerializeField] private OrderConfig _tutorialOrderConfig; 
    private OrderConfig[] _orderInstances;

    [SerializeField] private int _maxOrdersCount;
    [SerializeField] private int _maxOrdersCountInPeriod;
    private int _remainOrdersInPeriod;

    private int _maxSpawnCountInRow = 2;
    private (GoodsType orderType, int spawnCountInRow) _currentOrderSpawnInfo = (GoodsType.LowQuality, -1);

    [SerializeField] private RandomController _randomController;

    [SerializeField] private Window _ordersWindow;

    private Pool<Order> _pool;

    private OrderService _orderService;
    private IIDGenerator _idGenerator;
    private GameController _gameController;
    private GamesConditionChecker _gameConditionChecker;

    private WindowChildChangedHandler _windowChangedHandler;

    [SerializeField] private float _timeBetweenGenerate;
    private float _timePassed;

    private DifficultyController _difficultyController;
    private Action _onDifficultyChanged;

    private SubscribeController _subscribeController;

    private bool _isSubscribeToActivating;

    public override void Init()
    {
        _pool = ServiceLocator.Instance.Get<Pool<Order>>();
        _orderService = ServiceLocator.Instance.Get<OrderService>();
        _idGenerator = new IDGeneratorWithMinID(3, 1);

        _difficultyController = ServiceLocator.Instance.Get<DifficultyController>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        _gameController = ServiceLocator.Instance.Get<GameController>();
        _gameConditionChecker = ServiceLocator.Instance.Get<GamesConditionChecker>();

        var windowService = ServiceLocator.Instance.Get<WindowService>();
        _windowChangedHandler = new WindowChildChangedHandler(windowService.GetWindow(WindowType.OrdersWindow));

        InitOrderConfigs();
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
                    _windowChangedHandler.ChangeChilds(GenerateOrder);
                    if (_orderService.GetOrdersCount() + _windowChangedHandler.ActionCount >= _maxOrdersCount)
                    {
                        _randomController.BlockController();
                        return;
                    }
                }
                else if (_orderService.GetOrdersCount() + _windowChangedHandler.ActionCount < _maxOrdersCount) _randomController.UnblockController();

                _timePassed = 0;
            }
        }
    }

    public void ActivateOrderGenerator()
    {
        if (!_gameController.IsStartingTutorial && !_gameConditionChecker.IsPeriodFinished())
        {
            _remainOrdersInPeriod = _maxOrdersCountInPeriod;
            if (_isSubscribeToActivating) ActivatingUnsubscribe();
        }
    }

    private OrderInstanceConfig GetOrderConfig()
    {
        var order = _randomController.GetRandomizableWithChances() as OrderConfig;

        order.InitConfigValues();
        var orderInstance = new OrderInstanceConfig(order.Cost, order.Time, order.NeededGoods);
        CheckSpawnInRow(order);
        return orderInstance;
    }

    private void CheckSpawnInRow(OrderConfig order)
    {
        if (_currentOrderSpawnInfo.spawnCountInRow == -1)
        {
            _currentOrderSpawnInfo = (order.NeededGoods, 1);
            return;
        }

        if (_currentOrderSpawnInfo.orderType != order.NeededGoods)
        {
            _currentOrderSpawnInfo = (order.NeededGoods, 1);
        }
        else
        {
            _currentOrderSpawnInfo.spawnCountInRow ++;
            if (_currentOrderSpawnInfo.spawnCountInRow >= _maxSpawnCountInRow) _randomController.BlockRandomizable(order);
        }
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

    public void GenerateTutorialOrder()
    {
        _tutorialOrderConfig.CostDifficultyValue = _difficultyController.OrdersRewardValue;
        _tutorialOrderConfig.TimeDifficultyValue = _difficultyController.OrdersTimeValue;

        _tutorialOrderConfig.InitConfigValues();
        var config = new OrderInstanceConfig(_tutorialOrderConfig.Cost, _tutorialOrderConfig.Time, _tutorialOrderConfig.NeededGoods);

        var order = _pool.Get();
        var id = _idGenerator.GetID();

        order.InitVariant(id, config, _idGenerator);
        order.UpdateOrderUrgency();

        _orderService.AddOrder(order);
    }

    public void GenerateOrderByLoadData(OrderSaveConfig orderSaveConfig)
    {
        var config = new OrderInstanceConfig(orderSaveConfig.cost, orderSaveConfig.time, orderSaveConfig.neededGoods);

        var order = _pool.Get();
        _idGenerator.BorrowID(orderSaveConfig.id);

        order.InitVariant(orderSaveConfig.id, config, _idGenerator, orderSaveConfig.isApplied, orderSaveConfig.remainTime, orderSaveConfig.remainWaiting);

        _orderService.AddOrder(order);
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

        ActivatingSubscribe();
        _difficultyController.DifficultyChanged += _onDifficultyChanged;
        _onDifficultyChanged.Invoke();
    }

    public void Unsubscribe()
    {
        _difficultyController.DifficultyChanged -= _onDifficultyChanged;
        ActivatingUnsubscribe();
    }

    private void ActivatingSubscribe()
    {
        _isSubscribeToActivating = true;
        _gameController.GameStarted += ActivateOrderGenerator;
        _gameController.TutorialLevelStarted += ActivateOrderGenerator;
    }

    private void ActivatingUnsubscribe()
    {
        _isSubscribeToActivating = false;
        _gameController.GameStarted -= ActivateOrderGenerator;
        _gameController.TutorialLevelStarted -= ActivateOrderGenerator;
    }
}
