public class OpenButton : WindowControlButton
{
    protected override void ClickCallback()
    {
        base.ClickCallback();
        OpenWindow();
    }

    private void OpenWindow()
    {
        switch (_windowType)
        {
            case WindowType.PeriodFinish:
                break;
            case WindowType.OrdersWindow:
                _buttonService.OpenOrdersWindow();
                break;
            throw new System.ArgumentNullException("Window with type " + _windowType + " doesn't exist!");
        }
    }
}