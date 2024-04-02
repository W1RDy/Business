public class CloseButton : WindowControlButton
{
    protected override void ClickCallback()
    {
        base.ClickCallback();
        CloseWindow();
    }

    private void CloseWindow()
    {
        if (_windowType == WindowType.PeriodFinish) _buttonService.ClosePeriodWindow();
        else _buttonService.CloseWindow(_windowType);
    }
}
