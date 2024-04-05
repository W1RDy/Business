using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodController : IService
{
    private BankCoinsCounter _bankCoinsCounter;

    private WindowActivator _windowActivator;

    private ResultsOfTheMonthService _resultsOfTheMonthService;

    public PeriodController()
    {
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();

        _resultsOfTheMonthService = ServiceLocator.Instance.Get<ResultsOfTheMonthService>();
    }

    public void FinishPeriod()
    {
        var startCoins = _bankCoinsCounter.Coins;
        _bankCoinsCounter.DoubleCoins();
        var endCoins = _bankCoinsCounter.Coins;

        _resultsOfTheMonthService.UpdateResults(0, 0, 0, endCoins - startCoins);

        _windowActivator.ActivateWindow(WindowType.ResultsOfTheMonth);
    }
}
