using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Problem", menuName = "Problems/New Problem")]
public class ProblemConfig : ScriptableObject, IRandomizable, IEventWithCoinsParameters
{
    [SerializeField] private string _id;
    [SerializeField] private string _description;
    [SerializeField] protected int[] _coinsRequirementInterval;
    protected int _coins;

    [SerializeField] private float _chance;
    [SerializeField] private float _chancesUpdateValue;

    public string ID => _id;
    public string Description => _description;

    public int CoinsRequirements => _coins;

    public float CoinsDifficultyValue { private get; set; }

    public float Chance => _chance;
    public float ChancesUpdateValue => _chancesUpdateValue;
    public bool IsBlocked { get; set; }
    public int BlockedCounts { get; set; }

    public event Action Applied;

    public virtual void Apply()
    {
        Applied?.Invoke();
    }

    public virtual void InitProblemValues()
    {
        SetCoinsParameters(GetValueFromInterval(_coinsRequirementInterval, CoinsDifficultyValue));
    }

    public void SetCoinsParameters(int coins)
    {
        _coins = coins;
    }

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

    private int GetValueFromInterval(int[] interval, float difficultyValue)
    {
        int startIntervalPoint = (int)Math.Round(interval[0] * difficultyValue, MidpointRounding.AwayFromZero);
        int endIntervalPoint = (int)Math.Round(interval[1] * difficultyValue + 1, MidpointRounding.AwayFromZero);

        return Random.Range(startIntervalPoint, endIntervalPoint);
    }
}