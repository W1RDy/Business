using CoinsCounter;
using TMPro;
using UnityEngine;

public class ProblemWindow : Window
{
    private int _coinsCost;
    private HandsCoinsCounter _coinsCounter;

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
        var buttonChanger = new ButtonChanger(_wasteCoinsButton, _openDistributeCoinsButton);

        _problemsViewController = new EventsViewController(_problemText, _timeText, _coinsText, buttonChanger);
        _problemsViewController.SetEvent(problemConfig);

        _coinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _coinsCounter.CoinsChanged += ChangeButtonDelegate;
    }

    public void ChangeButtonDelegate()
    {
        _problemsViewController.ChangeButtons(_coinsCounter.Coins >= _coinsCost);
    }

    public void OnDisable()
    {
        _coinsCounter.CoinsChanged -= ChangeButtonDelegate;
    }
}
