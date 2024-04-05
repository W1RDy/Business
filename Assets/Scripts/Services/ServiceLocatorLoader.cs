using CoinsCounter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocatorLoader : MonoBehaviour
{
    [SerializeField] private TimeIndicator _timeIndicator;
    [SerializeField] private CoinsIndicator _handsCoinsIndicator;
    [SerializeField] private CoinsIndicator _bankCoinsIndicator;

    [SerializeField] private Window[] _windows;

    [SerializeField] private CoinsDistributor _coinsDistributor;

    #region PoolsTransforms

    [SerializeField] private RectTransform _orderPoolContainer;
    [SerializeField] private Transform _orderParent;

    [SerializeField] private RectTransform _goalPoolContainer;
    [SerializeField] private Transform _goalParent;

    [SerializeField] private RectTransform _deliveryOrderPoolContainer;
    [SerializeField] private Transform _deliveryOrderParent;

    [SerializeField] private RectTransform _goodsPoolContainer;
    [SerializeField] private Transform _goodsParent;

    #endregion

    [SerializeField] private CompositeOrder _compositeDeliveryOrder;

    [SerializeField] private GoodsConfig[] _goodsConfigs;
    [SerializeField] private PCConfig[] _pcConfigs;
    [SerializeField] private ProblemConfig[] _problemConfigs;


    private void Awake()
    {
        Bind();
    }

    private void Bind()
    {
        BindWindowService();
        BindWindowActivator();

        BindCoinsCounters();
        BindRewardHandler();

        BindCoinsDistributor();

        BindOrdersServices();
        BindDeliveryCompositeOrder();

        BindProblemGenerator();
        BindResultsService();

        BindPeriodController();
        BindTimeController();

        BindPools();

        BindGoodsService();
        BindPCService();

        BindOrderProgressChecker();

        BindButtonService();

        ServiceLocator.Instance.RegisterService();
    }

    private void BindProblemGenerator()
    {
        var problemGenerator = new ProblemsGenerator(_problemConfigs);
        ServiceLocator.Instance.Register(problemGenerator);
    }

    private void BindResultsService()
    {
        var resultService = new ResultsOfTheMonthService();
        resultService.ActivateNewResults();

        ServiceLocator.Instance.Register(resultService);
    }

    private void BindCoinsDistributor()
    {
        ServiceLocator.Instance.Register(_coinsDistributor);
    }

    private void BindOrderProgressChecker()
    {
        var orderProgressChecker = new OrderProgressChecker();
        ServiceLocator.Instance.Register(orderProgressChecker);
    }

    private void BindPCService()
    {
        var pcService = new PCService();
        ServiceLocator.Instance.Register(pcService);

        var pcGenerator = new PCGenerator(_pcConfigs);
        ServiceLocator.Instance.Register(pcGenerator);
    }

    private void BindGoodsService()
    {
        var goodsService = new GoodsService();
        ServiceLocator.Instance.Register(goodsService);

        var goodsGenerator = new GoodsGenerator(_goodsConfigs);
        ServiceLocator.Instance.Register(goodsGenerator);
    }

    private void BindDeliveryCompositeOrder()
    {
        _compositeDeliveryOrder.Init();

        ServiceLocator.Instance.Register(_compositeDeliveryOrder);
    }

    private void BindPools()
    {
        var orderPool = new Pool<Order>(new OrderFactory(_orderPoolContainer), _orderPoolContainer, _orderParent, 2);
        orderPool.Init();

        var goalPool = new Pool<Goal>(new GoalFactory(_goalPoolContainer), _goalPoolContainer, _goalParent, 2);
        goalPool.Init();

        var deliveryOrderPool = new Pool<DeliveryOrder>(new DeliveryOrderFactory(_deliveryOrderPoolContainer), _deliveryOrderPoolContainer, _deliveryOrderParent, 3);
        deliveryOrderPool.Init();

        var goodsPool = new Pool<Goods>(new GoodsFactory(_goodsPoolContainer), _goodsPoolContainer, _goodsParent, 3);
        goodsPool.Init();

        var pcPool = new Pool<PC>(new PCFactory(_goodsPoolContainer), _goodsPoolContainer, _goodsParent, 3);
        pcPool.Init();

        ServiceLocator.Instance.Register(orderPool);
        ServiceLocator.Instance.Register(goalPool);
        ServiceLocator.Instance.Register(deliveryOrderPool);
        ServiceLocator.Instance.Register(goodsPool);
        ServiceLocator.Instance.Register(pcPool);
    }

    private void BindRewardHandler()
    {
        var rewardHandler = new RewardHandler();

        ServiceLocator.Instance.Register(rewardHandler);
    }

    private void BindOrdersServices()
    {
        var ordersService = new OrderService();
        var activeOrderService = new ActiveOrderService();
        var deliveryOrderService = new DeliveryOrderService();

        ServiceLocator.Instance.Register(ordersService);
        ServiceLocator.Instance.Register(activeOrderService);
        ServiceLocator.Instance.Register(deliveryOrderService);
    }

    private void BindPeriodController()
    {
        var periodController = new PeriodController();
        ServiceLocator.Instance.Register(periodController);
    }

    private void BindWindowService()
    {
        var windowService = new WindowService(_windows);
        ServiceLocator.Instance.Register(windowService);
    }

    private void BindWindowActivator()
    {
        var windowActivator = new WindowActivator();
        ServiceLocator.Instance.Register(windowActivator);
    }

    private void BindButtonService()
    {
        var buttonService = new ButtonService();
        ServiceLocator.Instance.Register(buttonService);
    }

    private void BindTimeController()
    {
        var timeController = new TimeController(_timeIndicator);
        ServiceLocator.Instance.Register(timeController);
    }

    private void BindCoinsCounters()
    {
        var bankCoinsCounter = new BankCoinsCounter(_bankCoinsIndicator);
        var handCoinsCounter = new HandsCoinsCounter(_handsCoinsIndicator);

        ServiceLocator.Instance.Register(bankCoinsCounter);
        ServiceLocator.Instance.Register(handCoinsCounter);
    }
}
