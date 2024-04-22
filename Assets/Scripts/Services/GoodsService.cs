using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GoodsService : ClassForInitialization, IService, ISubscribable
{
    [SerializeField] private Dictionary<int, Goods> _goodsDict = new Dictionary<int, Goods>();

    private SubscribeController _subscribeController;
    private DataSaver _dataSaver;

    private Action SaveDelegate;

    public GoodsService() : base() { }

    public override void Init()
    {
        _dataSaver = ServiceLocator.Instance.Get<DataSaver>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        Subscribe();
    }

    public void AddGoods(Goods goods)
    {
        if (_goodsDict.ContainsKey(goods.ID)) throw new System.ArgumentException("Goods with id " +  goods.ID + " already exists!"); 
        _goodsDict.Add(goods.ID, goods);
    }

    public void RemoveGoods(Goods goods)
    {
        if (!_goodsDict.ContainsKey(goods.ID)) throw new System.ArgumentException("Goods with id " + goods.ID + " doesn't exist!");
        _goodsDict.Remove(goods.ID);
    }

    public Goods GetGoods(int id)
    {
        if (_goodsDict.TryGetValue(id, out var goods)) return goods;
        return null;
    }

    public virtual void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        SaveDelegate = () => _dataSaver.SaveGoods(_goodsDict.Values.ToArray());
        _dataSaver.OnStartSaving += SaveDelegate;
    }

    public void Unsubscribe()
    {
        _dataSaver.OnStartSaving -= SaveDelegate;
    }
}
