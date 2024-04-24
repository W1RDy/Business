using System;
using System.Collections.Generic;
using UnityEngine;

public class PCGenerator : ClassForInitialization, IService
{
    private PCService _pcService;

    private Pool<PC> _pool;
    private WindowChildChangedHandler _windowChildChangedHandler;

    private Dictionary<(GoodsType goodsType, bool isBroken, bool isReused), PCConfig> _configsDictionary = new Dictionary<(GoodsType goodsType, bool isBroken, bool isReused), PCConfig>();

    public PCGenerator(PCConfig[] goodsConfigs) : base()
    {
        InitDictionary(goodsConfigs);
    }

    public override void Init()
    {
        _pool = ServiceLocator.Instance.Get<Pool<PC>>();
        _pcService = ServiceLocator.Instance.Get<PCService>();

        var windowService = ServiceLocator.Instance.Get<WindowService>();
        _windowChildChangedHandler = new WindowChildChangedHandler(windowService.GetWindow(WindowType.GoodsWindow));
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
        Action action = () => Generate(goodsType, isBroken, isReused);
        _windowChildChangedHandler.ChangeChilds(action);
    }

    public void GeneratePCByLoadData(PCSaveConfig pcSaveConfig, Action loadCallback)
    {
        var config = GetConfig(pcSaveConfig.id);

        var pc = _pool.Get();
        pc.InitVariant(config, config.IsBroken, pcSaveConfig.amount);

        _pcService.AddPC(pc);
        loadCallback.Invoke();
    }

    private PCConfig GetConfig(int index)
    {
        foreach (var config in _configsDictionary.Values)
        {
            if (config.ID == index) return config;
        }
        throw new System.ArgumentNullException("Config with index " + index + " doesn't exist!");
    }

    private void Generate(GoodsType goodsType, bool isBroken, bool isReused)
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
        Action action = () => GenerateReused(goodsType);
        _windowChildChangedHandler.ChangeChilds(action);
    }

    private void GenerateReused(GoodsType goodsType)
    {
        var newGoodsIndexType = ((int)goodsType - 1);
        if (newGoodsIndexType < 0) GeneratePC(GoodsType.LowQuality, true, true);
        else GeneratePC((GoodsType)newGoodsIndexType, false, true);
    }
}