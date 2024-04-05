public class CloseButton : WindowControlButton
{
    protected override void ClickCallback()
    {
        base.ClickCallback();
        CloseWindow();
    }

    private void CloseWindow()
    {
        if (_windowType == WindowType.DistributeCoinsWindow) _buttonService.ClosePeriodFinishWindow();
        else if (_windowType == WindowType.ResultsOfTheMonth) _buttonService.CloseResultsWindow();
        else _buttonService.CloseWindow(_windowType);
    }
}
