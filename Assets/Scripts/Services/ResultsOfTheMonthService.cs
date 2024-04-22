using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class ResultsOfTheMonthService : ClassForInitialization, IService, ISubscribable
{
    private List<ResultsOfTheMonth> _results = new List<ResultsOfTheMonth>();
    private ResultsOfTheMonth _currentResults;

    private DataSaver _dataSaver;
    private Action SaveDelegate;

    private SubscribeController _subscribeController;

    public ResultsOfTheMonthService() : base() { }

    public override void Init()
    {
        _dataSaver = ServiceLocator.Instance.Get<DataSaver>();
        _subscribeController = ServiceLocator.Instance.Get<SubscribeController>();
        Subscribe();
    }

    public void UpdateResults(int purchase, int emergency, int orders, int bank)
    {
        _currentResults.UpdateResult(purchase, emergency, orders, bank);
    }

    public void ActivateNewResults()
    {
        _currentResults = new ResultsOfTheMonth();
        _results.Add(_currentResults);
    }

    public void SetResultsByLoadData(List<ResultSaveConfig> resultSaveConfigs)
    {
        foreach (ResultSaveConfig config in resultSaveConfigs)
        {
            var result = new ResultsOfTheMonth();
            result.UpdateResult(config.purchaseCosts, config.emergencyCosts, config.orderIncome, config.bankIncome);
            result.SummarizeResults();

            _results.Add(result);
        }
        _currentResults = _results[_results.Count - 1];
    }

    public ResultsOfTheMonth GetResultsOfTheMonth()
    {
        return _currentResults;
    }

    public List<ResultsOfTheMonth> GetResults() 
    {
        return _results; 
    }

    public void Subscribe()
    {
        _subscribeController.AddSubscribable(this);

        SaveDelegate = () =>
        {
            _dataSaver.SaveResults(_results);
        };
        _dataSaver.OnStartSaving += SaveDelegate;
    }

    public void Unsubscribe()
    {
        _dataSaver.OnStartSaving -= SaveDelegate;
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