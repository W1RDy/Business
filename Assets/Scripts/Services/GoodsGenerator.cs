using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoodsGenerator : ClassForInitialization, IService
{
    private GoodsService _goodsService;

    private Pool<Goods> _pool;
    private int _lowPCAmounts;
    private int _brokenPCAmounts;

    private WindowChildChangedHandler _windowChildChangedHandler;

    private Dictionary<GoodsType, GoodsConfig> _configsDictionary = new Dictionary<GoodsType, GoodsConfig>();

    public GoodsGenerator(GoodsConfig[] goodsConfigs) : base()
    {
        InitDictionary(goodsConfigs);
    }

    public override void Init()
    {
        _pool = ServiceLocator.Instance.Get<Pool<Goods>>();
        _goodsService = ServiceLocator.Instance.Get<GoodsService>();

        var windowService = ServiceLocator.Instance.Get<WindowService>();
        _windowChildChangedHandler = new WindowChildChangedHandler(windowService.GetWindow(WindowType.GoodsWindow));
    }

    private void InitDictionary(GoodsConfig[] goodsConfigs)
    {
        foreach (var goodsConfig in goodsConfigs)
        {
            var goodsConfigInstance = ScriptableObject.Instantiate(goodsConfig);

            _configsDictionary.Add(goodsConfigInstance.GoodsType, goodsConfigInstance);
        }
    }

    private int GetRandomBrokenGoodsCount(GoodsType goodsType, int amount)
    {
        if (goodsType != GoodsType.LowQuality) return 0;

        int counts = 0;
        _lowPCAmounts += amount;

        if (_lowPCAmounts < 4) return 0;

        for (int i = 0; i < amount; i++)
        {
            if ((float)counts / amount >= 0.2f) break;
            else if (_brokenPCAmounts / _lowPCAmounts >= 0.4f) break;

            var randomIndex = Random.Range(0, 101);
            if (randomIndex > 80)
            {
                counts++;
                _brokenPCAmounts++;
            }
        }
        return counts;
    }

    public void GenerateGoods(GoodsType goodsType, int amount)
    {
        Action action = () =>
        {
            Generate(goodsType, amount);
        };
        _windowChildChangedHandler.ChangeChilds(action);
    }

    public void GenerateGoodsByLoadData(GoodsSaveConfig goodsSaveConfig, Action loadCallback)
    {
        var goodsConfig = _configsDictionary[goodsSaveConfig.goodsType];

        var goods = _pool.Get();
        goods.InitVariant(goodsConfig, goodsSaveConfig.brokenGoodsAmount, goodsSaveConfig.amount);

        _goodsService.AddGoods(goods);
        loadCallback.Invoke();
    }

    private void Generate(GoodsType goodsType, int amount)
    {
        var goodsConfigInstance = _configsDictionary[goodsType];
        var brokenGoodsCount = GetRandomBrokenGoodsCount(goodsType, amount);

        var goods = _goodsService.GetGoods((int)goodsType);

        if (goods != null)
        {
            Debug.Log("Increase Goods");
            goods.Amount += amount;
        }
        else
        {
            goods = _pool.Get();
            goods.InitVariant(goodsConfigInstance, brokenGoodsCount, amount);

            _goodsService.AddGoods(goods);
        }
    }
}
