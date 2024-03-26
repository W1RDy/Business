﻿public class CloseButton : WindowControlButton
{
    protected override void ClickCallback()
    {
        base.ClickCallback();
        CloseWindow();
    }

    private void CloseWindow()
    {
        switch (_windowType)
        {
            case WindowType.PeriodFinish:
                _buttonService.ClosePeriodWindow();
                break;
            case WindowType.OrdersWindow:
                _buttonService.CloseOrdersWindow();
                break;
            throw new System.ArgumentNullException("Window with type " + _windowType + " doesn't exist!");
        }
    }
}
