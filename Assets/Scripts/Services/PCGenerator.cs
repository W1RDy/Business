using System.Collections.Generic;
using UnityEngine;

public class PCGenerator : IService
{
    private PCService _pcService;

    private Pool<PC> _pool;

    private Dictionary<GoodsType, PCConfig> _configsDictionary = new Dictionary<GoodsType, PCConfig>();

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

            _configsDictionary.Add(pcConfigInstance.GoodsType, pcConfigInstance);
        }
    }

    public void GeneratePC(GoodsType goodsType, bool isBroken)
    {
        var pcConfigInstance = _configsDictionary[goodsType];

        var pc = _pcService.GetPC((int)goodsType);

        if (pc != null) pc.Amount++;
        else
        {
            pc = _pool.Get();
            pc.Init(pcConfigInstance, isBroken);

            _pcService.AddPC(pc);
        }
    }
}