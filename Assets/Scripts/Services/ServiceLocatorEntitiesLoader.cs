using System;
using UnityEngine;

public class ServiceLocatorEntitiesLoader : MonoBehaviour
{
    [SerializeField] private RandomController _problemsRandomController;
    [SerializeField] private OrderGenerator _orderGenerator;

    [SerializeField] private ActionInNextFrameActivator _inNextFrameActivator;

    [SerializeField] private DeviceService _deviceService;
    private IEntitiesLinksService _linksService;

    [Header("Configs")]

    [SerializeField] private GoodsConfig[] _goodsConfigs;
    [SerializeField] private PCConfig[] _pcConfigs;
    [SerializeField] private ProblemConfig[] _problemConfigs;
    [SerializeField] private DeliveryConfig[] _deliveryConfigs;

    public void BindEntities()
    {
        _linksService = _deviceService.EntitiesLinksService;

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
        ServiceLocator.Instance.Register(_linksService._compositeDeliveryOrder);
    }

    private void BindPools()
    {
        var orderPool = new Pool<Order>(new OrderFactory(_linksService._orderPoolContainer, _deviceService.IsDesktop), _linksService._orderPoolContainer, _linksService._orderParent, 1);
        orderPool.Init();

        var goalPool = new Pool<Goal>(new GoalFactory(_linksService._goalPoolContainer, _deviceService.IsDesktop), _linksService._goalPoolContainer, _linksService._goalParent, 1);
        goalPool.Init();

        var deliveryOrderPool = new Pool<DeliveryOrder>(new DeliveryOrderFactory(_linksService._deliveryOrderPoolContainer, _deviceService.IsDesktop), _linksService._deliveryOrderPoolContainer, _linksService._deliveryOrderParent, 3);
        deliveryOrderPool.Init();

        var goodsPool = new Pool<Goods>(new GoodsFactory(_linksService._goodsPoolContainer, _deviceService.IsDesktop), _linksService._goodsPoolContainer, _linksService._goodsParent, 1);
        goodsPool.Init();

        var pcPool = new Pool<PC>(new PCFactory(_linksService._goodsPoolContainer, _deviceService.IsDesktop), _linksService._goodsPoolContainer, _linksService._goodsParent, 3);
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