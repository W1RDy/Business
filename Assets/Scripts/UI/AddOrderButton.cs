using I2.Loc;
using TMPro;
using UnityEngine;

public class AddOrderButton : TutorialButton
{
    [SerializeField] Delivery delivery;

    [SerializeField] private TextMeshProUGUI _buttonText;

    private ButtonTextFitter _buttonTextFitter;

    public override void Init()
    {
        base.Init();
        _buttonTextFitter = new ButtonTextFitter(_button.GetComponent<RectTransform>(), 15, _buttonText.fontSize);
    }

    private void OnEnable()
    {
        SetText("Deliver");
        _buttonTextFitter.CheckButtonTextFit(_buttonText);
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