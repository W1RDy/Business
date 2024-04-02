using System;
using System.Collections.Generic;
using UnityEngine;

public class PCService : IService
{
    [SerializeField] private Dictionary<int, PC> _pcDict = new Dictionary<int, PC>();

    public event Action<GoodsType> ItemsUpdated;

    public void AddPC(PC pc)
    {
        if (_pcDict.ContainsKey(pc.ID)) throw new System.ArgumentException("PC with id " + pc.ID + " already exists!");
        _pcDict.Add(pc.ID, pc);

        ItemsUpdated?.Invoke(pc.QualityType);
    }

    public void RemovePC(PC pc)
    {
        if (!_pcDict.ContainsKey(pc.ID)) throw new System.ArgumentException("Goods with id " + pc.ID + " doesn't exist!");
        _pcDict.Remove(pc.ID);

        ItemsUpdated?.Invoke(pc.QualityType);
    }

    public PC GetPC(int id)
    {
        if (_pcDict.TryGetValue(id, out var pc)) return pc;
        return null;
    }

    public bool HasPC(int id) => _pcDict.ContainsKey(id);
}
