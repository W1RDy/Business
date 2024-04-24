﻿using System;
using UnityEngine;

public class ResultsActivator : ClassForInitialization, IService
{
    private ResultsWindow _resultsWindow;
    private ResultsCalculator _calculator;
    private WindowActivator _windowActivator;

    public ResultsActivator() : base() { }

    public override void Init()
    {
        _calculator = new ResultsCalculator();
        _resultsWindow = ServiceLocator.Instance.Get<WindowService>().GetWindow(WindowType.Results) as ResultsWindow;
        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
    }

    public void ActivateResultsOfTheMonth()
    {
        var result = _calculator.CalculateResultsOfTheMonth();
        _resultsWindow.SetResults(result);
        _windowActivator.ActivateWindow(WindowType.Results);
    }

    public void ActivateResultsOfTheGame()
    {
        Debug.Log("ActivateLose");
        var result = _calculator.CalculateResultsOfTheGame();
        _resultsWindow.SetResults(result);
        _windowActivator.ActivateWindow(WindowType.Results);
    }
}
