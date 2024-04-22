using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PCService : ClassForInitialization, IService, ISubscribable
{
    [SerializeField] private Dictionary<int, PC> _pcDict = new Dictionary<int, PC>();

    public event Action ItemsUpdated;

    private SubscribeController _subscribeController;
    private DataSaver _dataSaver;

    private Action SaveDelegate;

    public PCService() : base() { }

    public override void Init()
    {
        _dataSaver = ServiceLocator.Instance.Get<DataSaver>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        Subscribe();
    }

    public void AddPC(PC pc)
    {
        if (_pcDict.ContainsKey(pc.ID)) throw new System.ArgumentException("PC with id " + pc.ID + " already exists!");
        _pcDict.Add(pc.ID, pc);

        ItemsUpdated?.Invoke();
    }

    public void RemovePC(PC pc)
    {
        if (!_pcDict.ContainsKey(pc.ID)) throw new System.ArgumentException("Goods with id " + pc.ID + " doesn't exist!");
        _pcDict.Remove(pc.ID);

        ItemsUpdated?.Invoke();
    }

    public PC GetPC(int id)
    {
        if (_pcDict.TryGetValue(id, out var pc)) return pc;
        return null;
    }

    public PC GetPCByQuality(GoodsType goodsType)
    {
        int goodsTypeIndex = (int)goodsType;
        PC minSuitableQualityPC = null;

        foreach (var pc in _pcDict.Values)
        {
            if (pc.IsBroken) continue;

            if ((int)pc.QualityType >= goodsTypeIndex && (minSuitableQualityPC == null || (int)pc.QualityType < (int)minSuitableQualityPC.QualityType))
            {
                minSuitableQualityPC = pc;
            }
        }
        return minSuitableQualityPC;
    }

    public bool HasPCByThisGoodsOrOver(GoodsType goodsType)
    {
        int goodsTypeIndex = (int)goodsType;
        foreach (var pc in _pcDict.Values)
        {
            if (pc.IsBroken) continue;
            
            if ((int)pc.QualityType >= goodsTypeIndex) return true;
        }
        return false;
    }

    public virtual void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        SaveDelegate = () => _dataSaver.SavePCS(_pcDict.Values.ToArray());
        _dataSaver.OnStartSaving += SaveDelegate;
    }

    public void Unsubscribe()
    {
        _dataSaver.OnStartSaving -= SaveDelegate;
    }
}
