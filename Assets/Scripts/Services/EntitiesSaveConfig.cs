using System;

[Serializable]
public class EntitiesSaveConfig
{
    public OrderSaveConfig[] orders;
    public DeliveryOrderSaveConfig[] deliveryOrders;

    public GoodsSaveConfig[] goods;
    public PCSaveConfig[] pcs;

    public ProblemSaveConfig problem;

    public void SetOrders(Order[] orders)
    {
        this.orders = new OrderSaveConfig[orders.Length];
        for (int i = 0; i < orders.Length; i++)
        {
            var order = orders[i];
            this.orders[i] = new OrderSaveConfig(order.ID, order.Cost, order.Time, order.NeededGoods, order.IsApplied);
        }
    }

    public void SetDeliveryOrders(DeliveryOrder[] deliveryOrders)
    {
        this.deliveryOrders = new DeliveryOrderSaveConfig[deliveryOrders.Length];
        for (int i = 0; i < deliveryOrders.Length; i++)
        {
            this.deliveryOrders[i] = new DeliveryOrderSaveConfig(deliveryOrders[i].NeededGoods);
        }
    }

    public void SetGoods(Goods[] goods)
    {
        this.goods = new GoodsSaveConfig[goods.Length];
        for (int i = 0; i < goods.Length; i++)
        {
            this.goods[i] = new GoodsSaveConfig((GoodsType)goods[i].ID, goods[i].Time);
        }
    }

    public void SetPC(PC[] pcs)
    {
        this.pcs = new PCSaveConfig[pcs.Length];
        for (int i = 0; i < pcs.Length; i++)
        {
            this.pcs[i] = new PCSaveConfig(pcs[i].ID);
        }
    }

    public void SetProblem(ProblemConfig problem)
    {
        this.problem = new ProblemSaveConfig(problem.ID, problem.CoinsRequirements);
    }
}

[Serializable]
public class OrderSaveConfig
{
    public int id;
    public int cost;
    public int time;
    public GoodsType neededGoods;

    public bool isApplied;

    public OrderSaveConfig(int id, int cost, int time, GoodsType neededGoods, bool isApplied)
    {
        this.id = id;
        this.cost = cost;
        this.time = time;
        this.neededGoods = neededGoods;
        this.isApplied = isApplied;
    }
}

[Serializable]
public class GoodsSaveConfig
{
    public GoodsType goodsType;
    public int buildTime;

    public GoodsSaveConfig(GoodsType goodsType, int buildTime)
    {
        this.goodsType = goodsType;
        this.buildTime = buildTime;
    }
}

[Serializable]
public class PCSaveConfig
{
    public int id;

    public PCSaveConfig(int id)
    {
        this.id = id;
    }
}

[Serializable]
public class ProblemSaveConfig
{
    public string id;
    public int coins;

    public ProblemSaveConfig(string id, int coins)
    {
        this.id = id;
        this.coins = coins;
    }
}

[Serializable]
public class DeliveryOrderSaveConfig
{
    public GoodsType goodsType;

    public DeliveryOrderSaveConfig(GoodsType goodsType)
    {
        this.goodsType = goodsType;
    }
}