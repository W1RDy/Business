﻿using UnityEngine;

public class Tab : OpenButton
{
    public override void Init()
    {
        base.Init();
        (_window as WindowWithCallbacks).WindowActivated += ChangeInteractableDelegate;
        if (_windowType == WindowType.DeliveryWindow) OpenWindow();
    }

    protected override void OpenWindow()
    {
        if (_windowType == WindowType.InventoryWindow) _buttonService.OpenInventoryWindow();
        else _buttonService.OpenDeliveryWindow();
    }

    private void ChangeInteractableDelegate()
    {
        ChangeTabInteractable(_window.gameObject.activeSelf);
    }

    public void ChangeTabInteractable(bool isInteractable)
    {
        _button.interactable = isInteractable;
    }
}