using System;
using UnityEngine;

public class ResultsCalculator : ClassForInitialization
{ 
    private ResultsOfTheMonthService _resultsOfTheMonthService;
    private TimeController _timeController;

    public ResultsCalculator () : base () { }

    public override void Init()
    {
        _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
        _timeController = ServiceLocator.Instance.Get<TimeController>();
    }

    public int CalculateSummary(ResultsOfTheMonth results)
    {
        return results.PurchaseCosts + results.EmergencyCosts + results.OrderIncome + results.BankIncome;
    }

    public ResultsOfTheMonth CalculateResultsOfTheMonth()
    {
        var results = _resultsOfTheMonthService.GetResultsOfTheMonth();
        results.SummarizeResults();
        return results;
    }

    public ResultsOfTheGame CalculateResultsOfTheGame()
    {
        var resultOfTheGame = new ResultsOfTheGame(_timeController.CurrentMonth - 1);

        foreach (var resultOfTheMonth in _resultsOfTheMonthService.GetResults()) 
        {
            var expenses = resultOfTheMonth.EmergencyCosts + resultOfTheMonth.PurchaseCosts;
            var income = resultOfTheMonth.OrderIncome + resultOfTheMonth.BankIncome;

            resultOfTheGame.UpdateResult(expenses, income);
        }

        return resultOfTheGame;
    }
}
