using System;

public class WindowWithCallbacks : Window
{
    public event Action WindowChanged;

    public override void ActivateWindow()
    {
        base.ActivateWindow();
        WindowChanged?.Invoke();
    }

    public override void DeactivateWindow()
    {
        base.DeactivateWindow();
        WindowChanged?.Invoke();
    }
}