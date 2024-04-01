using System.Collections.Generic;
using UnityEngine;

public class PCService : IService
{
    [SerializeField] private Dictionary<int, PC> _pcList = new Dictionary<int, PC>();

    public void AddPC(PC pc)
    {
        if (_pcList.ContainsKey(pc.ID)) throw new System.ArgumentException("PC with id " + pc.ID + " already exists!");
        _pcList.Add(pc.ID, pc);
    }

    public void RemovePC(PC pc)
    {
        if (!_pcList.ContainsKey(pc.ID)) throw new System.ArgumentException("Goods with id " + pc.ID + " doesn't exist!");
        _pcList.Remove(pc.ID);
    }

    public PC GetPC(int id)
    {
        if (_pcList.TryGetValue(id, out var pc)) return pc;
        return null;
    }
}
