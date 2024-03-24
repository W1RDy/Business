using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowActivator : IService
{
    private WindowService _windowService;

    public WindowActivator()
    {
        _windowService = ServiceLocator.Instance.Get<WindowService>();
    }

    public void ActivateWindow(WindowType windowType)
    {
        var window = _windowService.GetWindow(windowType);

        if (!window.gameObject.activeInHierarchy)
        {
            window.ActivateWindow();
        }
    }

    public void DeactivateWindow(WindowType windowType)
    {
        var window = _windowService.GetWindow(windowType);

        if (window.gameObject.activeInHierarchy)
        {
            window.DeactivateWindow();
        }
    }
}
