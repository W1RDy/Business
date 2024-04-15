﻿using System.Collections.Generic;
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
        _currentResults = new ResultsOfTheMonth();
        _results.Add(_currentResults);
    }

    public ResultsOfTheMonth GetResultsOfTheMonth()
    {
        return _currentResults;
    }

    public List<ResultsOfTheMonth> GetResults() 
    {
        return _results; 
    }
}

public class ResultsOfTheMonth : IResults
{
    public int PurchaseCosts { get; private set; }
    public int EmergencyCosts { get; private set; }
    public int OrderIncome { get; private set; }
    public int BankIncome { get; private set; }

    public int Summary { get; private set; }

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

    public void SummarizeResults()
    {
        CalculateSummary();
    }

    private void CalculateSummary()
    {
        Summary = PurchaseCosts + EmergencyCosts + OrderIncome + BankIncome; 
    }
}

public class ResultsOfTheGame : IResults
{
    public int Expenses { get; private set; }
    public int Income { get; private set; }
    public int Time {  get; private set; }

    public ResultsOfTheGame()
    {
        Expenses = 0;
        Income = 0;
        Time = -1;
    }

    public void UpdateResult(int expenses, int income, int time)
    {
        Expenses += expenses;
        Income += income;
        Time += time;
    }
}