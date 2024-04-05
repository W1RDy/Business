using TMPro;
using UnityEngine;

public class ProblemWindow : Window
{
    private int _coinsCost;

    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private WasteCoinsButton _wasteCoinsButton;

    [SerializeField] private TextMeshProUGUI _problemText;

    public void InitProblem(ProblemConfig problemConfig)
    {
        _coinsCost = problemConfig.CoinsRequirement;

        _problemText.text = problemConfig.ProblemDescription;

        _wasteCoinsButton.SetCoinsValue(_coinsCost);
        _coinsText.text = "-" + _coinsCost;
    }
}
