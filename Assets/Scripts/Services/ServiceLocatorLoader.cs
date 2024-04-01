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

    #region PoolsTransforms

    [SerializeField] private RectTransform _orderPoolContainer;
    [SerializeField] private Transform _orderParent;

    [SerializeField] private RectTransform _goalPoolContainer;
    [SerializeField] private Transform _goalParent;

    [SerializeField] private RectTransform _deliveryOrderPoolContainer;
    [SerializeField] private Transform _deliveryOrderParent;

    #endregion

    [SerializeField] private CompositeOrder _compositeDeliveryOrder;

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

        BindOrdersServices();
        BindDeliveryCompositeOrder();

        BindPeriodController();
        BindTimeController();

        BindButtonService();

        BindPools();
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

        ServiceLocator.Instance.Register(orderPool);
        ServiceLocator.Instance.Register(goalPool);
        ServiceLocator.Instance.Register(deliveryOrderPool);
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
