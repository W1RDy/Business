using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Problem", menuName = "Problems/New Problem")]
public class ProblemConfig : ScriptableObject, IRandomizable, IEventWithCoinsParameters
{
    [SerializeField] private string _id;
    [SerializeField] private string _description;
    [SerializeField] private int _coinsRequirement;

    [SerializeField] private float _chance;
    [SerializeField] private float _chancesUpdateValue;

    public string ID => _id;
    public string Description => _description;

    public int CoinsRequirements => _coinsRequirement;
    public float Chance => _chance;
    public float ChancesUpdateValue => _chancesUpdateValue;
    public bool IsBlocked { get; set; }
    public int BlockedCounts { get; set; }

    public event Action Applied;

    public void Apply()
    {
        Applied?.Invoke();
    }

    public void SetParameters(int coins)
    {
        _coinsRequirement = coins;
    }

    public void UpdateChance(float changeValue)
    {
        _chance += changeValue;
    }
}