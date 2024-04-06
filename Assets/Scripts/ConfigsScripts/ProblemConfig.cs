using UnityEngine;

[CreateAssetMenu(fileName = "Problem", menuName = "Problems/New Problem")]
public class ProblemConfig : ScriptableObject, IRandomizable
{
    [SerializeField] private string _problemDescription;
    [SerializeField] private int _coinsRequirement;

    [SerializeField] private float _chance;
    [SerializeField] private float _chancesUpdateValue;

    public string ProblemDescription => _problemDescription;
    public int CoinsRequirement => _coinsRequirement;
    public float Chance => _chance;
    public float ChancesUpdateValue => _chancesUpdateValue;
    public bool IsBlocked { get; set; }
    public int BlockedCounts { get; set; }

    public void UpdateChance(float changeValue)
    {
        _chance += changeValue;
    }
}