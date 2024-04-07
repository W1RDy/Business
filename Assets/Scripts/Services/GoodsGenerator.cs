using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoodsGenerator : IService
{
    private GoodsService _goodsService;

    private Pool<Goods> _pool;

    private Dictionary<GoodsType, GoodsConfig> _configsDictionary = new Dictionary<GoodsType, GoodsConfig>();

    public GoodsGenerator(GoodsConfig[] goodsConfigs)
    {
        _pool = ServiceLocator.Instance.Get<Pool<Goods>>();
        _goodsService = ServiceLocator.Instance.Get<GoodsService>();

        InitDictionary(goodsConfigs);
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
        for (int i = 0; i < amount; i++)
        {
            if ((float)counts / amount >= 0.6f) break;

            var randomIndex = Random.Range(0, 101);
            if (randomIndex > 70) counts++;
        }
        return counts;
    }

    public void GenerateGoods(GoodsType goodsType, int amount)
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
