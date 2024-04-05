using CoinsCounter;
using TMPro;
using UnityEngine;

public class ProblemWindow : Window
{
    private int _coinsCost;

    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _problemText;

    [SerializeField] private WasteCoinsButton _wasteCoinsButton;
    [SerializeField] private OpenDistributeCoinsButton _openDistributeCoinsButton;

    private ButtonChanger _buttonChanger;

    private HandsCoinsCounter _coinsCounter;

    public void InitProblem(ProblemConfig problemConfig)
    {
        _coinsCost = problemConfig.CoinsRequirement;

        _problemText.text = problemConfig.ProblemDescription;

        _wasteCoinsButton.SetCoinsValue(_coinsCost);
        _coinsText.text = "-" + _coinsCost;

        _buttonChanger = new ButtonChanger(_wasteCoinsButton, _openDistributeCoinsButton);

        _coinsCounter = ServiceLocator.Instance.Get<HandsCoinsCounter>();
        _coinsCounter.CoinsChanged += ChangeButtonDelegate;
    }

    public void ChangeButtonDelegate()
    {
        ChangeButton(_coinsCounter.Coins >= _coinsCost);
    }

    public void ChangeButton(bool activateDefaultButton)
    {
        _buttonChanger.ChangeButtons(activateDefaultButton);
    }

    public void OnDisable()
    {
        _coinsCounter.CoinsChanged -= ChangeButtonDelegate;
    }
}
