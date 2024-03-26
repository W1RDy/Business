using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendOrderButton : OrdersControlButton
{
    private void Awake()
    {
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
