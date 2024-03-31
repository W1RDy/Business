using System.Collections;
using System.Collections.Generic;

public class ApplyOrderButton : OrdersControlButton
{
    private void Awake()
    {
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
