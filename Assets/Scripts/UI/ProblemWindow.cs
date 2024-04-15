using CoinsCounter;
using TMPro;
using UnityEngine;

public class ProblemWindow : Window
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _problemText;

    [SerializeField] private ProblemButton _problemButton;
    [SerializeField] private OpenDistributeSuggestionButton _openDistributeCoinsButton;

    private EventsViewController _problemsViewController;

    public void SetProblem(ProblemConfig problemConfig)
    {
        _problemButton.SetProblem(problemConfig);

        _openDistributeCoinsButton.InitVariant(problemConfig);

        _problemsViewController = new EventsViewController(_problemText, _timeText, _coinsText);
        _problemsViewController.SetEvent(problemConfig);
    }
}
