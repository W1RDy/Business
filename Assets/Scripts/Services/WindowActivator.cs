using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowActivator : IService
{
    private WindowService _windowService;
    private WindowStackController _windowStackController;

    public WindowActivator()
    {
        _windowService = ServiceLocator.Instance.Get<WindowService>();
        _windowStackController = new WindowStackController(2);
    }

    public void ActivateWindow(WindowType windowType)
    {
        var window = _windowService.GetWindow(windowType);

        if (!window.gameObject.activeInHierarchy)
        {
            window.ActivateWindow();

            if (window.Type == WindowType.BasketWindow || window.Type == WindowType.DeliveryWindow || window.Type == WindowType.InventoryWindow) return;
            if (!_windowStackController.StackIsEmpty()) _windowStackController.PeekWindow().DeactivateWindow();
            _windowStackController.AddWindowToStack(window);
        }
    }

    public void DeactivateWindow(WindowType windowType)
    {
        var window = _windowService.GetWindow(windowType);

        if (window.gameObject.activeInHierarchy)
        {
            window.DeactivateWindow();

            if (window.Type == WindowType.BasketWindow || window.Type == WindowType.DeliveryWindow || window.Type == WindowType.InventoryWindow) return;

            _windowStackController.PopWindow();
            if (!_windowStackController.StackIsEmpty()) _windowStackController.PeekWindow().ActivateWindow();
        }
    }
}
