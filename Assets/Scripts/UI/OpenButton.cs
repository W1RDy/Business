public class OpenButton : WindowControlButton
{
    protected override void ClickCallback()
    {
        base.ClickCallback();
        OpenWindow();
    }

    protected virtual void OpenWindow()
    {
        _buttonService.OpenWindow(_windowType);
    }
}
