using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyOrderButton : OrdersControlButton
{
    protected override void Start()
    {
        base.Start();
        ChangeState(false);
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        ApplyOrder();
    }

    private void ApplyOrder()
    {
        _buttonService.ApplyOrder(_order);
    }

    public void ChangeState(bool isApplied)
    {
        if (_button == null) Start();
        _button.interactable = !isApplied;
        var text = isApplied ? "Applied" : "Apply";
        SetText(text);
    }
}
