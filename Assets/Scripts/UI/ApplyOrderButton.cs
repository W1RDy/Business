using System.Collections;
using System.Collections.Generic;

public class ApplyOrderButton : OrdersControlButton
{
    protected override void Awake()
    {
        base.Awake();
        SetText("Apply");
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        ApplyOrder();
    }

    private void ApplyOrder()
    {
        _buttonService.ApplyOrder(_order);
        _button.interactable = false;
        SetText("Applied");
    }
}