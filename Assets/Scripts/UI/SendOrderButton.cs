using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendOrderButton : OrdersControlButton
{
    protected override void Awake()
    {
        base.Awake();
        SetText("Send");
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        SendOrder();
    }

    private void SendOrder()
    {
        _buttonService.SendOrder(_order);
    }
}
