using System;
using UnityEngine;

[Serializable]
public class EntitiesSaveConfig
{
    public OrderSaveConfig[] orders;
    public DeliveryOrderSaveConfig[] deliveryOrders;

    public GoodsSaveConfig[] goods;
    public PCSaveConfig[] pcs;

    public ProblemSaveConfig problem;
    public int entitiesSetted;

    public void SetOrders(Order[] orders)
    {
        Debug.Log("SetOrders");
        this.orders = new OrderSaveConfig[orders.Length];
        for (int i = 0; i < orders.Length; i++)
        {
            var order = orders[i];
            this.orders[i] = new OrderSaveConfig(order.ID, order.Cost, order.Time, order.NeededGoods, order.IsApplied, order.RemainWaiting);
        }
        entitiesSetted++;
    }

    public void SetDeliveryOrders(DeliveryOrder[] deliveryOrders)
    {
        Debug.Log("SetDeliveryOrders");
        this.deliveryOrders = new DeliveryOrderSaveConfig[deliveryOrders.Length];
        for (int i = 0; i < deliveryOrders.Length; i++)
        {
            this.deliveryOrders[i] = new DeliveryOrderSaveConfig(deliveryOrders[i].NeededGoods, deliveryOrders[i].Amount);
        }
        entitiesSetted++;
    }

    public void SetGoods(Goods[] goods)
    {
        Debug.Log("SetGoods");
        this.goods = new GoodsSaveConfig[goods.Length];
        for (int i = 0; i < goods.Length; i++)
        {
            this.goods[i] = new GoodsSaveConfig((GoodsType)goods[i].ID, goods[i].Time, goods[i].Amount, goods[i].BrokenGoodsCount);
        }
        entitiesSetted++;
    }

    public void SetPC(PC[] pcs)
    {
        Debug.Log("SetPC");
        this.pcs = new PCSaveConfig[pcs.Length];
        for (int i = 0; i < pcs.Length; i++)
        {
            this.pcs[i] = new PCSaveConfig(pcs[i].ID, pcs[i].Amount);
        }
        entitiesSetted++;
    }

    public void SetProblem(ProblemConfig problem)
    {
        if (problem == null) this.problem = null;
        else
        {
            this.problem = new ProblemSaveConfig(problem.ID, problem.CoinsRequirements);
            if (problem is ProblemWithOrder problemWithOrder) this.problem.problemedOrder = problemWithOrder.ProblemedOrder;
        }
        entitiesSetted++;
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
    public int remainWaiting;

    public OrderSaveConfig(int id, int cost, int time, GoodsType neededGoods, bool isApplied, int remainWaiting)
    {
        this.id = id;
        this.cost = cost;
        this.time = time;
        this.neededGoods = neededGoods;
        this.isApplied = isApplied;
        this.remainWaiting = remainWaiting;
    }
}

[Serializable]
public class GoodsSaveConfig
{
    public GoodsType goodsType;
    public int buildTime;

    public int amount;
    public int brokenGoodsAmount;

    public GoodsSaveConfig(GoodsType goodsType, int buildTime, int amount, int brokenGoodsCount)
    {
        this.goodsType = goodsType;
        this.buildTime = buildTime;

        this.amount = amount;
        this.brokenGoodsAmount = brokenGoodsCount;
    }
}

[Serializable]
public class PCSaveConfig
{
    public int id;
    public int amount;

    public PCSaveConfig(int id, int amount)
    {
        this.id = id;
        this.amount = amount;
    }
}

[Serializable]
public class ProblemSaveConfig
{
    public string id;
    public int coins;

    public OrderInstanceConfig problemedOrder;

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

    public int amount;

    public DeliveryOrderSaveConfig(GoodsType goodsType, int amount)
    {
        this.goodsType = goodsType;
        this.amount = amount;
    }
}

[Serializable]
public class ResultSaveConfig
{
    public int purchaseCosts;
    public int emergencyCosts;
    public int orderIncome;
    public int bankIncome;

    public ResultSaveConfig(int purchaseCosts, int emergencyCosts, int orderIncome, int bankIncome)
    {
        this.purchaseCosts = purchaseCosts;
        this.emergencyCosts = emergencyCosts;
        this.orderIncome = orderIncome;
        this.bankIncome = bankIncome;
    }
}