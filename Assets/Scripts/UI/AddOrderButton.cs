using I2.Loc;
using TMPro;
using UnityEngine;

public class AddOrderButton : TutorialButton
{
    [SerializeField] Delivery delivery;

    [SerializeField] private TextMeshProUGUI _buttonText;

    public override void Init()
    {
        base.Init();
        SetText("Deliver");
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        AddOrder();
    }

    private void AddOrder()
    {
        _buttonService.AddDeliveryOrder(delivery);
    }

    private void SetText(string textKey)
    {
        _buttonText.text = LocalizationManager.GetTranslation(textKey);
    }
}