using System;

public class WindowWithCallbacks : Window
{
    public event Action WindowActivated;

    public override void ActivateWindow()
    {
        base.ActivateWindow();
        WindowActivated?.Invoke();
    }
}