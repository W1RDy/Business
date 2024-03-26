using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        SetText("Applied");
    }
}
