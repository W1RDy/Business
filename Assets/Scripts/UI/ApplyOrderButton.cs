using System.Collections;
using System.Collections.Generic;

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
        ChangeState(true);
    }

    public void ChangeState(bool isApplied)
    {
        _button.interactable = !isApplied;
        var text = isApplied ? "Applied" : "Apply";
        SetText(text);
    }
}