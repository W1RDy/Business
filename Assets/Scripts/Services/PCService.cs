using System;
using System.Collections.Generic;
using UnityEngine;

public class PCService : IService
{
    [SerializeField] private Dictionary<int, PC> _pcDict = new Dictionary<int, PC>();

    public event Action ItemsUpdated;

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
}
