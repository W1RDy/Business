using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using I2.Loc;

public class ResultsViewController
{
    private TextMeshProUGUI _titleText;

    private TextMeshProUGUI _costsText1;
    private TextMeshProUGUI _costsText2;

    private TextMeshProUGUI _incomeText1;
    private TextMeshProUGUI _incomeText2;

    private TextMeshProUGUI _summaryTimeText;
    private TextMeshProUGUI _summaryCoinsText;

    private TextMeshProUGUI[] _texts;

    private Color _positiveColor;
    private Color _negativeColor;

    private AudioPlayer _audioPlayer;

    public ResultsViewController(TextMeshProUGUI titleText, TextMeshProUGUI costsText1, TextMeshProUGUI costsText2, TextMeshProUGUI incomeText1, TextMeshProUGUI incomeText2,
        TextMeshProUGUI summaryTimeText, TextMeshProUGUI summaryCoinsText, Color positiveColor, Color negativeColor)
    {
        _titleText = titleText;

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

        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
    }

    public void SetResults(IResults results)
    {   
        if (results is ResultsOfTheMonth resultsOfTheMonth) ShowResultsOfTheMonth(resultsOfTheMonth);
        else if (results is ResultsOfTheGame resultsOfTheGame) ShowResultsOfTheGame(resultsOfTheGame);
    }

    private void ShowResultsOfTheMonth(ResultsOfTheMonth resultsOfTheMonth)
    {
        _titleText.text = LocalizationManager.GetTranslation("Results of the month");

        _costsText1.text = LocalizationManager.GetTranslation("Purchase costs") + ": " + resultsOfTheMonth.PurchaseCosts.ToString();
        _costsText2.text = LocalizationManager.GetTranslation("Emergency costs") + ": " + resultsOfTheMonth.EmergencyCosts.ToString();

        _incomeText1.text = LocalizationManager.GetTranslation("Orders income") + ": +" + resultsOfTheMonth.OrderIncome.ToString();
        _incomeText2.text = LocalizationManager.GetTranslation("Bank income") + ": +" + resultsOfTheMonth.BankIncome.ToString();

        var valueSymbol = resultsOfTheMonth.Summary >= 0 ? "+" : "-";
        _summaryCoinsText.text = LocalizationManager.GetTranslation("Summary") + ": " + valueSymbol + Math.Abs(resultsOfTheMonth.Summary).ToString();

        _summaryCoinsText.color = resultsOfTheMonth.Summary >= 0 ? _positiveColor : _negativeColor;

        TurnOnTexts(_costsText1, _costsText2, _incomeText1, _incomeText2, _summaryCoinsText);
        _audioPlayer.PlaySound("FinishPeriod");
    }

    private void ShowResultsOfTheGame(ResultsOfTheGame resultsOfTheGame)
    {
        _titleText.text = LocalizationManager.GetTranslation("Results of the game");

        _costsText1.text = LocalizationManager.GetTranslation("Expences") + ": " + resultsOfTheGame.Expenses.ToString();
        _incomeText1.text = LocalizationManager.GetTranslation("Incomes") + ": +" + resultsOfTheGame.Income.ToString();
        _summaryTimeText.text = LocalizationManager.GetTranslation("Time") + ": " + resultsOfTheGame.Time.ToString() + " " + LocalizationManager.GetTranslation("Months");

        TurnOnTexts(_costsText1, _incomeText1, _summaryTimeText);
        _audioPlayer.PlaySound("FinishPeriod");
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