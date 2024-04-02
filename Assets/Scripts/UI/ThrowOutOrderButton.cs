public class ThrowOutOrderButton : OrdersControlButton
{
    protected override void ClickCallback()
    {
        base.ClickCallback();
        CancelOrder();
    }

    public void CancelOrder()
    {
        _buttonService.ThrowOut(_order);
    }
}