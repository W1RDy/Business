using System;
using UnityEngine;

public class ServiceLocatorEntitiesLoader : MonoBehaviour
{
    [SerializeField] private RandomController _problemsRandomController;
    [SerializeField] private CompositeOrder _compositeDeliveryOrder;
    [SerializeField] private OrderGenerator _orderGenerator;

    [SerializeField] private ActionInNextFrameActivator _inNextFrameActivator;

    [Header("Pools")]

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

    [Header("Configs")]

    [SerializeField] private GoodsConfig[] _goodsConfigs;
    [SerializeField] private PCConfig[] _pcConfigs;
    [SerializeField] private ProblemConfig[] _problemConfigs;
    [SerializeField] private DeliveryConfig[] _deliveryConfigs;

    public void BindEntities()
    {
        BindActivatorActionInNextFrame();

        BindOrdersServices();
        BindDeliveryCompositeOrder();
        BindOrderGenerator();
        BindDeliveryGenerator();

        BindProblemGenerator();

        BindPools();

        BindGoodsService();
        BindPCService();
    }

    private void BindActivatorActionInNextFrame()
    {
        ServiceLocator.Instance.Register(_inNextFrameActivator);
    }

    private void BindOrderGenerator()
    {
        ServiceLocator.Instance.Register(_orderGenerator);
    }

    private void BindOrdersServices()
    {
        var ordersService = new OrderService();
        var activeOrderService = new ActiveOrderService();
        var deliveryOrderService = new DeliveryOrderService();
        var rememberedOrderService = new RememberedOrderService();

        ServiceLocator.Instance.Register(ordersService);
        ServiceLocator.Instance.Register(activeOrderService);
        ServiceLocator.Instance.Register(deliveryOrderService);
        ServiceLocator.Instance.Register(rememberedOrderService);
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
        ServiceLocator.Instance.Register(_compositeDeliveryOrder);
    }

    private void BindPools()
    {
        var orderPool = new Pool<Order>(new OrderFactory(_orderPoolContainer), _orderPoolContainer, _orderParent, 1);
        orderPool.Init();

        var goalPool = new Pool<Goal>(new GoalFactory(_goalPoolContainer), _goalPoolContainer, _goalParent, 1);
        goalPool.Init();

        var deliveryOrderPool = new Pool<DeliveryOrder>(new DeliveryOrderFactory(_deliveryOrderPoolContainer), _deliveryOrderPoolContainer, _deliveryOrderParent, 3);
        deliveryOrderPool.Init();

        var goodsPool = new Pool<Goods>(new GoodsFactory(_goodsPoolContainer), _goodsPoolContainer, _goodsParent, 1);
        goodsPool.Init();

        var pcPool = new Pool<PC>(new PCFactory(_goodsPoolContainer), _goodsPoolContainer, _goodsParent, 3);
        pcPool.Init();

        ServiceLocator.Instance.Register(orderPool);
        ServiceLocator.Instance.Register(goalPool);
        ServiceLocator.Instance.Register(deliveryOrderPool);
        ServiceLocator.Instance.Register(goodsPool);
        ServiceLocator.Instance.Register(pcPool);
    }

    private void BindProblemGenerator()
    {
        var problemGenerator = new ProblemsGenerator(_problemConfigs, _problemsRandomController);
        ServiceLocator.Instance.Register(problemGenerator);
    }

    private void BindDeliveryGenerator()
    {
        var deliveryGenerator = new DeliveryOrderGenerator(_deliveryConfigs);

        ServiceLocator.Instance.Register(deliveryGenerator);
    }
}