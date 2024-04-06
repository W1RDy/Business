using System.Collections.Generic;
using UnityEngine;

public class ResultsOfTheMonthService : IService
{
    private List<ResultsOfTheMonth> _results = new List<ResultsOfTheMonth>();

    private ResultsOfTheMonth _currentResults;

    public void UpdateResults(int purchase, int emergency, int orders, int bank)
    {
        _currentResults.UpdateResult(purchase, emergency, orders, bank);
    }

    public void ActivateNewResults()
    {
        if (_currentResults != null) _results.Add(_currentResults);
        _currentResults = new ResultsOfTheMonth();
    }

    public ResultsOfTheMonth GetResultsOfTheMonth()
    {
        return _currentResults;
    }
}

public class ResultsOfTheMonth
{
    public int PurchaseCosts { get; private set; }
    public int EmergencyCosts { get; private set; }
    public int OrderIncome { get; private set; }
    public int BankIncome { get; private set; }

    public ResultsOfTheMonth()
    {
        PurchaseCosts = 0;
        EmergencyCosts = 0;
        OrderIncome = 0;
        BankIncome = 0;
    }

    public void UpdateResult(int purchase, int emergency, int order, int bank)
    {
        PurchaseCosts += purchase;
        EmergencyCosts += emergency;
        OrderIncome += order;
        BankIncome += bank;
    }
}