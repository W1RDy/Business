using System.Collections.Generic;
using UnityEngine;

public class PCGenerator : IService
{
    private PCService _pcService;

    private Pool<PC> _pool;

    private Dictionary<(GoodsType goodsType, bool isBroken, bool isReused), PCConfig> _configsDictionary = new Dictionary<(GoodsType goodsType, bool isBroken, bool isReused), PCConfig>();

    public PCGenerator(PCConfig[] goodsConfigs)
    {
        _pool = ServiceLocator.Instance.Get<Pool<PC>>();
        _pcService = ServiceLocator.Instance.Get<PCService>();

        InitDictionary(goodsConfigs);
    }

    private void InitDictionary(PCConfig[] pcConfigs)
    {
        foreach (var pcConfig in pcConfigs)
        {
            var pcConfigInstance = ScriptableObject.Instantiate(pcConfig);

            _configsDictionary.Add((pcConfigInstance.GoodsType, pcConfigInstance.IsBroken, pcConfigInstance.IsReturned), pcConfigInstance);
        }
    }

    public void GeneratePC(GoodsType goodsType, bool isBroken, bool isReused)
    {
        if (isBroken) goodsType = GoodsType.LowQuality;
        var pcConfigInstance = _configsDictionary[(goodsType, isBroken, isReused)];

        var pc = _pcService.GetPC(pcConfigInstance.ID);

        if (pc != null) pc.Amount++;
        else
        {
            pc = _pool.Get();
            pc.InitVariant(pcConfigInstance, isBroken);

            _pcService.AddPC(pc);
        }
    }

    public void GenerateReusedPC(GoodsType goodsType)
    {
        var newGoodsIndexType = ((int)goodsType - 1);
        if (newGoodsIndexType < 0) GeneratePC(GoodsType.LowQuality, true, true);
        else GeneratePC((GoodsType)newGoodsIndexType, false, true);
    }
}