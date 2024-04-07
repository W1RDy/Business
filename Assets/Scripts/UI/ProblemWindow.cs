using CoinsCounter;
using TMPro;
using UnityEngine;

public class ProblemWindow : Window
{
    private int _coinsCost;

    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _problemText;

    [SerializeField] private WasteCoinsButton _wasteCoinsButton;
    [SerializeField] private OpenDistributeCoinsButton _openDistributeCoinsButton;

    private EventsViewController _problemsViewController;

    public void SetProblem(ProblemConfig problemConfig)
    {
        _coinsCost = problemConfig.CoinsRequirements;
        _wasteCoinsButton.SetCoinsValue(_coinsCost);

        _problemsViewController = new EventsViewController(_problemText, _timeText, _coinsText);
        _problemsViewController.SetEvent(problemConfig);
    }
}
