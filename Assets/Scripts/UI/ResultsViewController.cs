﻿using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class ResultsViewController
{
    private TextMeshProUGUI _costsText1;
    private TextMeshProUGUI _costsText2;

    private TextMeshProUGUI _incomeText1;
    private TextMeshProUGUI _incomeText2;

    private TextMeshProUGUI _summaryTimeText;
    private TextMeshProUGUI _summaryCoinsText;

    private TextMeshProUGUI[] _texts;

    private Color _positiveColor;
    private Color _negativeColor;

    public ResultsViewController(TextMeshProUGUI costsText1, TextMeshProUGUI costsText2, TextMeshProUGUI incomeText1, TextMeshProUGUI incomeText2,
        TextMeshProUGUI summaryTimeText, TextMeshProUGUI summaryCoinsText, Color positiveColor, Color negativeColor)
    {
        _costsText1 = costsText1;
        _costsText2 = costsText2;

        _incomeText1 = incomeText1;
        _incomeText2 = incomeText2;

        _summaryTimeText = summaryTimeText;
        _summaryCoinsText = summaryCoinsText;

        _texts = new TextMeshProUGUI[6] { _costsText1, _costsText2, _incomeText1, _incomeText2, _summaryTimeText, _summaryCoinsText };


        _positiveColor = positiveColor;
        _negativeColor = negativeColor;

        _costsText1.color = _negativeColor;
        _costsText2.color = _negativeColor;

        _incomeText1.color = _positiveColor;
        _incomeText2.color = _positiveColor;
    }

    public void SetResults(IResults results)
    {   
        if (results is ResultsOfTheMonth resultsOfTheMonth) ShowResultsOfTheMonth(resultsOfTheMonth);
        else if (results is ResultsOfTheGame resultsOfTheGame) ShowResultsOfTheGame(resultsOfTheGame);
    }

    private void ShowResultsOfTheMonth(ResultsOfTheMonth resultsOfTheMonth)
    {
        _costsText1.text = "Purchase costs: " + resultsOfTheMonth.PurchaseCosts.ToString();
        _costsText2.text = "Emergency costs: " + resultsOfTheMonth.EmergencyCosts.ToString();

        _incomeText1.text = "Orders income: +" + resultsOfTheMonth.OrderIncome.ToString();
        _incomeText2.text = "Bank income: +" + resultsOfTheMonth.BankIncome.ToString();

        var valueSymbol = resultsOfTheMonth.Summary >= 0 ? "+" : "-";
        _summaryCoinsText.text = "Summary: " + valueSymbol + Math.Abs(resultsOfTheMonth.Summary).ToString();

        _summaryCoinsText.color = resultsOfTheMonth.Summary >= 0 ? _positiveColor : _negativeColor;

        TurnOnTexts(_costsText1, _costsText2, _incomeText1, _incomeText2, _summaryCoinsText);
    }

    private void ShowResultsOfTheGame(ResultsOfTheGame resultsOfTheGame)
    {
        _costsText1.text = "Expenses: " + resultsOfTheGame.Expenses.ToString();
        _incomeText1.text = "Income: " + resultsOfTheGame.Income.ToString();
        _summaryTimeText.text = "Time: " + resultsOfTheGame.Time.ToString() + " months";

        TurnOnTexts(_costsText1, _incomeText1, _summaryTimeText);
    }

    private void TurnOnTexts(params TextMeshProUGUI[] turningOnTexts)
    {
        foreach (var text in _texts)
        {
            if (turningOnTexts.Contains(text)) text.transform.parent.gameObject.SetActive(true);
            else text.transform.parent.gameObject.SetActive(false);
        }
    }
}