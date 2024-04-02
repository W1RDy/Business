using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderConfig", menuName = "OrderConfigs/New OrderConfig")]
public class OrderConfig : ScriptableObject
{
    [SerializeField] private int _cost;
    [SerializeField] private int _time;

    [SerializeField] private GoodsType _neededGoods;

    public int Cost => _cost;
    public int Time => _time;

    public GoodsType NeededGoods => _neededGoods;
}
