using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodController : ClassForInitialization, IService
{
    private BankCoinsCounter _bankCoinsCounter;

    private ResultsActivator _resultsActivator;

    private ResultsOfTheMonthService _resultsOfTheMonthService;

    public PeriodController() : base()
    {

    }

    public override void Init()
    {
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _resultsActivator = ServiceLocator.Instance.Get<ResultsActivator>();

        _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
    }

    public void FinishPeriod()
    {
        var startCoins = _bankCoinsCounter.Coins;
        _bankCoinsCounter.DoubleCoins();
        var endCoins = _bankCoinsCounter.Coins;

        _resultsOfTheMonthService.UpdateResults(0, 0, 0, endCoins - startCoins);

        _resultsActivator.ActivateResultsOfTheMonth();
    }

    public void FinishPeriodWithoutDouble()
    {
        _resultsActivator.ActivateResultsOfTheMonth();
    }
}
