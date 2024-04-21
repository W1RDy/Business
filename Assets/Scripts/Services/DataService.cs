using YG;

public class DataService
{
    private EntitiesSaveConfig _entitiesSaveConfig = new EntitiesSaveConfig();

    public void SaveOrders(Order[] orders)
    {
        _entitiesSaveConfig.SetOrders(orders);
    }

    public void SaveDeliveryOrders(DeliveryOrder[] deliveryOrders)
    {
        _entitiesSaveConfig.SetDeliveryOrders(deliveryOrders);
    }

    public void SaveProblem(ProblemConfig problemConfig)
    {
        _entitiesSaveConfig.SetProblem(problemConfig);
    }

    public void SaveGoods(Goods[] goods)
    {
        _entitiesSaveConfig.SetGoods(goods);
    }

    public void SavePCS(PC[] pcs)
    {
        _entitiesSaveConfig.SetPC(pcs);
    }

    public void LoadEntities()
    {
        _entitiesSaveConfig = YandexGame.savesData.entitiesSaveConfig;


    }
}

public class EntitiesLoader
{
    private OrderGenerator _orderGenerator;
    private DeliveryOrderGenerator _deliveryGenerator;

    private GoodsGenerator _goodsGenerator;
    private PCGenerator _pcGenerator;

    private ProblemsGenerator _problemsGenerator;

    public void LoadEntities(EntitiesSaveConfig entitiesSaveConfig)
    {

    }
}