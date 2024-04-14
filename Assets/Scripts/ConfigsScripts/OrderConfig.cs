using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "OrderConfig", menuName = "OrderConfigs/New OrderConfig")]
public class OrderConfig : ScriptableObject, IRandomizable
{
    [SerializeField] private int[] _costInterval;
    [SerializeField] private int[] _timeInterval;

    private int _cost;
    private int _time;

    [SerializeField] private float _chance;
    [SerializeField] private float _chancesUpdateValue;

    [SerializeField] private GoodsType _neededGoods;

    public int Cost => _cost;
    public int Time => _time;

    public float Chance => _chance;
    public float ChancesUpdateValue => _chancesUpdateValue;

    public float CostDifficultyValue { private get; set; }
    public float TimeDifficultyValue { private get; set; }


    public bool IsBlocked { get; set; }
    public int BlockedCounts { get; set; }

    public GoodsType NeededGoods => _neededGoods;

    public void UpdateChance(float changeValue)
    {
        _chance += changeValue;
    }

    public void ChangeChance(float newChance)
    {
        float chanceUpdateValueDivide = _chance / _chancesUpdateValue;
        _chance = newChance;
        _chancesUpdateValue = Mathf.Floor(_chance / chanceUpdateValueDivide);
    }

    public void InitConfigValues()
    {
        _cost = GetValueFromInterval(_costInterval, CostDifficultyValue);
        _time = GetValueFromInterval(_timeInterval, TimeDifficultyValue);
    }

    private int GetValueFromInterval(int[] interval, float difficultyValue)
    {
        int startIntervalPoint = (int)Math.Round(interval[0] * difficultyValue, MidpointRounding.AwayFromZero);
        int endIntervalPoint = (int)Math.Round(interval[1] * difficultyValue + 1, MidpointRounding.AwayFromZero);

        return Random.Range(startIntervalPoint, endIntervalPoint);
    }
}

public class OrderInstanceConfig
{
    public int Cost {get; private set; }
    public int Time {get; private set; }

    public GoodsType NeededGoods { get; private set; }

    public OrderInstanceConfig(int cost, int time, GoodsType neededGoods)
    {
        Cost = cost;
        Time = time;
        NeededGoods = neededGoods;
    }
}