using TMPro;
using UnityEngine;

public class HandsCoinsButton : CustomButton
{
    [SerializeField] private int _coinsValue;

    [SerializeField] private TextMeshProUGUI _buttonText;

    protected override void Init()
    {
        base.Init();

        SetText();
    }

    protected override void ClickCallback()
    {
        base.ClickCallback();
        _buttonService.RemoveHandsCoins(_coinsValue);
    }

    private void SetText()
    {
        _buttonText.text = "Waste " + _coinsValue + " coins";
    }
}
