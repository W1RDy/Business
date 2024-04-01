using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsService : IService
{
    [SerializeField] private Dictionary<int, Goods> _goodsList = new Dictionary<int, Goods>();

    public void AddGoods(Goods goods)
    {
        if (_goodsList.ContainsKey(goods.ID)) throw new System.ArgumentException("Goods with id " +  goods.ID + " already exists!"); 
        _goodsList.Add(goods.ID, goods);
    }

    public void RemoveGoods(Goods goods)
    {
        if (!_goodsList.ContainsKey(goods.ID)) throw new System.ArgumentException("Goods with id " + goods.ID + " doesn't exist!");
        _goodsList.Remove(goods.ID);
    }

    public Goods GetGoods(int id)
    {
        if (_goodsList.TryGetValue(id, out var goods)) return goods;
        return null;
    }
}
