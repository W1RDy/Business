using UnityEngine;

public class Tab : OpenButton
{
    public override void Init()
    {
        base.Init();
        (_window as WindowWithCallbacks).WindowChanged += ChangeInteractableDelegate;
        if (_windowType == WindowType.DeliveryWindow) OpenWindow();
    }

    protected override void OpenWindow()
    {
        if (_windowType == WindowType.InventoryWindow) _buttonService.OpenInventoryWindow();
        else _buttonService.OpenDeliveryWindow();
    }

    private void ChangeInteractableDelegate()
    {
        Debug.Log("ChangeInteractable");
        ChangeTabInteractable(!_window.gameObject.activeSelf);
    }

    public void ChangeTabInteractable(bool isInteractable)
    {
        _button.interactable = isInteractable;
    }

    public void OnDestroy()
    {
        (_window as WindowWithCallbacks).WindowChanged -= ChangeInteractableDelegate;
    }
}