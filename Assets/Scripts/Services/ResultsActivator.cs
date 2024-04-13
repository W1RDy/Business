using System;

public class ResultsActivator : IService
{
    private ResultsWindow _resultsWindow;
    private ResultsCalculator _calculator;
    private WindowActivator _windowActivator;

    private Action InitDelegate;

    public ResultsActivator()
    {
        _calculator = new ResultsCalculator();

        InitDelegate = () =>
        {
            _resultsWindow = ServiceLocator.Instance.Get<WindowService>().GetWindow(WindowType.Results) as ResultsWindow;
            _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
            ServiceLocator.Instance.ServiceRegistered -= InitDelegate;
        };
        ServiceLocator.Instance.ServiceRegistered += InitDelegate;
        if (ServiceLocator.Instance.IsRegistered) InitDelegate.Invoke();
    }

    public void ActivateResultsOfTheMonth()
    {
        var result = _calculator.CalculateResultsOfTheMonth();
        _resultsWindow.SetResults(result);
        _windowActivator.ActivateWindow(WindowType.Results);
    }

    public void ActivateResultsOfTheGame()
    {
        var result = _calculator.CalculateResultsOfTheGame();
        _resultsWindow.SetResults(result);
        _windowActivator.ActivateWindow(WindowType.Results);
    }
}
