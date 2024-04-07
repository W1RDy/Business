using System;

public class ResultsCalculator
{ 
    private ResultsOfTheMonthService _resultsOfTheMonthService;
    private Action InitDelegate;

    public ResultsCalculator()
    {
        InitDelegate = () =>
        {
            _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();

            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate?.Invoke();
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
        var resultOfTheGame = new ResultsOfTheGame();

        foreach (var resultOfTheMonth in _resultsOfTheMonthService.GetResults()) 
        {
            var expenses = resultOfTheMonth.EmergencyCosts + resultOfTheMonth.PurchaseCosts;
            var income = resultOfTheMonth.OrderIncome + resultOfTheMonth.BankIncome;

            resultOfTheGame.UpdateResult(expenses, income, 1);
        }

        return resultOfTheGame;
    }
}
