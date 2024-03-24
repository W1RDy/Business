using CoinsCounter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodController : IService
{
    private BankCoinsCounter _bankCoinsCounter;

    private WindowActivator _windowActivator;

    public PeriodController()
    {
        _bankCoinsCounter = ServiceLocator.Instance.Get<BankCoinsCounter>();

        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
    }

    public void FinishPeriod()
    {
        _bankCoinsCounter.DoubleCoins();
        _windowActivator.ActivateWindow(WindowType.PeriodFinish);
    }
}
