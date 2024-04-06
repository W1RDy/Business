using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderConfig", menuName = "OrderConfigs/New OrderConfig")]
public class OrderConfig : ScriptableObject, IRandomizable
{
    [SerializeField] private int _cost;
    [SerializeField] private int _time;

    [SerializeField] private float _chance;
    [SerializeField] private int _chancesUpdateValue;

    [SerializeField] private GoodsType _neededGoods;

    public int Cost => _cost;
    public int Time => _time;

    public float Chance => _chance;
    public float ChancesUpdateValue => _chancesUpdateValue;
    public bool IsBlocked { get; set; }
    public int BlockedCounts { get; set; }

    public GoodsType NeededGoods => _neededGoods;

    public void UpdateChance(float changeValue)
    {
        _chance += changeValue;
    }
}
