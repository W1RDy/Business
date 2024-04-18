using UnityEngine;

public class AddOrderButton : TutorialButton
{
    [SerializeField] Delivery delivery;

    protected override void ClickCallback()
    {
        base.ClickCallback();
        AddOrder();
    }

    private void AddOrder()
    {
        _buttonService.AddDeliveryOrder(delivery);
    }
}