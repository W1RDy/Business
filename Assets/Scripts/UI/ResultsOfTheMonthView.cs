using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ResultsOfTheMonthView : MonoBehaviour
{
    #region Texts

    [Header("Texts")]

    [SerializeField] private TextMeshProUGUI _purchaseCostsText;
    [SerializeField] private TextMeshProUGUI _emergencyCostsText;
    [SerializeField] private TextMeshProUGUI _ordersIncomeText;
    [SerializeField] private TextMeshProUGUI _bankIncomeText;
    [SerializeField] private TextMeshProUGUI _summaryText;

    #endregion

    [Header("Colors")]

    [SerializeField] private Color _positiveColor;
    [SerializeField] private Color _negativeColor;

    private ResultsCalculator _calculator;
    private ResultsOfTheMonthService _resultsOfTheMonthService;

    private void Awake()
    {
        _calculator = new ResultsCalculator();
    }

    public void OnEnable()
    {
        if (_resultsOfTheMonthService == null) _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();

        var results = _resultsOfTheMonthService.GetResultsOfTheMonth();
        var summary = _calculator.CalculateSummary(results);
        ShowResults(results.PurchaseCosts, results.EmergencyCosts, results.OrderIncome, results.BankIncome, summary);
    }

    public void ShowResults(int purchaseCosts, int emergencyCosts, int ordersIncome, int bankIncome, int summary)
    {
        _purchaseCostsText.text = "Purchase costs: " + purchaseCosts.ToString();
        _emergencyCostsText.text = "Emergency costs: " + emergencyCosts.ToString();

        _ordersIncomeText.text = "Orders income: +" + ordersIncome.ToString();
        _bankIncomeText.text = "Bank income: +" + bankIncome.ToString();

        var valueSymbol = summary >= 0 ? "+" : "-";
        _summaryText.text = "Summary: " + valueSymbol + Math.Abs(summary).ToString();

        _purchaseCostsText.color = _negativeColor;
        _emergencyCostsText.color = _negativeColor;

        _ordersIncomeText.color = _positiveColor;
        _bankIncomeText.color = _positiveColor;

        _summaryText.color = summary >= 0 ? _positiveColor : _negativeColor;
    }
}