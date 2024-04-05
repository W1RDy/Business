using UnityEngine;

[CreateAssetMenu(fileName = "Problem", menuName = "Problems/New Problem")]
public class ProblemConfig : ScriptableObject
{
    [SerializeField] private string _problemDescription;
    [SerializeField] private int _coinsRequirement;

    public string ProblemDescription => _problemDescription;
    public int CoinsRequirement => _coinsRequirement;
}