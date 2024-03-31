public class CloseButton : WindowControlButton
{
    protected override void ClickCallback()
    {
        base.ClickCallback();
        CloseWindow();
    }

    private void CloseWindow()
    {
        _buttonService.CloseWindow(_windowType);
    }
}
