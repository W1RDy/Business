using System.Collections.Generic;
using UnityEngine;

public class DeliveryOrderGenerator : ClassForInitialization, IService
{
    private Pool<DeliveryOrder> _pool;

    private DeliveryOrderService _orderService;
    private CompositeOrder _compositeOrder;

    private Dictionary<GoodsType, DeliveryConfig> _configs = new Dictionary<GoodsType, DeliveryConfig>();

    public DeliveryOrderGenerator(DeliveryConfig[] configs) : base()
    {
        InitConfigs(configs);
    }

    public override void Init()
    {
        _pool = ServiceLocator.Instance.Get<Pool<DeliveryOrder>>();
        _orderService = ServiceLocator.Instance.Get<DeliveryOrderService>();
        _compositeOrder = ServiceLocator.Instance.Get<CompositeOrder>();
    }

    private void InitConfigs(DeliveryConfig[] configs)
    {
        for (int i = 0; i < configs.Length; i++)
        {
            _configs.Add(configs[i].GoodsType, ScriptableObject.Instantiate(configs[i]));
        }
    }

    public void GenerateOrder(int id, int cost, int time, GoodsType goodsType, Sprite icon)
    {
        var deliveryOrder = _pool.Get();
        deliveryOrder.InitVariant(id, cost, time, goodsType, icon);
        _orderService.AddOrder(deliveryOrder);

        _compositeOrder.AddOrder(deliveryOrder);
    }

    public void GenerateOrderByLoadData(DeliveryOrderSaveConfig saveConfig)
    {
        var config = _configs[saveConfig.goodsType];

        var deliveryOrder = _pool.Get();
        deliveryOrder.InitVariant((int)config.GoodsType, config.DeliveryCost, config.DeliveryTime, saveConfig.amount, config.GoodsType, config.Icon);
        _orderService.AddOrder(deliveryOrder);

        _compositeOrder.AddOrder(deliveryOrder);
    }
}