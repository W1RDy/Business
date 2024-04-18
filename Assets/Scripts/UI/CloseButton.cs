using System;

public class CloseButton : WindowControlButton
{
    protected override void ClickCallback()
    {
        base.ClickCallback();
        if (this as ConfirmSuggestionButton == null) CloseWindow();
    }

    protected virtual void CloseWindow()
    {
        if (_windowType == WindowType.Results) _buttonService.CloseResultsWindow();
        else if (_windowType == WindowType.SuggestionWindow || _windowType == WindowType.DistributeSuggestionWindow)
        {
            _buttonService.CancelSuggestion((_window as SuggestionWindow).GetSuggestion());
            _buttonService.CloseWindow(_windowType);
        }
        else _buttonService.CloseWindow(_windowType);
    }
}