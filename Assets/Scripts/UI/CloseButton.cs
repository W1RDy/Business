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
        else if (_windowType == WindowType.Results) _buttonService.CloseResultsWindow();
        else if (_windowType == WindowType.SuggestionWindow)
        {
            _buttonService.CancelSuggestion((_window as SuggestionWindow).GetSuggestion());
            _buttonService.CloseWindow(_windowType);
        }
        else _buttonService.CloseWindow(_windowType);
    }
}